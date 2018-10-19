using System.Net.Http;
using AoLibs.HttpHelper.Interfaces;

namespace AoLibs.HttpHelper
{
    public static class HttpHelperConfiguration
    {
        internal static IApiCommunicator ApiCommunicator;
        internal static IRequestExceptionHandler ExceptionHandler;

        public static void Setup(
            int defaultRequestTimeout,
            HttpMessageHandler customHttpMessageHandler = null)
        {
            ExceptionHandler = new RequestExceptionHandler();

            ApiCommunicator = new ApiCommunicator(
                ExceptionHandler,
                customHttpMessageHandler,
                defaultRequestTimeout
            );
        }
    }
}
