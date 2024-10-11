namespace ChessGameWebApp.Server.Services
{
    public interface IConnectionService
    {
        Task Add(int accountId, string connectionId);
        Task Remove(string connectionId);
        Task<IReadOnlyList<string>> GetConnections(int[] accountIds);
    }
}
