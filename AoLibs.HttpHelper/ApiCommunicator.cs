using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AoLibs.HttpHelper.Interfaces;
using AoLibs.HttpHelper.Models;

namespace AoLibs.HttpHelper
{
    public class ApiCommunicator : IApiCommunicator
    {
        private readonly IRequestExceptionHandler _exceptionHandler;

        private HttpClient _httpClient;

        public ApiCommunicator(
            IRequestExceptionHandler exceptionHandler,
            HttpMessageHandler customHttpMessageHandler,
            int timeoutMs)
        {
            _exceptionHandler = exceptionHandler;

            if (customHttpMessageHandler == null)
                _httpClient = new HttpClient();
            else
                _httpClient = new HttpClient(customHttpMessageHandler);

            _httpClient.Timeout = TimeSpan.FromSeconds(timeoutMs);
        }

        #region interface implementation

        public async Task<TypeResponse> Get<TypeResponse>(IApiDefinition api, string address, IApiExceptionHandler exceptionHandlerOverride)
        {
            return await ProcessRequest<TypeResponse>(api, address, HttpMethod.Get, HttpStatusCode.OK, exceptionHandlerOverride);
        }

        public async Task<TypeResponse> Post<TypeResponse>(IApiDefinition api, string address, IApiExceptionHandler exceptionHandlerOverride, IHttpContentSerializable content)
        {
            return await ProcessRequest<TypeResponse>(api, address, HttpMethod.Post, HttpStatusCode.OK, exceptionHandlerOverride, content);
        }

        #endregion

        #region private methods

        private async Task<TypeResponse> ProcessRequest<TypeResponse>(
            IApiDefinition api,
            string address,
            HttpMethod method,
            HttpStatusCode expectedResult,
            IApiExceptionHandler exceptionHandlerOverride = null,
            IHttpContentSerializable content = null)
        {
            var response = await ProcessRequestBase(api, address, method, exceptionHandlerOverride, content);

            if (response != null && response.StatusCode == expectedResult)
            {
                var stringContent = await response.Content.ReadAsStringAsync();
                return api.GetContentDeserializer().Deserialize<TypeResponse>(stringContent);
            }
            else
            {
                return default(TypeResponse);
            }
        }

        private async Task<HttpResponseMessage> ProcessRequestBase(
            IApiDefinition api,
            string address,
            HttpMethod method,
            IApiExceptionHandler exceptionHandlerOverride = null,
            IHttpContentSerializable content = null)
        {
            var fullAddress = api.BaseAddress + address;
            var message = new HttpRequestMessage(method, fullAddress);

            if (content != null)
                api.GetContentSerializer().AddContent(message, content);

            api.AddHeaders(message);

            var request = _httpClient.SendAsync(message, HttpCompletionOption.ResponseContentRead);
            var throwableTask = _exceptionHandler.Execute(request, address, api.GetResponseHandler());

            var exceptionHandler = api.GetExceptionHandler();

            if (exceptionHandlerOverride != null)
                exceptionHandler = exceptionHandlerOverride;

            var response = await _exceptionHandler.HandleExeptions(throwableTask, exceptionHandler);

            return response;
        }

        #endregion
    }
}
