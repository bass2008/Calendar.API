using GraphQL.Types;

namespace Calendar.API.Models.Inputs
{
    public class GraphQLAdminInviteUserInput : InputObjectGraphType
    {
        public GraphQLAdminInviteUserInput()
        {
            Name = "AdminInviteUserInput";
            Field<NonNullGraphType<StringGraphType>>("email");
        }
    }
}
