using System.Threading.Tasks;
using GraphQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Calendar.API.Models.Common;
using Calendar.API.Models.Public;
using StackExchange.Profiling;
using Microsoft.AspNetCore.Http;

namespace Calendar.API.Controllers
{
    [Route("/graphql")]
    public class PublicGraphQLController : BaseGraphQLController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PublicGraphQLController(
            IHttpContextAccessor httpContextAccessor,
            IPublicSchema schema, DocumentExecuter documentExecuter, ILogger<BaseGraphQLController> logger)
            : base(schema, documentExecuter, logger)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [AllowAnonymous]
        [Authorize(Policy = "JwtBearer")]
        public async Task<IActionResult> Post([FromBody] GraphQLQuery query)
        {
            return await MiniProfiler.Current.Inline(async () => await RunQuery(query), "GraphQL Body");
        }
    }
}