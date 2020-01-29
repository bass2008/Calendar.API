namespace Calendar.Domain
{
    public interface IUserOwnedElement : IDbElement
    {
        int UserId { get; set; }
    }
}
