using System;

namespace AoLibs.HttpHelper.Interfaces
{
    public interface IApiConnectionExceptionNotifier
    {
        event EventHandler<Exception> ExceptionOccured;
        void NotifyExceptionOccured(Exception e);
    }
}