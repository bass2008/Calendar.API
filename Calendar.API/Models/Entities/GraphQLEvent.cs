using GraphQL.Types;
using Calendar.Domain.Models;

namespace Calendar.API.Models.Entities
{
    public class GraphQLEvent : ObjectGraphType<Event>
    {
        public GraphQLEvent()
        {
            Field(x => x.Id);
            Field(x => x.Description);
            Field(x => x.Notification);
            Field(x => x.Start);
            Field(x => x.End);
            Field(x => x.Repeat);
            Field(x => x.UserId);
            Field(x => x.Title);
            Field(x => x.TabId);
        }
    }
}
