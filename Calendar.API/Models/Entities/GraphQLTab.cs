using GraphQL.Types;
using Calendar.Domain.Models;
using Calendar.DAL.Interfaces;
using Calendar.DAL;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Calendar.API.Models.Entities
{
    public class GraphQLTab : ObjectGraphType<Tab>
    {
        public GraphQLTab(CalendarDbContext dbContext)
        {
            Field(x => x.Id);
            Field(x => x.Name);
            Field(x => x.Logo, nullable: true);
            FieldAsync<NonNullGraphType<ListGraphType<NonNullGraphType<GraphQLEvent>>>>("events",
                arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
                resolve: async context =>
                {
                    return await dbContext.Events.Where(x => x.TabId == context.Source.Id).ToListAsync();
                });
        }
    }
}
