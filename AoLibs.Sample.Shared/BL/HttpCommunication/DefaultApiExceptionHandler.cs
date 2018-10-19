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
    public class DefaultApiExceptionHandler : IApiExceptionHandler
    {
        private readonly IMessageBoxProvider _messageBoxAdapter;

        public DefaultApiExceptionHandler(
            IMessageBoxProvider messageBoxAdapter)
        {
            _messageBoxAdapter = messageBoxAdapter;
        }

        public async Task OnNoInternetConnection(NoInternetConnectionException e)
        {
            await _messageBoxAdapter.ShowMessageBoxOkAsync(
                "No internetz",
                "Not fancy failure occured",
                "Ok");
        }

        public async Task OnApiCommunication(ApiCommunicationException e)
        {
            await _messageBoxAdapter.ShowMessageBoxOkAsync(
                "Internal server error",
                "Not fancy failure occured",
                "Ok");
        }

        public async Task OnUnauthorized(UnauthorizedException e)
        {
            await _messageBoxAdapter.ShowMessageBoxOkAsync(
                "No authorization",
                "Not fancy failure occured",
                "Ok");
        }
    }
}
