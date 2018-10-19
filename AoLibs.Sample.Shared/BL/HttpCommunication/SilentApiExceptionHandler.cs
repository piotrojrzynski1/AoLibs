using System;
using System.Threading.Tasks;
using AoLibs.Adapters.Core.Interfaces;
using AoLibs.HttpHelper;
using AoLibs.HttpHelper.Interfaces;
using AoLibs.Navigation.Core;
using AoLibs.Navigation.Core.Interfaces;
using AoLibs.Sample.Shared.Models;

namespace AoLibs.Sample.Shared.BL.HttpCommunication
{
    public class SilentApiExceptionHandler : IApiExceptionHandler
    {
        public async Task OnNoInternetConnection(NoInternetConnectionException e)
        {
        }

        public async Task OnApiCommunication(ApiCommunicationException e)
        {
        }

        public async Task OnUnauthorized(UnauthorizedException e)
        {
        }
    }
}
