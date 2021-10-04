namespace ChatApp.StockBot.HttpOut
{
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    public interface IStooqHttpClient
    {
        Task<string> GetStockQuote(string stockCode);
    }

    public class StooqHttpClient : IStooqHttpClient
    {
        private readonly HttpClient _client;

        public StooqHttpClient(HttpClient client) => _client = client;

        public async Task<string> GetStockQuote(string stockCode)
        {
            var response = await _client.GetAsync($"q/l/?s={stockCode.ToLower()}&f=sd2t2ohlcv&h&e=csv");

            if (!response.IsSuccessStatusCode)
                return string.Empty;

            var byteArray = await response.Content.ReadAsByteArrayAsync();
            return Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);
        }
    }
}