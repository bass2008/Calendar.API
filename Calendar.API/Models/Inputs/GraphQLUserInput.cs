using GraphQL.Types;

namespace Calendar.API.Models.Inputs
{
    public class GraphQLUserInput : InputObjectGraphType
    {
        public GraphQLUserInput()
        {
            Name = "UserInput";
            Field<StringGraphType>("phone");
            Field<StringGraphType>("name");
            Field<StringGraphType>("image");

        }
    }
}
