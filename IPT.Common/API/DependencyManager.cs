using Rage;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System;

namespace IPT.Common.API
{
    /// <summary>
    /// A static class for checking plugin dependencies.
    /// </summary>
    public static class DependencyManager
    {
        /// <summary>
        /// Checks a list of dependencies and displays a notification if any are missing or outdated.
        /// </summary>
        /// <param name="callingPluginName">The name of the plugin performing the check (e.g., "ProwlerRadar").</param>
        /// <param name="dependencies">A list of dependencies to check.</param>
        /// <returns>True if all dependencies are met, otherwise false.</returns>
        public static bool Check(string callingPluginName, List<Dependency> dependencies)
        {
            foreach (var dependency in dependencies)
            {
                if (!IsAssemblyAvailable(callingPluginName, dependency))
                {
                    string message = $"~y~{callingPluginName} requires:~n~{dependency.Name}~n~Version {dependency.Version} or higher";
                    Game.DisplayNotification("3dtextures", "mpgroundlogo_cops", callingPluginName, "~r~Dependency Error", message);
                    return false;
                }
            }
            return true;
        }

        private static bool IsAssemblyAvailable(string callingPluginName, Dependency dependency)
        {
            try
            {
                // We assume the dependency DLL is in the root GTA V folder
                var assembly = AssemblyName.GetAssemblyName($"{AppDomain.CurrentDomain.BaseDirectory}/{dependency.Name}.dll");
                if (assembly.Version >= new Version(dependency.Version))
                {
                    Logging.Info($"{callingPluginName} dependency '{dependency.Name}' is available (v{assembly.Version}).");
                    return true;
                }

                Logging.Warning($"{callingPluginName} dependency '{dependency.Name}' does not meet minimum requirements (v{assembly.Version} < v{dependency.Version}).");
                return false;
            }
            catch (Exception ex) when (ex is FileNotFoundException || ex is BadImageFormatException)
            {
                Logging.Warning($"{callingPluginName} dependency '{dependency.Name}' is not available.");
                return false;
            }
        }
    }
}
