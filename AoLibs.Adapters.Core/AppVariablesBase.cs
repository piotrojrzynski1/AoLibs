﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AoLibs.Adapters.Core.Excpetions;
using AoLibs.Adapters.Core.Interfaces;
using Newtonsoft.Json;

namespace AoLibs.Adapters.Core
{
    /// <summary>
    /// Base class for user defined AppVariables classes.
    /// </summary>
    public abstract class AppVariablesBase
    {
        private readonly ISyncStorage _syncStorage;
        private readonly IAsyncStorage _asyncStorage;

        /// <summary>
        /// Gets list of all found attributes on given properties.
        /// </summary>
        public IReadOnlyCollection<VariableAttribute> Attributes { get; internal set; }

        /// <summary>
        /// Attribute that marks property of type <see cref="Holder{T}"/> as variable for persistent data storage.
        /// Marked property is required to have both setter and getter. 
        /// Properties of type different than <see cref="Holder{T}"/> will be ignored.
        /// </summary>
        [AttributeUsage(AttributeTargets.Property)]
        public class VariableAttribute : Attribute
        {
            /// <summary>
            /// Gets or sets a value indicating whether to store in persistent cache.
            /// </summary>
            public bool MemoryOnly { get; set; }

            /// <summary>
            /// Gets or sets key that will be used to save the file, by default property name will be used.
            /// </summary>
            public string CustomKey { get; set; }

            /// <summary>
            /// Gets or sets time* in seconds describing how long data is valid since last write.
            /// By default only supported in async calls when calling <see cref="AppVariablesBase"/> 
            /// constructor with <see cref="IDataCache"/>. Provide custom <see cref="ISyncStorage"/> to consume this attribute.
            /// </summary>
            public int ExpirationTime { get; set; } = -1;
        }

        /// <summary>
        /// Interface used by <see cref="AppVariablesBase"/> to store items in asynchronous manner, on filesystem for example.
        /// </summary>
        public interface IAsyncStorage
        {
            Task SetAsync<T>(T data, string key, VariableAttribute attr);
            Task<T> GetAsync<T>(string key, VariableAttribute attr);
            Task Reset(string key, VariableAttribute attr);
        }

        /// <summary>
        /// Interface used by <see cref="AppVariablesBase"/> to store items in synchronous manner, in application settings for example.
        /// </summary>
        public interface ISyncStorage
        {
            void SetValue<T>(T data, string key, VariableAttribute attr);
            void GetValue<T>(ref T data, string key, VariableAttribute attr);
        }

        private class DefaultSyncStorage : ISyncStorage
        {
            private readonly ISettingsProvider _settingsProvider;

            public DefaultSyncStorage(ISettingsProvider settingsProvider)
            {
                _settingsProvider = settingsProvider;
            }

            public void GetValue<T>(ref T local, string prop, VariableAttribute attr)
            {
                var cached = _settingsProvider.GetString(prop);
                if (cached == null)
                    return;

                local = JsonConvert.DeserializeObject<T>(cached);
            }

            public void SetValue<T>(T value, string prop, VariableAttribute attr)
            {
                _settingsProvider.SetString(
                    prop,
                    value == null ? null : JsonConvert.SerializeObject(value));
            }
        }

        private class DefaultAsyncStorage : IAsyncStorage
        {
            private readonly IDataCache _dataCache;

            private class TimestampWrapper<T>
            {
                public TimestampWrapper(T data)
                {
                    Value = data;
                }

                public DateTime WriteTime { get; set; }
                public T Value { get; set; }
            }

            public DefaultAsyncStorage(IDataCache dataCache)
            {
                _dataCache = dataCache;
            }

            public Task SetAsync<T>(T data, string key, VariableAttribute attr)
            {
                return _dataCache.SaveDataAsync(key, data);
            }

            public async Task<T> GetAsync<T>(string key, VariableAttribute attr)
            {
                try
                {
                    return await _dataCache.RetrieveData<T>(
                        key,
                        attr.ExpirationTime > 0 ? (TimeSpan?) TimeSpan.FromSeconds(attr.ExpirationTime) : null);
                }
                catch (DataExpiredException)
                {
                    return default;
                }
            }

            public Task Reset(string key, VariableAttribute attr)
            {
                return _dataCache.Clear(key);
            }
        }

        /// <summary>
        /// Base utility class for <see cref="Holder{T}"/>
        /// </summary>
        public class HolderBase
        {
#pragma warning disable SA1401 // Fields must be private
            internal static AppVariablesBase _parent;
#pragma warning restore SA1401 // Fields must be private
        }

