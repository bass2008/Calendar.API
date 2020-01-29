using GraphQL.Types;

namespace Calendar.API.Models.Inputs
{
    public class GraphQLUserConfirmForgotPasswordInput : InputObjectGraphType
    {
        public GraphQLUserConfirmForgotPasswordInput()
        {
            Name = "ConfirmForgotPasswordInput";
            Field<NonNullGraphType<StringGraphType>>("email");
            Field<NonNullGraphType<StringGraphType>>("confirmationCode");
            Field<NonNullGraphType<StringGraphType>>("newPassword");
        }
    }
}
