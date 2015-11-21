using Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ConsoleClient.Config
{
    public class ConfigManager<T> where T : new()
    {
        private readonly Dictionary<string, PropertyInfo> _configMaps;
        private bool _loaded;
        private readonly T _config;

        private static ConfigManager<T> _instance;

        public static ConfigManager<T> Resolve(string path)
        {
            return _instance ?? (_instance = new ConfigManager<T>(path));
        }

        private ConfigManager(string path)
        {
            _configMaps = new Dictionary<string, PropertyInfo>();
            _config = new T();
            Load(path);
        }

        public void Load(string path)
        {
            if (_loaded) return;
            foreach (var p in typeof(T).GetProperties())
            {
                _configMaps.Add(p.Name.ToLower(), p);
            }

            var line = "";
            try
            {
                using (var sr = new StreamReader(path))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (!line.Contains("="))
                            continue;

                        var split = line.Split('=');

                        var cmd = split[0].ToLower();
                        var val = split[1];

                        var prop = _configMaps[cmd];

                        prop.SetValue(_config, Convert.ChangeType(val, prop.PropertyType));
                    }
                }
                _loaded = true;
            }
            catch (Exception ex)
            {
                Out.Red(string.Format("Exception occured in config load: {0}\nOffending line: {1}\n{2}", ex.Message, line, ex.StackTrace));
            }
        }

        /// <summary>
        /// Attempt to get a config value of the provided type.
        /// </summary>
        /// <typeparam name="T">Expected type of the value.</typeparam>
        /// <param name="action">Action to execute.</param>
        /// <returns>The config value, or default if it is not found.</returns>
        public TTarget TryGet<TTarget>(Func<T, TTarget> action)
        {
            try
            {
                return action(_config);
            }
            catch
            {
                return default(TTarget);
            }
        }

        /// <summary>
        /// Provide a func to evaluate on the config instance.
        /// </summary>
        /// <param name="action">Action to execute.</param>
        /// <returns>Whether action is true or false</returns>
        public bool Query(Func<T, bool> action)
        {
            return action(_config);
        }
    }
}
