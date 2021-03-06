﻿using Microsoft.Extensions.Configuration;
using PostSharp.Aspects;
using PostSharp.Serialization;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using CrossCutting.Caching;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Data;

using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace CrossCutting.Logging
{
    [PSerializable]

    public class ServiceLoggingAttribute : OnMethodBoundaryAspect
    {

        //Logger used to log the messages. The NLog logger is not serializable, so this won't be serialized. (this does not impact functionality!) 
        [NonSerialized]
        public Logger Logger;
        public List<ParameterInfo> ParameterInfos;
        //Name of the method, initialized at compile time to improve performance!  
        private String _methodName;
        //Name of the class, initialized at compile time to improve performance!  
        private String _className;

        //Initialize some fields at compile time to improve performance. 
        public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
        {
            base.CompileTimeInitialize(method, aspectInfo);
            //Compile Time initialization of the class name.  
            if (method.ReflectedType != null) _className = method.ReflectedType.Name;
            //Compile Time initialization of the method name.  
            _methodName = _className + "." + method.Name;
            ParameterInfos = new List<ParameterInfo>(method.GetParameters());
        }


        public override void RuntimeInitialize(MethodBase method)
        {
            var config = new LoggingConfiguration();

            var dbTarget = new DatabaseTarget
            {
                ConnectionString = CrossCutting.Caching.Caching.Instance.GetApplicationConfigs("DBConnection"),
                CommandText = @"INSERT INTO [dbo].[LogsServiceConcrete]
           ([Application]   ,[Logged]     ,[Level]     ,[UserName]
           ,[Message]      ,[MachineName]      ,[Logger]     ,[Callsite]           ,[Exception])
    VALUES(@application, GetDate(), @level,@username,
@message, @machinename,   @logger, @callSite, @exception)"
            };

            // dbTarget.ConnectionString = @"Data Source=.;Initial Catalog=EmployeeManage;Integrated Security=True";
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@application", new NLog.Layouts.SimpleLayout(@"${appsetting:name=AppName:default=Unknown\: set AppName in appSettings}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@username", new NLog.Layouts.SimpleLayout("${identity}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@machinename", new NLog.Layouts.SimpleLayout("${machinename}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@callSite", new NLog.Layouts.SimpleLayout("${callsite}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@thread", new NLog.Layouts.SimpleLayout("${threadid}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@level", new NLog.Layouts.SimpleLayout("${level}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@logger", new NLog.Layouts.SimpleLayout("${logger}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@message", new NLog.Layouts.SimpleLayout("${message}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@exception", new NLog.Layouts.SimpleLayout("${exception}")));

            var asyncWrapper = new AsyncTargetWrapper(dbTarget, queueLimit: 10000,
                overflowAction: AsyncTargetWrapperOverflowAction.Grow)
            {
                OptimizeBufferReuse = true, TimeToSleepBetweenBatches = 50
            };

            var autoFlushWrapper = new AutoFlushTargetWrapper(wrappedTarget: asyncWrapper)
            {
                OptimizeBufferReuse = true, Condition = "level >= LogLevel.Warn", AsyncFlush = false
            };


            var rule = new LoggingRule(loggerNamePattern: "*", minLevel: LogLevel.Trace, target: autoFlushWrapper);
            config.AddTarget("database", autoFlushWrapper);
            config.LoggingRules.Add(rule);

            //Activate configuration
            LogManager.Configuration = config;


        }
        public override void OnEntry(MethodExecutionArgs args)
        {

            base.OnEntry(args);
            Logger = LogManager.GetCurrentClassLogger();
            string logMessage = "Entering " + _methodName + " with arguments: ";
            logMessage = AddArgumentsToLogMessage(args, logMessage);
            Logger.Debug(logMessage);
            
        }

        //This implementation can be improved by using a string builder.
        //Also you might be interested in checking if the argument is an IEnumerable and log this using a specific syntax.  
        private string AddArgumentsToLogMessage(MethodExecutionArgs args, string logMessage)
        {
            int argumentIndex = 0;
            foreach (var argument in args.Arguments)
            {
                ParameterInfo argumentInfo = ParameterInfos[argumentIndex];
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
            //args.FlowBehavior = FlowBehavior.RethrowException;
        }
    }
}
