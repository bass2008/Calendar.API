using GraphQL;
using GraphQL.Types;

namespace Calendar.API.Models.Public
{
    public interface IPublicSchema : ISchema { }

    public class PublicCalendarSchema : Schema, IPublicSchema
    {
        public PublicCalendarSchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<PublicCalendarQuery>();
            Mutation = resolver.Resolve<PublicCalendarMutation>();
        }
    }
}
