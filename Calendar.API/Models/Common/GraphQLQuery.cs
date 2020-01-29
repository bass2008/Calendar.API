using Newtonsoft.Json.Linq;

namespace Calendar.API.Models.Common
{
    public class GraphQLQuery
    {
        public string OperationName { get; set; }
        public string NamedQuery { get; set; }
        public string Query { get; set; }
        public JObject Variables { get; set; } = new JObject(); //https://github.com/graphql-dotnet/graphql-dotnet/issues/389
    }
}
