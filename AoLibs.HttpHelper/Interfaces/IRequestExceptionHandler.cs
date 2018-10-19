using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AoLibs.HttpHelper.Interfaces
{
    public interface IRequestExceptionHandler
    {
        Task<HttpResponseMessage> Execute(Task<HttpResponseMessage> request, string identifier, IApiResponseHandler responseHandler);
        Task<T> HandleExeptions<T>(Task<T> request, IApiExceptionHandler exceptionHandler);
        Task<bool> HandleExeptions(Task request, IApiExceptionHandler exceptionHandler);
    }
}
