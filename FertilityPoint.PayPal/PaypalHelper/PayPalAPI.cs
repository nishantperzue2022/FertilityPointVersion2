
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;

namespace FertilityPoint.PayPal.PaypalHelper
{
    public class PayPalAPI
    {
        public IConfiguration configuration { get; }
        public PayPalAPI(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public string GetRedirectUrlToPayPal(double total, string currency)
        {
            try
            {
                return Task.Run(async () =>
                {
                    HttpClient http = GetPaypalHttpClient();

                    // Step 1: Get an access token
                    PayPalAccessToken accessToken = await GetPayPalAccessTokenAsync(http);

                    Log.Information("Access Token \n{@accessToken}", accessToken);

                    // Step 2: Create the payment
                    PayPalPaymentCreatedResponse createdPayment = await CreatePaypalPaymentAsync(http, accessToken, total, currency);

                    Log.Information("Created payment \n{@createdPayment}", createdPayment);

                    // Step 3: Get the approval_url and paste it into a browser
                    // It should look something like this: https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_express-checkout&token=EC-97965369EL8295114

                    var approval_url = createdPayment.links.First(x => x.rel == "approval_url").href;

                    return approval_url;

                }).Result;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, "Failed to login to paypal");

                return null;
            }
        }
        public string GetRedirectUrlToPayPal1(double total, string currency)
        {
            try
            {
                Task.Run(async () =>
                {
                    HttpClient http = GetPaypalHttpClient();

                    // Step 1: Get an access token
                    PayPalAccessToken accessToken = await GetPayPalAccessTokenAsync(http);

                    Log.Information("Access Token \n{@accessToken}", accessToken);

                    // Step 2: Create the payment
                    PayPalPaymentCreatedResponse createdPayment = await CreatePaypalPaymentAsync(http, accessToken, total, currency);

                    Log.Information("Created payment \n{@createdPayment}", createdPayment);

                    // Step 3: Get the approval_url and paste it into a browser
                    // It should look something like this: https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_express-checkout&token=EC-97965369EL8295114

                    var approval_url = createdPayment.links.First(x => x.rel == "approval_url").href;

                    Log.Information("approval_url\n{approval_url}", approval_url);

                    //
                    // IMPORTANT: Stop the program here, and re-run only the section below (comment out Step 2 and Step 3) and paste in the correct paymentId and payerId
                    //

                    // Step 4: When paypal redirects to the return_url, we need to grab the PayerID and the paymentId and execute the payment
                    var paymentId = "PAY-9LN814307S704373KK6UFTHI";

                    var payerId = "LMWV7AASBDUQQ";

                    PayPalPaymentExecutedResponse executedPayment = await ExecutePaypalPaymentAsync(http, accessToken, paymentId, payerId);

                    Log.Information("Executed payment \n{@executedPayment}", executedPayment);

                }).Wait();

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to login to PalPal");

                return "1";
            }

            return ("0");
        }
        private async Task<PayPalPaymentCreatedResponse> CreatePaypalPaymentAsync(HttpClient http, PayPalAccessToken accessToken, double total, string currency)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "v1/payments/payment");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.access_token);

            var payment = JObject.FromObject(new
            {
                intent = "sale",

                redirect_urls = new
                {
                    return_url = configuration.GetValue<string>("PayPal:ReturnUrl"),

                    cancel_url = configuration.GetValue<string>("PayPal:CancelUrl")
                },
                payer = new { payment_method = "paypal" },

                transactions = JArray.FromObject(new[]
                {
            new
            {
                amount = new
                {
                    total = total,

                    currency = currency
                }
            }
        })
            });

            request.Content = new StringContent(JsonConvert.SerializeObject(payment), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await http.SendAsync(request);

            string content = await response.Content.ReadAsStringAsync();

            PayPalPaymentCreatedResponse paypalPaymentCreated = JsonConvert.DeserializeObject<PayPalPaymentCreatedResponse>(content);

            return paypalPaymentCreated;
        }
        public HttpClient GetPaypalHttpClient()
        {
            string sandbox = configuration.GetValue<string>("PayPal:UrlAPI");

            var http = new HttpClient
            {
                BaseAddress = new Uri(sandbox),

                Timeout = TimeSpan.FromSeconds(30),
            };

            return http;
        }
        public async Task<PayPalPaymentExecutedResponse> ExecutedPayment(string paymentId, string payerId)
        {
            try
            {
                HttpClient http = GetPaypalHttpClient();

                PayPalAccessToken accessToken = await GetPayPalAccessTokenAsync(http);

                return await ExecutePaypalPaymentAsync(http, accessToken, paymentId, payerId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, "Failed to login to paypal");

                return null;
            }
        }
        private async Task<PayPalPaymentExecutedResponse> ExecutePaypalPaymentAsync(HttpClient http, PayPalAccessToken accessToken, string paymentId, string payerId)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"v1/payments/payment/{paymentId}/execute");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.access_token);

            var payment = JObject.FromObject(new
            {
                payer_id = payerId
            });

            request.Content = new StringContent(JsonConvert.SerializeObject(payment), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await http.SendAsync(request);

            string content = await response.Content.ReadAsStringAsync();

            PayPalPaymentExecutedResponse executedPayment = JsonConvert.DeserializeObject<PayPalPaymentExecutedResponse>(content);
            return executedPayment;
        }
        private async Task<PayPalAccessToken> GetPayPalAccessTokenAsync(HttpClient http)
        {
            var clientId = configuration.GetValue<string>("PayPal:ClientId");

            var secret = configuration.GetValue<string>("PayPal:Secret");

            byte[] bytes = Encoding.GetEncoding("iso-8859-1").GetBytes($"{clientId}:{secret}");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/v1/oauth2/token");

            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(bytes));

            var form = new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials"
            };

            request.Content = new FormUrlEncodedContent(form);

            HttpResponseMessage response = await http.SendAsync(request);

            string content = await response.Content.ReadAsStringAsync();

            PayPalAccessToken accessToken = JsonConvert.DeserializeObject<PayPalAccessToken>(content);

            return accessToken;
        }

    }
}
