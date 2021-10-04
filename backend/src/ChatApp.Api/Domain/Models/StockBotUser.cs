namespace ChatApp.Api.Domain.Models
{
    public class StockBotUser : User
    {
        public const string BotUserName = "stock.bot";

        public StockBotUser() : base(BotUserName, string.Empty)
        {
        }

        public override bool IsBot => true;
    }
}