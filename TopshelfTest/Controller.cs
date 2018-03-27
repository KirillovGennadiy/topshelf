using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Topshelf;
using NLog;
using Autofac;
using TopshelfTest.Services;

namespace TopshelfTest
{
    public class Controller : ServiceControl
    {
        private Timer _timerSync;
        private ILogger _logger;
        private IContainer _container;
         
        public Controller(IContainer container)
        {
            _container = container;
            _logger = LogManager.GetCurrentClassLogger();
        }
        
        public bool Start(HostControl hostControl)
        {

            if (_timerSync != null)
            {
                _logger.Fatal("Service is already running");
                throw new InvalidOperationException("Service is already running");
            }

            _logger.Info("Сервис запущен");

            _timerSync = new Timer(state =>
            {
                SynchCurrent();
                CheckArchive();
            }, 
            null,
            TimeSpan.Zero, 
            TimeSpan.FromMinutes(5));
            
            return true;
        }


        public bool Stop(HostControl hostControl)
        {
            if (_timerSync == null)
            {
                _logger.Fatal("Service is not run");
                throw new InvalidOperationException("Service is not run");
            }
            _timerSync.Dispose();
            _timerSync = null;

            _logger.Info("Сервис остановлен");

            return true;
        }


        private bool SynchCurrent()
        {
            using (var lifetimescope = _container.BeginLifetimeScope())
            {
                _logger.Info("Запуск сканирования текущей дериктории");
                var filemanager = lifetimescope.Resolve<IFileManagerService>();
                return filemanager.SynchCurrent(_logger);
            }
        }

        private bool CheckArchive()
        {
            using (var lifetimescope = _container.BeginLifetimeScope())
            {
                _logger.Info("Запуск сканирования архивной дериктории");
                var filemanager = lifetimescope.Resolve<IFileManagerService>();
                return filemanager.CheckArchive(_logger);
            }
        }

    }
}
