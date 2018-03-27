using Topshelf;
using TopshelfTest.Autofac;
using Autofac;
using TopshelfTest.Services;
using NLog;

namespace TopshelfTest
{
    class Program
    {
        private static ContainerBuilder _containerBuilder;

        static void Main(string[] args)
        {
            _containerBuilder = new ContainerBuilder();
            _containerBuilder.RegisterModule(new ModuleRegisterService());
            var _container = _containerBuilder.Build();

            HostFactory.Run(x =>
            {
                x.Service(() => new Controller(_container));
                x.SetServiceName("MyTestService");
                x.StartAutomatically();

                x.EnableServiceRecovery(src => src.RestartService(0).OnCrashOnly());
            });
        }
    }
}
