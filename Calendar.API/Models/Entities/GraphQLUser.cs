using GraphQL.Types;
using Calendar.DAL.Interfaces;
using Calendar.Domain.Models;

namespace Calendar.API.Models.Entities
{
    public class GraphQLUser : ObjectGraphType<User>
    {
        public GraphQLUser(IUserRepository repository)
        {
            Field(x => x.Id);
            Field(x => x.Email);
            Field(x => x.Name, nullable: true);
            Field(x => x.Image, nullable: true);
            Field(x => x.Phone, nullable: true);
            Field<NonNullGraphType<GraphQLUserPreference>>("userPreference",
                arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
                resolve: context =>
                {
                    return repository.GetUserPreferences(context.Source.Id);
                });
        }
    }
}
