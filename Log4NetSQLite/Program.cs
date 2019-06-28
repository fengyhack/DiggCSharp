using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using log4net;
using log4net.Config;

namespace SQLiteDemo
{
    static class Program
    {
        static void Main(string[] args)
        {
            var fi = new FileInfo("logger.xml");
            XmlConfigurator.ConfigureAndWatch(fi);
            var logger = LogManager.GetLogger("Ado");

            try
            {
                for (int i = 0; i < 1000; ++i)
                {
                    logger.Debug($"{i}");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
