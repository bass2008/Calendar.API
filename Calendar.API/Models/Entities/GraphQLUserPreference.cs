using GraphQL.Types;
using Calendar.Domain.Models;

namespace Calendar.API.Models.Entities
{
    public class GraphQLUserPreference : ObjectGraphType<UserPreference>
    {
        public GraphQLUserPreference()
        {
            Field(x => x.EmailChecked);
            Field(x => x.SmsChecked);
            Field(x => x.Periodicity);
        }
    }
}
