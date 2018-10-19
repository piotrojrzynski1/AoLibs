using System;
using System.Threading.Tasks;
using AoLibs.HttpHelper.Interfaces;
using AoLibs.HttpHelper.Models;

namespace AoLibs.HttpHelper
{
    public class ApiControllerBase
    {
        protected virtual IApiDefinition ApiDefinition { get; }
        protected virtual string ControllerPrefix { get; }

        protected async Task<T> Get<T>(string address, IApiExceptionHandler exceptionHandlerOverride = null)
        {
            return await Execute(async () => await HttpHelperConfiguration.ApiCommunicator.Get<T>(
                    ApiDefinition,
                    ControllerPrefix + "/" + address,
                    exceptionHandlerOverride));
        }

        protected async Task<T> Post<T>(string address, IHttpContentSerializable model, IApiExceptionHandler exceptionHandlerOverride = null)
        {
            return await Execute(async () => await HttpHelperConfiguration.ApiCommunicator.Post<T>(
                    ApiDefinition,
                    ControllerPrefix + "/" + address,
                    exceptionHandlerOverride,
                    model));
        }

        private T Execute<T>(Func<T> request)
        {
            try
            {
                return request.Invoke();
            }
            catch (ApiCommunicatorRequestFailed)
            {
                return default(T);
            }
        }
    }
}
