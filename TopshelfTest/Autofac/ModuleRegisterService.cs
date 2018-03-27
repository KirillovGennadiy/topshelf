using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using NLog;
using TopshelfTest.Services;
using System.Threading.Tasks;

namespace TopshelfTest.Autofac
{
    public class ModuleRegisterService : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FileManagerService>().As<IFileManagerService>();
            builder.Register(c => LogManager.GetCurrentClassLogger()).As<ILogger>();
        }
    }
}
