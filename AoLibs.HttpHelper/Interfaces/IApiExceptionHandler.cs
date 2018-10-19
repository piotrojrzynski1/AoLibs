
using System.Threading.Tasks;

namespace AoLibs.HttpHelper.Interfaces
{
    public interface IApiExceptionHandler
    {
        Task OnNoInternetConnection(NoInternetConnectionException e);
        Task OnApiCommunication(ApiCommunicationException e);
        Task OnUnauthorized(UnauthorizedException e);
    }
}
