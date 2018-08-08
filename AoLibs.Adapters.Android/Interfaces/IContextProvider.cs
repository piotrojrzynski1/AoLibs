﻿using Android.App;

namespace AoLibs.Adapters.Android.Interfaces
{
    /// <summary>
    /// Interface used by library components to resolve current application context.
    /// </summary>
    public interface IContextProvider
    {
        /// <summary>
        /// Current activity.
        /// </summary>
        Activity CurrentContext { get; }
    }
}