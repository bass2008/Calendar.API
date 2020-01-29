using GraphQL.Types;

namespace Calendar.API.Models.Inputs
{
    public class GraphQLAdminChangeUserPasswordInput : InputObjectGraphType
    {
        public GraphQLAdminChangeUserPasswordInput()
        {
            Name = "AdminChangeUserPasswordInput";
            Field<NonNullGraphType<StringGraphType>>("email");
            Field<NonNullGraphType<StringGraphType>>("password");
        }
    }
}
