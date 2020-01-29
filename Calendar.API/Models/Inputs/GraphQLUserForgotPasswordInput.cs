using GraphQL.Types;

namespace Calendar.API.Models.Inputs
{
    public class GraphQLUserForgotPasswordInput : InputObjectGraphType
    {
        public GraphQLUserForgotPasswordInput()
        {
            Name = "ForgotPasswordInput";
            Field<NonNullGraphType<StringGraphType>>("email");
        }
    }
}
