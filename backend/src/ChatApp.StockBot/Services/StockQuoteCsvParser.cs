namespace ChatApp.StockBot.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Models;
    using TinyCsvParser;
    using TinyCsvParser.Mapping;

    public interface IStockQuoteCsvParser
    {
        StockQuote? Parse(string csv);
    }

    public class StockQuoteParser : IStockQuoteCsvParser
    {
        public StockQuote? Parse(string csv)
        {
            var csvParserOptions = new CsvParserOptions(true, ',');
            var csvReaderOptions = new CsvReaderOptions(new[] { Environment.NewLine });
            var csvMapper = new StockQuoteCsvMapping();
            var csvParser = new CsvParser<StockQuote>(csvParserOptions, csvMapper);

            var stockQuote = csvParser
                .ReadFromString(csvReaderOptions, csv)
                .Select(x => x.Result)
                .FirstOrDefault();

            return stockQuote;
        }

        private class StockQuoteCsvMapping : CsvMapping<StockQuote>
        {
            public StockQuoteCsvMapping() : base()
            {
                MapProperty(0, x => x.Symbol);
                MapProperty(1, x => x.Date);
                MapProperty(2, x => x.Time);
                MapProperty(3, x => x.Open);
                MapProperty(4, x => x.High);
                MapProperty(5, x => x.Low);
                MapProperty(6, x => x.Close);
                MapProperty(7, x => x.Volume);
            }
        }
    }
}