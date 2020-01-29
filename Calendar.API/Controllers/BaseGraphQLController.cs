using System;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Calendar.API.Models.Common;

namespace Calendar.API.Controllers
{
    [Route("[controller]")]
    public abstract class BaseGraphQLController : Controller
    {
        protected readonly DocumentExecuter _documentExecuter;
        protected readonly ISchema _schema;
        protected readonly ILogger _logger;

        public BaseGraphQLController(ISchema schema, DocumentExecuter documentExecuter, ILogger<BaseGraphQLController> logger)
        {
            _schema = schema;
            _documentExecuter = documentExecuter;
            _logger = logger;
        }

        protected async Task<IActionResult> RunQuery(GraphQLQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            var inputs = query.Variables.ToInputs();

            var executionOptions = new ExecutionOptions
            {
                Schema = _schema,
                Query = query.Query,
                Inputs = inputs
            };

            var result = await _documentExecuter.ExecuteAsync(executionOptions);

            if (result.Errors?.Count > 0)
            {
                #if !DEBUG
                foreach(var error in result.Errors)
                    _logger.LogError(error, $"Something wrong while processing the GraphQL request.");
                #endif

                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}