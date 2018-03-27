using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace TopshelfTest.Services
{
    public interface IFileManagerService
    {
        bool SynchCurrent(ILogger logger);
        bool CheckArchive(ILogger logger);
    }
}
