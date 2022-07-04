using FertilityPoint.DTO.MpesaC2BModule;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Polly;
using Polly.Extensions.Http;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FertilityPoint.Web.Services.MpesaC2BModule
{
    public class MpesaClient : IMpesaClient
    {
        private readonly HttpClient _client;

        private readonly JsonSerializer _serializer = new JsonSerializer();

        readonly Random jitterer = new Random();
        public MpesaClient(HttpClient client)
        {
            _client = client;
        }
        public MpesaClient(Enums.Environment environment)
        {
            var retryPolicy = HttpPolicyExtensions.HandleTransientHttpError()
                .WaitAndRetryAsync(1, retryAttempt =>
                {
                    return TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(jitterer.Next(0, 100));
                });

            var noOpPolicy = Policy.NoOpAsync().AsAsyncPolicy<HttpResponseMessage>();

            var services = new ServiceCollection();
            services.AddHttpClient("MpesaApiClient", client =>
            {
                switch (environment)
                {
                    case Enums.Environment.Sandbox:
                        client.BaseAddress = MpesaRequestEndpoint.SandboxBaseAddress;
                        client.Timeout = TimeSpan.FromMinutes(10);
                        break;

                    case Enums.Environment.Live:
                        client.BaseAddress = MpesaRequestEndpoint.LiveBaseAddress;
                        client.Timeout = TimeSpan.FromMinutes(10);
                        break;

                    default:
                        break;
                }
            }).ConfigurePrimaryHttpMessageHandler(messageHandler =>
            {
                var handler = new HttpClientHandler();

                if (handler.SupportsAutomaticDecompression)
                {
                    handler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                }

                return handler;
            }).AddPolicyHandler(request => request.Method.Equals(HttpMethod.Get) ? retryPolicy : noOpPolicy);

            var serviceProvider = services.BuildServiceProvider();

            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

            _client = httpClientFactory.CreateClient("MpesaApiClient");
        }



        public async Task<MpesaResponse> MakeC2BPayment(CustomerToBusinessSimulate customerToBusinessSimulate, string accesstoken, string mpesaRequestEndpoint, CancellationToken cancellationToken = default)
        {
            var validator = new CustomerToBusinessSimulateTransactionValidator();

            var results = await validator.ValidateAsync(customerToBusinessSimulate, cancellationToken);

            return !results.IsValid
                ? throw new MpesaAPIException(HttpStatusCode.BadRequest, string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage.ToString())))
                : await MpesaPostRequestAsync<MpesaResponse>(customerToBusinessSimulate, accesstoken, mpesaRequestEndpoint, cancellationToken);

        }

        public async Task<MpesaResponse> RegisterC2BUrl(CustomerToBusinessRegisterUrl customerToBusinessRegisterUrl, string accesstoken, string mpesaRequestEndpoint, CancellationToken cancellationToken = default)
        {
            var validator = new CustomerToBusinessRegisterUrlValidator();

            var results = await validator.ValidateAsync(customerToBusinessRegisterUrl, cancellationToken);

            return !results.IsValid

                ? throw new MpesaAPIException(HttpStatusCode.BadRequest, string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage.ToString())))

                : await MpesaPostRequestAsync<MpesaResponse>(customerToBusinessRegisterUrl, accesstoken, mpesaRequestEndpoint, cancellationToken);
        }

        private async Task<T> MpesaPostRequestAsync<T>(object mpesaDto, string accessToken, string mpesaEndpoint, CancellationToken cancellationToken = default) where T : new()
        {


            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            T result = new();
            string json = JsonConvert.SerializeObject(mpesaDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            cancellationToken.ThrowIfCancellationRequested();
            var response = await _client.PostAsync(mpesaEndpoint, content, cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
#if NET5_0_OR_GREATER
                await response.Content.ReadAsStreamAsync(cancellationToken).ContinueWith((Task<Stream> stream) =>
                {
                    using var reader = new StreamReader(stream.Result);
                    using var json = new JsonTextReader(reader);
                    result = _serializer.Deserialize<T>(json);
                }, cancellationToken);
#endif
#if NETSTANDARD2_0_OR_GREATER
                await response.Content.ReadAsStreamAsync().ContinueWith((Task<Stream> stream) =>
                {
                    using var reader = new StreamReader(stream.Result);
                    using var json = new JsonTextReader(reader);
                    result = _serializer.Deserialize<T>(json);
                }, cancellationToken);
#endif
            }
            else
            {
                MpesaErrorResponse mpesaErrorResponse = new MpesaErrorResponse();
#if NET5_0_OR_GREATER
                await response.Content.ReadAsStreamAsync(cancellationToken).ContinueWith((Task<Stream> stream) =>
                {
                    using var reader = new StreamReader(stream.Result);
                    using var json = new JsonTextReader(reader);
                    mpesaErrorResponse = _serializer.Deserialize<MpesaErrorResponse>(json);
                }, cancellationToken);
                throw new MpesaAPIException(new HttpRequestException(mpesaErrorResponse.ErrorMessage), response.StatusCode, mpesaErrorResponse);
#endif
#if NETSTANDARD2_0_OR_GREATER
                await response.Content.ReadAsStreamAsync().ContinueWith((Task<Stream> stream) =>
                {
                    using var reader = new StreamReader(stream.Result);
                    using var json = new JsonTextReader(reader);
                    mpesaErrorResponse = _serializer.Deserialize<MpesaErrorResponse>(json);
                }, cancellationToken);
                throw new MpesaAPIException(new HttpRequestException(mpesaErrorResponse.ErrorMessage), response.StatusCode, mpesaErrorResponse);
#endif
            }
            return result;
        }

    }
}
