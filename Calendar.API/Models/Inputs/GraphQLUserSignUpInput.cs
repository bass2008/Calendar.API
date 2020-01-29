using GraphQL.Types;

namespace Calendar.API.Models.Inputs
{
    public class GraphQLUserSignUpInput : InputObjectGraphType
    {
        public GraphQLUserSignUpInput()
        {
            Name = "SignUpInput";
            Field<NonNullGraphType<StringGraphType>>("email");
            Field<NonNullGraphType<StringGraphType>>("password");
        }
    }
}
