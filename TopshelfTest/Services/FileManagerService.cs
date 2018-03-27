using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace TopshelfTest.Services
{
    public class FileManagerService : IFileManagerService
    {
        public bool SynchCurrent(ILogger logger)
        {
            string _path = @"D:\TopshelfDir\Current";
            string _archivePath = @"D:\TopshelfDir\Archive";
            bool done = false;

            try
            {
                if (!Directory.Exists(_archivePath))
                {
                    logger.Info("current directory created");
                    Directory.CreateDirectory(_path);
                }

                if (!Directory.Exists(_path))
                {
                    logger.Info("Archive directory created");
                    Directory.CreateDirectory(_archivePath);
                }
            }
            catch(Exception err)
            {
                logger.Error(err.Message);
                return false;
            }
           

            try
            {
                string[] files = Directory.GetFiles(_path);

                if(files.Length == 0)
                {
                    logger.Trace("No files in current directory");
                    return false;
                }
                else
                {
                    foreach (var fpath in files)
                    {
                        FileInfo file = new FileInfo(fpath);
                        if (file.Exists)
                        {
                            if (file.CreationTime < DateTime.Now.AddMinutes(-5))
                            {
                                file.MoveTo(_archivePath + @"\" + file.Name);
                                logger.Trace("File (file Name: {0}. Creation time: {1}.) moved to Archive directory", file.Name, file.CreationTime);
                            }
                        }
                    }

                    if (!done)
                    {
                        logger.Info("Nothing to move");
                    }
                    
                    return true;
                }
            }
            catch(Exception err)
            {
                logger.Error(err.Message);
                return false;
            }

        }

        public bool CheckArchive(ILogger logger)
        {
            string _archivePath = @"D:\TopshelfDir\Archive";
            bool done = false;

            try
            {
                if (!Directory.Exists(_archivePath))
                {
                    logger.Info("Archive directory created");
                    Directory.CreateDirectory(_archivePath);
                }
            }
            catch (Exception err)
            {
                logger.Error(err.Message);
                return false;
            }

            try
            {
                string[] files = Directory.GetFiles(_archivePath);

                if (files.Length == 0)
                {
                    logger.Trace("No files in archive directory");
                    return false;
                }
                else
                {
                    foreach (var fpath in files)
                    {
                        FileInfo file = new FileInfo(fpath);
                        if (file.Exists)
                        {
                            if (file.CreationTime < DateTime.Now.AddHours(-1))
                            {
                                done = true;
                                file.Delete();
                                logger.Trace("File (file Name: {0}. Creation time: {1}.) delete from Archive directory", file.Name, file.CreationTime);
                            }
                        }
                    }

                    if (!done)
                    {
                        logger.Info("Nothing to delete");
                    }

                    return true;
                }
            }
            catch (Exception err)
            {
                logger.Error(err.Message);
                return false;
            }
        }
        
    }
}
