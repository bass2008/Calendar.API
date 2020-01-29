using Calendar.API.Decorators;
using Calendar.API.Models.Common;
using Calendar.API.Models.Entities;
using Calendar.DAL.Interfaces;
using Calendar.Domain.Models;

namespace Calendar.API.Models.Public
{
    public class PublicCalendarQuery : BaseCalendarQuery
    {
        public PublicCalendarQuery(
            CalendarRequestDecorator requestDecorator,
            IUserOwnedGenericRepository<Tab> tabRepository,
            IUserOwnedGenericRepository<Event> eventRepository,
            IUserOwnedGenericRepository<UserPreference> userPreferenceRepository,
            IGenericRepository<User> userRepository) : base(requestDecorator)
        {
            AddFieldAsync<Tab, GraphQLTab>("tab", "tabs", tabRepository);
            AddFieldAsync<Event, GraphQLEvent>("event", "events", eventRepository);
            AddFieldAsync<User, GraphQLUser>("user", "users", userRepository);
            AddSingleFieldAsync<UserPreference, GraphQLUserPreference>("userPreference", userPreferenceRepository);
        }
    }
}