        /// <summary>
        /// Class that holds stored data. Cannot be inherited. 
        /// If not instantiated it will be automatically created by underlying mechanisms.
        /// </summary>
        /// <typeparam name="T">The wrapped type.</typeparam>
        [Preserve(AllMembers = true)]
        public sealed class Holder<T> : HolderBase 
            where T : class
        {
            /// <summary>
            /// Fires whenever the value is changed.
            /// </summary>
            public event EventHandler<T> ValueChanged;

            private readonly VariableAttribute _attribute;
            private readonly string _propName;
            private T _value;
            private T _defaultValue;

            public Holder(T defaultValue)
            {
                _value = defaultValue;
            }

            public Holder(string propName, VariableAttribute attribute)
            {
                _attribute = attribute;
                _propName = attribute.CustomKey ?? propName;
            }

            /// <summary>
            /// Clears both memory storage and <see cref="ISyncStorage"/>
            /// </summary>
            public void Reset()
            {
                _value = default(T);
                if (!_attribute.MemoryOnly)
                    _parent._syncStorage.SetValue(default(T), _propName, _attribute);
            }

            /// <summary>
            /// Clears both memory storage and <see cref="IAsyncStorage"/>
            /// </summary>
            public async Task ResetAsync()
            {
                _value = default(T);
                if (!_attribute.MemoryOnly)
                    await _parent._asyncStorage.Reset(_propName, _attribute);
            }

            /// <summary>
            /// Gets or sets value using <see cref="ISyncStorage"/>
            /// </summary>
            public T Value
            {
                get
                {
                    if (_value != null)
                        return _value;
                    if (!_attribute.MemoryOnly)
                        _parent._syncStorage.GetValue(ref _value, _propName, _attribute);
                    return _value;
                }
                set
                {
                    _value = value;
                    if (!_attribute.MemoryOnly)
                        _parent._syncStorage.SetValue(value, _propName, _attribute);
                    ValueChanged?.Invoke(this, _value);
                }
            }

            /// <summary>
            /// Reads value using <see cref="IAsyncStorage"/>
            /// </summary>
            public Task<T> GetAsync()
            {
                if (_parent._asyncStorage == null)
                {
                    throw new InvalidOperationException(
                        "You can call async methods only after providing async storage interface in AppVariablesBase.");
                }

                if (_attribute.MemoryOnly)
                {
                    return Task.FromResult(_value);
                }

                return _parent._asyncStorage.GetAsync<T>(_propName, _attribute);
            }

            /// <summary>
            /// Writes value using <see cref="IAsyncStorage"/>
            /// </summary>
            /// <param name="data">Data to save.</param>
            public async Task SetAsync(T data)
            {
                if (_parent._asyncStorage == null)
                {
                    throw new InvalidOperationException(
                        "You can call async methods only after providing async storage interface in AppVariablesBase.");
                }

                if (_attribute.MemoryOnly)
                {
                    _value = data;
                    return;
                }

                await _parent._asyncStorage.SetAsync(data, _propName, _attribute);
                ValueChanged?.Invoke(this, _value);
            }

            /// <summary>
            /// Saves the value to the <see cref="ISyncStorage"/>.
            /// Useful when the content of the stored object changes but the actual setter of <see cref="Holder{T}"/> is not called.
            /// </summary>
            public void Save()
            {
                if (!_attribute.MemoryOnly)
                    _parent._syncStorage.SetValue(_value, _propName, _attribute);
            }

            /// <summary>
            /// Updates currently stored value in memory to the newest found in <see cref="ISyncStorage"/>
            /// </summary>
            public void Update()
            {
                if (!_attribute.MemoryOnly)
                    _parent._syncStorage.GetValue(ref _value, _propName, _attribute);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppVariablesBase"/> class.
        /// Initializes all properties marked with <see cref="VariableAttribute"/>
        /// </summary>
        private AppVariablesBase()
        {
            HolderBase._parent = this;

            var props = GetType().GetRuntimeProperties()
                .Where(info => info.PropertyType.GetGenericTypeDefinition() == typeof(Holder<>));
            var attributes = new List<VariableAttribute>();
            foreach (var prop in props)
            {
                var attr = prop.GetCustomAttribute<VariableAttribute>();

                if (attr != null)
                {
                    attributes.Add(attr);
                    var holder = prop.GetValue(this);
                    // default value not provided, we have to create instance
                    if (holder == null) 
                    {
                        prop.SetValue(this, Activator.CreateInstance(
                            prop.PropertyType,
                            new object[] {prop.Name, attr}));
                    } // we have value but we have to fill missing data
                    else 
                    {
                        var typeInfo = holder.GetType().GetTypeInfo();
                        typeInfo.GetDeclaredField("_propName")
                            .SetValue(holder, attr.CustomKey ?? prop.Name);

                        typeInfo.GetDeclaredField("_attribute")
                            .SetValue(holder, attr);
                    }
                }

                Attributes = attributes;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppVariablesBase"/> class.
        /// Initialize with default <see cref="ISyncStorage"/> where <see cref="ISettingsProvider"/> is underlying storage layer.
        /// Async methods of <see cref="Holder{T}"/> will be unavailable and throw <see cref="InvalidOperationException"/>
        /// </summary>
        /// <param name="settingsProvider">Settings provider.</param>
        /// <param name="dataCache">Data cache.</param>
        protected AppVariablesBase(ISettingsProvider settingsProvider, IDataCache dataCache = null) 
            : this()
        {
            if (dataCache != null)
                _asyncStorage = new DefaultAsyncStorage(dataCache);
            _syncStorage = new DefaultSyncStorage(settingsProvider);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppVariablesBase"/> class.
        /// Initialize with custom implementations of <see cref="ISyncStorage"/> and optionally <see cref="IAsyncStorage"/>.
        /// Not providing <see cref="IAsyncStorage"/> will result in <see cref="InvalidOperationException"/> when accessing async methods of <see cref="Holder{T}"/>
        /// </summary>
        /// <param name="syncStorage">Synchronous storage used when calling <see cref="Holder{T}.Value"/></param>
        /// <param name="asyncStorage">Asynchronous storage.</param>
        protected AppVariablesBase(ISyncStorage syncStorage, IAsyncStorage asyncStorage = null)
        {
            _syncStorage = syncStorage;
            _asyncStorage = asyncStorage;
        }
    }
}