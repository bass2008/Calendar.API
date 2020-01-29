using GraphQL.Types;

namespace Calendar.API.Models.Inputs
{
    public class GraphQLTabInput : InputObjectGraphType
    {
        public GraphQLTabInput()
        {
            Name = "TabInput";
            Field<NonNullGraphType<StringGraphType>>("name");
            Field<NonNullGraphType<StringGraphType>>("logo");
        }
    }
}
