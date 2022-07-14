namespace FertilityPoint.PayPal.PaypalHelper
{
    public class PayPalAccessToken
    {
        public string Scope { get; set; }
        public string access_token { get; set; }
        public string TokenType { get; set; }
        public string AppId { get; set; }
        public int ExpiresIn { get; set; }
        public string Nonce { get; set; }
    }
}
