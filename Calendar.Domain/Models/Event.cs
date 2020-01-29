namespace Calendar.Domain.Models
{
    public class Event : IDbElement, IUserOwnedElement
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Start { get; set; }

        public int End { get; set; }

        public int Repeat { get; set; }

        public int Notification { get; set; }

        public int TabId { get; set; }

        public Tab Tab { get; set; }
    }
}
