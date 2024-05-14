namespace SubRedditAPI.Interfaces
{
    public interface IRedditRepository
    {
        Task<string> GetSubRedditAsync();
    }
}
