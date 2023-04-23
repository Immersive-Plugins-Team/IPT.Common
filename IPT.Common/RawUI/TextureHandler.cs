using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using IPT.Common.API;
using Rage;

namespace IPT.Common.RawUI
{
    /// <summary>
    /// Class for loading and accessing textures.
    /// </summary>
    public static class TextureHandler
    {
        private static readonly Dictionary<Assembly, Dictionary<string, Texture>> TexturesByAssembly = new Dictionary<Assembly, Dictionary<string, Texture>>();

        /// <summary>
        /// Loads textures from the specified directory path.  You must call this from your plugin if you want to access your textures.
        /// </summary>
        /// <param name="path">The path to the directory containing the textures.</param>
        public static void Load(string path)
        {
            var textures = new Dictionary<string, Texture>();
            try
            {
                foreach (string file in Directory.GetFiles(path, "*.png", SearchOption.AllDirectories))
                {
                    var texture = Functions.LoadTexture(file);
                    if (texture != null)
                    {
                        var name = GetRelativePath(path, file);
                        textures[name] = texture;
                        Logging.Debug($"loaded texture: {name}");
                    }
                    else
                    {
                        Logging.Warning($"could not load texture: {file}");
                    }
                }

                var assembly = Assembly.GetCallingAssembly();
                TexturesByAssembly[assembly] = textures;
                Logging.Info($"loaded {textures.Count} textures for {assembly.FullName}");
            }
            catch (IOException ex)
            {
                Logging.Error($"cannot access texture path: {path}", ex);
            }
            catch (System.Security.SecurityException ex)
            {
                Logging.Error($"LoadTextures cannot access calling assembly!", ex);
            }
        }

        /// <summary>
        /// Gets a texture by its name for the specified assembly.
        /// </summary>
        /// <param name="name">The name of the texture.</param>
        /// <returns>The texture if it exists, otherwise null.</returns>
        public static Texture Get(string name)
        {
            var assembly = Assembly.GetCallingAssembly();
            if (TexturesByAssembly.TryGetValue(assembly, out Dictionary<string, Texture> textures))
            {
                if (textures.TryGetValue(name, out Texture texture))
                {
                    return texture;
                }
            }

            Logging.Warning($"could not load texture {name} for assembly {assembly.FullName}");
            return null;
        }

        private static string GetRelativePath(string rootPath, string fullPath)
        {
            Uri rootUri = new Uri(rootPath);
            Uri fullUri = new Uri(fullPath);
            return rootUri.MakeRelativeUri(fullUri).ToString();
        }
    }
}
