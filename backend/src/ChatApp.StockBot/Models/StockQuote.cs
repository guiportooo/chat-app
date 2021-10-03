namespace ChatApp.StockBot.Models
{
    public class StockQuote
    {
        public string Symbol { get; set; } = null!;
        public string Date { get; set; } = null!;
        public string Time { get; set; } = null!;
        public string Open { get; set; } = null!;
        public string High { get; set; } = null!;
        public string Low { get; set; } = null!;
        public string Close { get; set; } = null!;
        public string Volume { get; set; } = null!;
    }
}