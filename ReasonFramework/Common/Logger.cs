using System;
using System.Text;

using System.IO;

namespace ReasonFramework.Common
{
    // Class for write log
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

		public static void Log(System.Char[] message)
		{
			if (isLoggingEnabled)
			{
				foreach (LogOutput logOutput in outputs)
				{
					logOutput.Log(message);
				}
			}
		}

        public static void Log(string message, object obj1)
        {
            if (isLoggingEnabled)
            {
                Log(System.String.Format(message, obj1));
            }
        }

        public static void Log(string message, object obj1, object obj2)
        {
            if (isLoggingEnabled)
            {
                Log(System.String.Format(message, obj1, obj2));
            }
        }

        public static void Log(string message, object obj1, object obj2, object obj3)
        {
            if (isLoggingEnabled)
            {
                Log(System.String.Format(message, obj1, obj2, obj3));
            }
        }

        public static void Log(string message, object obj1, object obj2, object obj3, object obj4)
        {
            if (isLoggingEnabled)
            {
                Log(System.String.Format(message, obj1, obj2, obj3,obj4));
            }
        }
    }

    public abstract class LogOutput
    {
        public abstract void Log(object o);
		public abstract void Log(System.Char[] o);
    }
    // Console output thread
    public class ConsoleOutput : LogOutput
    {
        public ConsoleOutput()
        {
        }

        public override void Log(object o)
        {
            System.Diagnostics.Debug.WriteLine(o);
        }

		public override void Log(System.Char[] o)
		{
            System.Diagnostics.Debug.WriteLine(o);
		}
    }

    // File output thread
    //NOTE: Don't wotk, because need access to write file for android
    public class FileOutput : LogOutput
    {
        private StreamWriter fileWriter = new StreamWriter("out.log");
        
        public override void Log(object o)
        {
            fileWriter.WriteLine(o != null ? o.ToString() : "null");
        }

		public override void Log(System.Char[] o)
		{
			fileWriter.WriteLine(o != null ? o.ToString() : "null");
		}

        ~FileOutput(){
            fileWriter.Flush();
            fileWriter.Close();
            fileWriter.Dispose();
        }
    }
}