
namespace XMLConverter.Instrumentation
{
    using System;
    using log4net;
    using log4net.Config;

    public class Logger : ILogger
    {
        private static readonly ILog BaseLogger = LogManager.GetLogger(typeof(Logger));

        private static Logger _instance;
        public static Logger Instance
        {
            get { return _instance ?? (_instance = new Logger()); }
        }

        public Logger()
        {
            XmlConfigurator.Configure();
        }
        
        public void Error(Exception ex, string messageFormatted, params object[] arguments)
        {
            BaseLogger.Error(string.Format(messageFormatted, arguments), ex);
        }

        public void Error(string messageFormatted, params object[] arguments)
        {
            BaseLogger.ErrorFormat(messageFormatted, arguments);
        }

        public void Debug(string messageFormatted, params object[] arguments)
        {
            BaseLogger.DebugFormat(messageFormatted, arguments);
        }

        public void Info(string messageFormatted, params object[] arguments)
        {
            BaseLogger.InfoFormat(messageFormatted, arguments);
        }
    }
}
