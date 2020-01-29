using GraphQL.Types;
using Calendar.DAL.Interfaces;
using Calendar.Domain.Models;

namespace Calendar.API.Models.Entities
{
    public class GraphQLLoginInfo : ObjectGraphType<LoginInfo>
    {
        public GraphQLLoginInfo()
        {
            Field(x => x.Token);
            Field(x => x.ExpiresAt);
            Field<NonNullGraphType<GraphQLUser>>("user",
                resolve: context =>
                {
                    return context.Source.User;
                });
        }
    }
}
