using AoLibs.Adapters.Core.Interfaces;
using AoLibs.HttpHelper.ContentSerialization;
using AoLibs.Sample.Shared.BL;
using AoLibs.Sample.Shared.BL.HttpCommunication;
using AoLibs.Sample.Shared.BL.HttpCommunication.ApiControllers;
using AoLibs.Sample.Shared.BL.Providers;
using AoLibs.Sample.Shared.Interfaces;
using Autofac;

namespace AoLibs.Sample.Shared.Statics
{
    public static class ResourceLocator
    {
        private static IContainer _container;

        internal static void RegisterResources(this ContainerBuilder builder)
        {
            builder.RegisterBuildCallback(BuildCallback);

            builder.RegisterType<JsonContentSerializer>().As<JsonContentSerializer>().SingleInstance();
            builder.RegisterType<JsonContentDeserializer>().As<JsonContentDeserializer>().SingleInstance();
            builder.RegisterType<FancyApiResponseHandler>().As<FancyApiResponseHandler>().SingleInstance();
            builder.RegisterType<DefaultApiExceptionHandler>().As<DefaultApiExceptionHandler>().SingleInstance();

            builder.RegisterType<FancyPostsController>().As<IFancyPostsController>().SingleInstance();

            builder.RegisterType<FancyTrainsProvider>().As<ISomeFancyProvider>().SingleInstance();
            builder.RegisterType<FancyTurtlesProvider>().As<ISomeFancyProvider>().SingleInstance();
            builder.RegisterType<FancyPostsProvider>().As<IFancyPostsProvider>().SingleInstance();

            builder.RegisterType<AppVariables>().UsingConstructor(typeof(ISettingsProvider), typeof(IDataCache))
                .SingleInstance();
        }

        private static void BuildCallback(IContainer obj)
        {
            _container = obj;
        }

        public static ILifetimeScope ObtainScope()
        {
            return _container.BeginLifetimeScope();
        }
    }
}
