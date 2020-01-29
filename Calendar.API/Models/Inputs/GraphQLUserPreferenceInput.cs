using GraphQL.Types;

namespace Calendar.API.Models.Inputs
{
    public class GraphQLUserPreferenceInput : InputObjectGraphType
    {
        public GraphQLUserPreferenceInput()
        {
            Name = "UserPreferenceInput";
            Field<NonNullGraphType<BooleanGraphType>>("emailChecked");
            Field<NonNullGraphType<BooleanGraphType>>("smsChecked");
            Field<NonNullGraphType<IntGraphType>>("periodicity");
        }
    }
}
