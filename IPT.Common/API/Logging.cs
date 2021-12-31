using System;
using System.Collections.Generic;
using System.Reflection;
using IPT.Common.User.Settings;
using Rage;

namespace IPT.Common.API
{
    /// <summary>
    /// Used to determine whether a logging call should actually be used.
    /// </summary>
    public enum LoggingLevel
    {
        /// <summary>The most verbose level, logs everything.</summary>
        DEBUG,

        /// <summary>A standard logging level.</summary>
        INFO,

        /// <summary>Not quite errors but also not normal.</summary>
        WARNING,

        /// <summary>Used for exception handling.</summary>
        ERROR,
    }

    /// <summary>
    /// Interface to logging to the Rage.Game logger.
    /// </summary>
    public static class Logging
    {
        private static readonly Dictionary<Assembly, LoggingLevel> Assemblies = new Dictionary<Assembly, LoggingLevel>();

        /// <summary>
        /// Register an assembly (plugin) to set its specific logging level.
        /// <example>Example:
        /// <code>
        /// Logging.Register(Assembly.GetExecutingAssembly(), LoggingLevel.INFO);
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="assembly">The calling assembly.</param>
        /// <param name="level">The minimum logging level.  Logging calls below this level will not be used.</param>
        public static void Register(Assembly assembly, LoggingLevel level)
        {
            Assemblies[assembly] = level;
        }

        /// <summary>
        /// Generates a SettingsInt object to include in your Settings class.
        /// </summary>
        /// <param name="defaultLevel">The default logging level.</param>
        /// <returns>A SettingsInt object.</returns>
        public static SettingInt GetLogLevelSetting(LoggingLevel defaultLevel = LoggingLevel.DEBUG)
        {
            return new SettingInt(
                "Advanced",
                "LogLevel",
                "The logging level",
                (int)defaultLevel,
                (int)LoggingLevel.DEBUG,
                (int)LoggingLevel.ERROR,
                1);
        }

        /// <summary>
        /// Send a DEBUG level log messsage.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        public static void Debug(string message)
        {
            Log(LoggingLevel.DEBUG, Assembly.GetCallingAssembly(), message);
        }

        /// <summary>
        /// Send an ERROR level log message. Use for Exception handling.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        /// <param name="ex">The Exception raised.</param>
        public static void Error(string message, Exception ex)
        {
            var assembly = Assembly.GetCallingAssembly();
            Log(LoggingLevel.ERROR, assembly, message);
            Log(LoggingLevel.ERROR, assembly, ex.Message);
            Log(LoggingLevel.DEBUG, assembly, ex.StackTrace);
        }

        /// <summary>
        /// Send an INFO level log message.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        public static void Info(string message)
        {
            Log(LoggingLevel.INFO, Assembly.GetCallingAssembly(), message);
        }

        /// <summary>
        /// Send a WARNING level log message.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        public static void Warning(string message)
        {
            Log(LoggingLevel.WARNING, Assembly.GetCallingAssembly(), message);
        }

        private static LoggingLevel GetLoggingLevel(Assembly assembly)
        {
            if (Assemblies.ContainsKey(assembly))
            {
                return Assemblies[assembly];
            }

            return LoggingLevel.DEBUG;
        }

        private static void Log(LoggingLevel level, Assembly assembly, string message)
        {
            string plugin = assembly.GetName().Name;
            if (level >= GetLoggingLevel(assembly))
            {
                Game.LogTrivial($"{plugin}: [{Enum.GetName(typeof(LoggingLevel), level)}] {message}");
            }
        }
    }
}
