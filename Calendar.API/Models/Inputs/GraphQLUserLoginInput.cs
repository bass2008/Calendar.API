using GraphQL.Types;

namespace Calendar.API.Models.Inputs
{
    public class GraphQLUserLoginInput : InputObjectGraphType
    {
        public GraphQLUserLoginInput()
        {
            Name = "LoginInput";
            Field<NonNullGraphType<StringGraphType>>("email");
            Field<NonNullGraphType<StringGraphType>>("password");
            Field<StringGraphType>("newPasswordIfRequired");
        }
    }
}
