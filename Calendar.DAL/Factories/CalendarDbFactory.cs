using Calendar.Domain.Services;

namespace Calendar.DAL.Factories
{
    public class CalendarDbFactory
    {
        private readonly SecretManager _secretManager;

        public CalendarDbFactory(SecretManager secretManager)
        {
            _secretManager = secretManager;
        }

        public CalendarDbContext CreateDbContext()
        {
            return new CalendarDbContext(_secretManager);
        }
    }
}
