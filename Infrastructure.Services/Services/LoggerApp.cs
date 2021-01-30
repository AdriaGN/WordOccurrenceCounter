using NLog;
using NLog.Config;
using Infrastructure.Services.Interfaces;

namespace Infrastructure.Services
{
    public class LoggerApp : ILoggerApp
    {
        private static Logger loggerApp = LogManager.GetCurrentClassLogger();

        public void Trace(string message)
        {
            loggerApp.Trace(message);
        }

        public void Debug(string message)
        {
            loggerApp.Debug(message);
        }

        public void Info(string message)
        {
            loggerApp.Info(message);
        }

        public void Fatal(string message)
        {
            loggerApp.Fatal(message);
        }
    }
}
