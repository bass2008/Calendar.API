using GraphQL.Types;

namespace Calendar.API.Models.Inputs
{
    public class GraphQLEventInput : InputObjectGraphType
    {
        public GraphQLEventInput()
        {
            Name = "EventInput";
            Field<NonNullGraphType<StringGraphType>>("title");
            Field<NonNullGraphType<StringGraphType>>("description");
            Field<NonNullGraphType<IntGraphType>>("start");
            Field<NonNullGraphType<IntGraphType>>("end");
            Field<NonNullGraphType<IntGraphType>>("repeat");
            Field<NonNullGraphType<IntGraphType>>("notification");
            Field<NonNullGraphType<IntGraphType>>("tabId");
        }
    }
}
