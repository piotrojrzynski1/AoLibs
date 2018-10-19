using System.Net.Http;
using AoLibs.HttpHelper.ContentSerialization;
using AoLibs.HttpHelper.Interfaces;
using AoLibs.Sample.Shared.Statics;
using Autofac;

namespace AoLibs.Sample.Shared.BL.HttpCommunication
{
    public class FancyApiDefinition : IApiDefinition
    {
        public string BaseAddress => "https://jsonplaceholder.typicode.com/";

        public void AddHeaders(HttpRequestMessage message)
        {
             message.Headers.TryAddWithoutValidation("Accept", "application/json");
        }

        public IApiResponseHandler GetResponseHandler()
        {
            using (var scope = ResourceLocator.ObtainScope())
                return scope.Resolve<FancyApiResponseHandler>();
        }

        public IHttpContentSerializer GetContentSerializer()
        {
            using (var scope = ResourceLocator.ObtainScope())
                return scope.Resolve<JsonContentSerializer>();
        }

        public IApiExceptionHandler GetExceptionHandler()
        {
            using (var scope = ResourceLocator.ObtainScope())
                return scope.Resolve<DefaultApiExceptionHandler>();
        }

        public IHttpContentDeserializer GetContentDeserializer()
        {
            using (var scope = ResourceLocator.ObtainScope())
                return scope.Resolve<JsonContentDeserializer>();
        }
    }
}
