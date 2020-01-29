using GraphQL.Types;

namespace Calendar.API.Models.Inputs
{
    public class GraphQLUserConfirmInput : InputObjectGraphType
    {
        public GraphQLUserConfirmInput()
        {
            Name = "ConfirmUserInput";
            Field<NonNullGraphType<StringGraphType>>("email");
            Field<NonNullGraphType<StringGraphType>>("confirmationCode");
        }
    }
}
