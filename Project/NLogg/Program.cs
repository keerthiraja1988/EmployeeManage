using NLog;
using NLog.Conditions;
using NLog.Config;
using NLog.Targets;
using PostSharp.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NLogg
{
    class Program
    {
        static void Main(string[] args)
        {
            

            HelloWorld("sdcdcd");

            Console.ReadKey();

        }

        [NLog]
        public static void HelloWorld(String Name)
        {
            Console.WriteLine("Hello " + Name);
        }

    }

    [Serializable]  // .NET requires every attribute to be serializable
    public class NLogAttribute : OnMethodBoundaryAspect
    {
        //Logger used to log the messages. The NLog logger is not serializable, so this won't be serialized. (this does not impact functionality!) 
        [NonSerialized]
        public Logger Logger;
        public List<ParameterInfo> parameterInfos;
        //Name of the method, initialized at compile time to improve performance!  
        private String _methodName;
        //Name of the class, initialized at compile time to improve performance!  
        private String _className;

       

        //Intialize some fields at compile time to improve performance. 
        public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
        {

          

            base.CompileTimeInitialize(method, aspectInfo);
            //Compile Time initizalization of the class name.  
            _className = method.ReflectedType.Name;
            //Compile Time initizalization of the method name.  
            _methodName = _className + "." + method.Name;
            parameterInfos = new List<ParameterInfo>(method.GetParameters());

          

        }


        public override void RuntimeInitialize(MethodBase method)
        {
            //var config = new NLog.Config.LoggingConfiguration();

            //var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "file.txt" };
            //var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            //config.AddRule(LogLevel.Trace, LogLevel.Fatal, logconsole);
            //config.AddRule(LogLevel.Trace, LogLevel.Fatal, logfile);

            //NLog.LogManager.Configuration = config;


            //Logger = NLog.LogManager.GetCurrentClassLogger();


            var config = new LoggingConfiguration();

            var target =
                new FileTarget
                {
                    FileName = typeof(Program).FullName + ".log"
                };

            config.AddTarget("logfile", target);

            var dbTarget = new DatabaseTarget();

            dbTarget.ConnectionString = @"Data Source=.;Initial Catalog=EmployeeManage;Integrated Security=True";

            dbTarget.CommandText =
@"INSERT INTO [AspNetCoreLog] (Logged,  Level, Logger, Message, Exception) 
    VALUES(GETDATE(), @level, @logger, @message, @exception)";

            dbTarget.Parameters.Add(new DatabaseParameterInfo("@thread", new NLog.Layouts.SimpleLayout("${threadid}")));

            dbTarget.Parameters.Add(new DatabaseParameterInfo("@level", new NLog.Layouts.SimpleLayout("${level}")));

            dbTarget.Parameters.Add(new DatabaseParameterInfo("@logger", new NLog.Layouts.SimpleLayout("${logger}")));

            dbTarget.Parameters.Add(new DatabaseParameterInfo("@message", new NLog.Layouts.SimpleLayout("${message}")));

            dbTarget.Parameters.Add(new DatabaseParameterInfo("@exception", new NLog.Layouts.SimpleLayout("${exception}")));

            config.AddTarget("database", dbTarget);

            var rule = new LoggingRule("*", LogLevel.Trace, target);

            config.LoggingRules.Add(rule);

            var dbRule = new LoggingRule("*", LogLevel.Trace, dbTarget);

            config.LoggingRules.Add(dbRule);

            LogManager.Configuration = config;

            Logger = LogManager.GetCurrentClassLogger();
            Logger.Info("Hello World");
        }
        public override void OnEntry(MethodExecutionArgs args)
        {
           
            base.OnEntry(args);
            Logger = LogManager.GetCurrentClassLogger();
            string logMessage = "Entering " + _methodName + " with arguments: ";
            logMessage = AddArgumentsToLogMessage(args, logMessage);
            Logger.Info(logMessage);
            Logger.Info("Hello World");
        }

        //This implementation can be improved by using a string builder.
        //Also you might be interested in checking if the argument is an IEnumerable and log this using a specific syntax.  
        private string AddArgumentsToLogMessage(MethodExecutionArgs args, string logMessage)
        {
            int argumentIndex = 0;
            foreach (var argument in args.Arguments)
            {
                ParameterInfo argumentInfo = parameterInfos[argumentIndex];
                logMessage += argumentInfo.Name + " = { ";
                if (argument != null)
                    logMessage += argument.ToString();
                else
                    logMessage += "[null]";
                logMessage += " }" + " ";
                argumentIndex++;
            }
            return logMessage;
        }
        public override void OnExit(MethodExecutionArgs args)
        {
            base.OnExit(args);
            string logMessage = "Leaving " + _methodName + " with arguments: ";
            logMessage = AddArgumentsToLogMessage(args, logMessage);
            logMessage += "With result: ";
            if (args.ReturnValue == null)
                logMessage += "[null]";
            else
                logMessage += args.ReturnValue.ToString();
            Logger.Debug(logMessage);
        }
        public override void OnException(MethodExecutionArgs args)
        {
            base.OnException(args);
            string logMessage = "Exception occurred in " + _methodName + " with arguments: ";
            logMessage = AddArgumentsToLogMessage(args, logMessage);
            logMessage += "With exception: " + args.Exception.Message;
            Logger.Error(logMessage);
            args.FlowBehavior = FlowBehavior.RethrowException;
        }
    }
}
