using System;

namespace AoLibs.HttpHelper.Interfaces
{
    public interface IApiResponseHandler
    {
        bool ProcessResponse(string content);
    }
}
