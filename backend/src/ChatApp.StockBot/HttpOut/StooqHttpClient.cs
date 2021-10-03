namespace ChatApp.StockBot.HttpOut
{
    using System.Threading.Tasks;

    public interface IStooqHttpClient
    {
        Task<string> GetStockQuote(string stockCode);
    }

    public class StooqHttpClient : IStooqHttpClient
    {
        public Task<string> GetStockQuote(string stockCode) =>
            Task.FromResult(
                $"Symbol,Date,Time,Open,High,Low,Close,Volume\r\n{stockCode},2021-10-01,22:00:18,141.9,142.92,139.1101,142.65,94639581");
    }
}