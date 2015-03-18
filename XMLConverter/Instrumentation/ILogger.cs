
namespace XMLConverter.Instrumentation
{
    using System;

    public interface ILogger
    {
        void Error(Exception ex, string messageFormatted, params object[] arguments);

        void Error(string messageFormatted, params object[] arguments);

        void Debug(string messageFormatted, params object[] arguments);

        void Info(string messageFormatted, params object[] arguments);
    }
}
