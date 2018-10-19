using System;
using Newtonsoft.Json;
using AoLibs.HttpHelper.Interfaces;
using AoLibs.HttpHelper.Models;
using AoLibs.Sample.Shared.Models.HttpCommunication;

namespace AoLibs.Sample.Shared.BL.HttpCommunication
{
    public class FancyApiResponseHandler : IApiResponseHandler
    {
        public bool ProcessResponse(string content)
        {
            return true;
        }
    }
}
