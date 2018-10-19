using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AoLibs.HttpHelper.Interfaces;

namespace AoLibs.HttpHelper
{
    public class RequestExceptionHandler : IRequestExceptionHandler
    {
        public async Task<HttpResponseMessage> Execute(Task<HttpResponseMessage> request, string identifier, IApiResponseHandler responseHandler)
        {
            HttpResponseMessage response = null;

            try
            {
                response = await request;
            }
            catch (WebException e)
            {
                if (e.Message.Contains("NameResolutionFailure") || 
                    e.Message.Contains("Network is unreachable") ||
                    e.Message.Contains("Internet connection") ||
                    e.Message.Contains("ReceiveFailure"))
                {
                    throw new NoInternetConnectionException("[iOS case] " + e.ToString(), e);
                }
                else
                {
                    throw new ApiCommunicationException(e.ToString(), e);
                }
            }
            catch (Exception e)
            {
                throw new NoInternetConnectionException("[Android case] " + e.ToString(), e);
            }

            var content = await response?.Content?.ReadAsStringAsync();

            if ((int)response.StatusCode >= 200 && (int)response.StatusCode < 300)
            {
                if (responseHandler.ProcessResponse(content))
                {
                    return response;
                }
                else
                {
                    throw new ApiCommunicatorRequestFailed(content);
                }
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedException(response.ToString());
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NoInternetConnectionException(response.ToString());
            }
            else if (response.StatusCode == HttpStatusCode.Found) //redirecting occurred, wifi authentication for example
            {
                throw new NoInternetConnectionException(response.ToString());
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new ApiCommunicationException(response.ToString(), response);
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new ApiCommunicationException(response.ToString() + "\nContent: \n" + content, response);
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new ApiCommunicationException(response.ToString(), response);
            }
            else
            {
                throw new ApiCommunicationException(response.ToString(), response);
            }
        }

        public async Task<T> HandleExeptions<T>(Task<T> request, IApiExceptionHandler exceptionHandler)
        {
            try
            {
                return await request;
            }
            catch (NoInternetConnectionException e)
            {
                exceptionHandler.OnNoInternetConnection(e);
                return default(T);
            }
            catch (ApiCommunicationException e)
            {
                exceptionHandler.OnApiCommunication(e);
                return default(T);
            }
            catch (UnauthorizedException e)
            {
                exceptionHandler.OnUnauthorized(e);
                return default(T);
            }
        }

        public async Task<bool> HandleExeptions(Task request, IApiExceptionHandler exceptionHandler)
        {
            try
            {
                await request;
                return true;
            }
            catch (NoInternetConnectionException e)
            {
                exceptionHandler.OnNoInternetConnection(e);
                return false;
            }
            catch (ApiCommunicationException e)
            {
                exceptionHandler.OnApiCommunication(e);
                return false;
            }
            catch (UnauthorizedException e)
            {
                exceptionHandler.OnUnauthorized(e);
                return false;
            }
        }
    }
}
