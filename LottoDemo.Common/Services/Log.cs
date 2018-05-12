﻿using log4net;
using System;

namespace LottoDemo.Common.Services
{
    public static class Log
    {
        public static void Info(Type loggerType, string message)
        {
            GetLogger(loggerType).Info(message);
        }

        public static void Info(Type loggerType, string message, Exception error)
        {
            GetLogger(loggerType).Info(message, error);
        }

        public static void Error(Type loggerType, string message)
        {
            GetLogger(loggerType).Error(message);
        }

        public static void Error(Type loggerType, string message, Exception error)
        {
            GetLogger(loggerType).Error(message, error);
        }

        public static void Error(Type loggerType, Exception error)
        {
            GetLogger(loggerType).Error(error.Message, error);
        }

        public static void Debug(Type loggerType, string message)
        {
            GetLogger(loggerType).Debug(message);
        }

        public static void Debug(Type loggerType, string message, Exception error)
        {
            GetLogger(loggerType).Debug(message, error);
        }

        internal static ILog GetLogger(Type loggerType)
        {
            return LogManager.GetLogger(loggerType);
        }

        internal static ILog GetLogger(string loggerName)
        {
            return LogManager.GetLogger(loggerName);
        }
    }
}
