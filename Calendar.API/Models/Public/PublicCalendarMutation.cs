using Calendar.API.Decorators;
using Calendar.API.Models.Common;
using Calendar.API.Models.Entities;
using Calendar.API.Models.Inputs;
using Calendar.API.Services;
using Calendar.DAL;
using Calendar.DAL.Interfaces;
using Calendar.Domain.Models;

namespace Calendar.API.Models.Public
{
    public class PublicCalendarMutation : BaseCalendarMutation
    {
        public PublicCalendarMutation(
            CalendarDbContext dbContext,
            CalendarRequestDecorator requestDecorator,
            IUserOwnedGenericRepository<Tab> tabRepository,
            IUserOwnedGenericRepository<Event> eventRepository,
            IUserOwnedGenericRepository<UserPreference> userPreferenceRepository,
            IUserRepository userRepository,
            CognitoService cognitoService) : base(requestDecorator)
        {
            AddFieldAsync<Tab, GraphQLTab, GraphQLTabInput>("Tab", tabRepository);
            AddFieldAsync<Event, GraphQLEvent, GraphQLEventInput>("Event", eventRepository);
            AddSingleFieldAsync<UserPreference, GraphQLUserPreference, GraphQLUserPreferenceInput>("UserPreference", userPreferenceRepository);

            AddUserAsync(userRepository, cognitoService, dbContext);
        }
    }
}
