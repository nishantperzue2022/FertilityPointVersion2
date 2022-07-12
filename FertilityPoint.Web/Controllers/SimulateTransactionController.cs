using FertilityPoint.DTO.MpesaStkModule;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace FertilityPoint.Web.Controllers
{
    public class SimulateTransactionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }











        public async void Simulate()
        {
            Console.Title = "C2B Simulation";

            String a = "https://sandbox.safaricom.co.ke/mpesa/c2b/v1/simulate";

            string baseUrl = a;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUrl);

            string token = await GenerateAccessToken();

            request.Headers.Add("authorization", "Bearer " + token);

            request.ContentType = "application/json";

            request.Headers.Add("cache-control", "no-cache");

            request.KeepAlive = false;

            request.Method = "POST";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = "{\"ShortCode\":\"600000\"," +
                              "\"CommandID\":\"CustomerPayBillOnline\"," +
                              "\"Amount\":\"1\"," +
                              "\"Msisdn\":\"254708374149\"," +
                              "\"BillRefNumber\":\"Test\"}";

                streamWriter.Write(json);

                streamWriter.Flush();

                streamWriter.Close();
            }

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                // Get the stream associated with the response.
                Stream receiveStream = response.GetResponseStream();

                // Pipes the stream to a higher level stream reader with the required encoding format. 
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

                Console.WriteLine(readStream.ReadToEnd());

                response.Close();

                readStream.Close();

            }
            catch (WebException ex)
            {
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();

                Console.WriteLine(resp);
            }


        }

        public async Task<string> GenerateAccessToken()
        {
            try
            {
                var key = "5CAjwH782Y2gKvDfUQvAXuDDjKkXBJTe";

                var secrete = "25CuZHuXfaG3gTij";

                var url = @"https://sandbox.safaricom.co.ke/oauth/v1/generate?grant_type=client_credentials";

                HttpClient client = new HttpClient();

                var byteArray = Encoding.ASCII.GetBytes(key + ":" + secrete);

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                HttpResponseMessage response = await client.GetAsync(url);

                HttpContent content = response.Content;

                string result = await content.ReadAsStringAsync();

                var mpesaAccessToken = JsonConvert.DeserializeObject<MpesaAccessTokenDTO>(result);

                return mpesaAccessToken.access_token;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
    }
}
