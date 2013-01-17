using System;
using System.Text;

namespace ReasonFramework.Common
{
    /// <summary>
    /// Класс логов
    /// </summary>
    public class Logger
    {
        // Enable/Disable write log
        private static bool isLoggingEnabled = true;

        // Output thread
        private static LogOutput[] outputs = new LogOutput[] { new ConsoleOutput()};

        // Write log message to all output threads
        public static void Log(object message)
        {
            if (isLoggingEnabled)
            {
                foreach (LogOutput logOutput in outputs)
                {
                    logOutput.Log(message);
                }
            }
        }

        public static void Log(string message, params object[] mas)
        {
            if (isLoggingEnabled)
            {
                if (mas.Length > 0)
                    Log((object)System.String.Format(message, mas));
                else 
                    Log((object)message);
            }
        }
    }

    public abstract class LogOutput
    {
        public abstract void Log(object o);
    }

    /// <summary>
    /// Имплементация для консоли
    /// </summary>
    public class ConsoleOutput : LogOutput
    {
        public ConsoleOutput()
        {
        }

        public override void Log(object o)
        {
            System.Diagnostics.Debug.WriteLine("[DEBUG]{0}",o);
        }
    }
}