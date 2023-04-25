using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IPT.Common.API;

namespace IPT.Common
{
    /// <summary>
    /// Methods to help with callouts.
    /// </summary>
    public static class Callouts
    {
        /// <summary>
        /// Create a Callout object from an LHandle.  Returns null if not a valid callout.
        /// </summary>
        /// <param name="handle">The LSPDFR LHandle object.</param>
        /// <returns>A Callout object contained within the LHandle.</returns>
        public static LSPD_First_Response.Mod.Callouts.Callout GetCalloutFromHandle(LSPD_First_Response.Mod.API.LHandle handle)
        {
            if (handle != null)
            {
                try
                {
                    foreach (var props in handle.GetType().GetRuntimeProperties())
                    {
                        if (props.GetValue(handle) is LSPD_First_Response.Mod.Callouts.Callout callout)
                        {
                            return callout;
                        }
                    }
                }
                catch
                {
                    // just return null
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the callouts.
        /// </summary>
        /// <returns>A dictionary with plugin names as the key and a list of callout names as the value.</returns>
        public static Dictionary<string, List<string>> GetCallouts()
        {
            var callouts = new Dictionary<string, List<string>>
            {
                { "LSPDFR", new List<string>() { "Holdup", "Pursuit" } },
            };

            foreach (var plugin in LSPD_First_Response.Mod.API.Functions.GetAllUserPlugins())
            {
                var names = new List<string>();
                try
                {
                    foreach (var callout in plugin.GetTypes().Where(x => typeof(LSPD_First_Response.Mod.Callouts.Callout).IsAssignableFrom(x)))
                    {
                        var attribute = callout.GetCustomAttributes(typeof(LSPD_First_Response.Mod.Callouts.CalloutInfoAttribute), true).FirstOrDefault();
                        if (attribute is LSPD_First_Response.Mod.Callouts.CalloutInfoAttribute calloutInfo)
                        {
                            names.Add(calloutInfo.Name);
                        }
                    }

                    if (names.Any())
                    {
                        callouts[plugin.GetName().Name] = names;
                    }
                }
                catch (System.Reflection.ReflectionTypeLoadException ex)
                {
                    Logging.Warning($"there was an error while trying to access plugin: {plugin.FullName}");
                    foreach (var entry in ex.LoaderExceptions)
                    {
                        Logging.Error($"{plugin.FullName}: ", entry);
                    }
                }
            }

            return callouts;
        }
    }
}
