using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Repository.Hierarchy;
using System;
using PTLib.Log;

namespace PileSW
{
    public class Logger
    {

        public static void Setup(bool LogOnFile)
        {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
            CMTraceLog cMTraceLogLayout = new CMTraceLog();
            cMTraceLogLayout.ActivateOptions();
           /* PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = @"<![LOG[%logger %-5p %c %m [%-5level] [%logger] ]LOG]!><time=""%date{HH:mm:ss.fff}+000"" date=""%date{MM-dd-yyyy}"" component="""" context="""" type=""4"" thread=""%thread"">%n";
            //%date{HH:mm:ss} or %date{dd MMM yyyy HH:mm:ss,fff}. 
                //%date [%thread] %-5level %logger - %message%newline";
         
            patternLayout.ActivateOptions();
            */
            if (LogOnFile)
            { 
                RollingFileAppender roller = new RollingFileAppender();
                roller.AppendToFile = true;
                roller.File = @"Logs\Log_"+DateTime.Now.Date.ToString("yyyy_MM_dd")+".log";
               // roller.Layout = patternLayout; usiamo il nostro layout
                roller.Layout = cMTraceLogLayout;
                roller.MaxSizeRollBackups = 5;
                roller.MaximumFileSize = "1GB";
                roller.RollingStyle = RollingFileAppender.RollingMode.Size;
                roller.StaticLogFileName = true;
                roller.ActivateOptions();
                hierarchy.Root.AddAppender(roller);
            }

            MemoryAppender memory = new MemoryAppender();
            memory.ActivateOptions();
            hierarchy.Root.AddAppender(memory);

            hierarchy.Root.Level = Level.Debug;
            hierarchy.Configured = true;
        }
    }
  
}
