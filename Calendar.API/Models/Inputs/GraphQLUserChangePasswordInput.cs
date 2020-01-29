using GraphQL.Types;

namespace Calendar.API.Models.Inputs
{
    public class GraphQLUserChangePasswordInput : InputObjectGraphType
    {
        public GraphQLUserChangePasswordInput()
        {
            Name = "UserChangePasswordInput";
            Field<NonNullGraphType<StringGraphType>>("token");
            Field<NonNullGraphType<StringGraphType>>("oldPassword");
            Field<NonNullGraphType<StringGraphType>>("newPassword");
        }
    }
}
