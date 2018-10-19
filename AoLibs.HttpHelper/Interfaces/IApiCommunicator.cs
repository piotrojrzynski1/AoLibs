using System;
using System.Threading.Tasks;
using AoLibs.HttpHelper.Models;

namespace AoLibs.HttpHelper.Interfaces
{
    public interface IApiCommunicator
    {
        Task<TypeResponse> Get<TypeResponse>(
            IApiDefinition api,
            string address,
            IApiExceptionHandler exceptionHandlerOverride
        );

        Task<TypeResponse> Post<TypeResponse>(
            IApiDefinition api,
            string address,
            IApiExceptionHandler exceptionHandlerOverride,
            IHttpContentSerializable content
        );
    }
}
