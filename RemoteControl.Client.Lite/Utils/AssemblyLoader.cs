using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RemoteControl.Client.Utils
{
    /// <summary>
    /// 程序集加载器
    /// <para>注：用于从资源中动态加载Deflate压缩的程序集</para>
    /// </summary>
    static class AssemblyLoader
    {
        private static readonly object _nullCacheLock = new object();
        private static readonly Dictionary<string, bool> _nullCache = new Dictionary<string, bool>();
        private static readonly Dictionary<string, string> _assemblyNames = new Dictionary<string, string>();

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="resFullName">资源全称</param>
        public static void Register(string assemblyName, string resFullName)
        {
            assemblyName = assemblyName.ToLower();
            if (_assemblyNames.ContainsKey(assemblyName))
                return;
            _assemblyNames.Add(assemblyName, resFullName);
        }

        /// <summary>
        /// 关联
        /// </summary>
        public static void Attach()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyLoader.ResolveAssembly);
        }

        private static Assembly ResolveAssembly(object sender, ResolveEventArgs e)
        {
            object obj = AssemblyLoader._nullCacheLock;
            lock (obj)
            {
                if (AssemblyLoader._nullCache.ContainsKey(e.Name))
                {
                    Assembly result = null;
                    return result;
                }
            }
            AssemblyName assemblyName = new AssemblyName(e.Name);
            Assembly assembly = AssemblyLoader.ReadExistingAssembly(assemblyName);
            if (assembly != null)
            {
                return assembly;
            }
            assembly = AssemblyLoader.ReadFromEmbeddedResources(AssemblyLoader._assemblyNames, assemblyName);
            if (assembly == null)
            {
                obj = AssemblyLoader._nullCacheLock;
                lock (obj)
                {
                    AssemblyLoader._nullCache[e.Name] = true;
                }
                if (assemblyName.Flags == AssemblyNameFlags.Retargetable)
                {
                    assembly = Assembly.Load(assemblyName);
                }
            }
            return assembly;
        }

        private static string CultureToString(CultureInfo culture)
        {
            if (culture == null)
            {
                return "";
            }
            return culture.Name;
        }

        private static Assembly ReadExistingAssembly(AssemblyName name)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                Assembly assembly = assemblies[i];
                AssemblyName name2 = assembly.GetName();
                if (string.Equals(name2.Name, name.Name, StringComparison.InvariantCultureIgnoreCase) && string.Equals(AssemblyLoader.CultureToString(name2.CultureInfo), AssemblyLoader.CultureToString(name.CultureInfo), StringComparison.InvariantCultureIgnoreCase))
                {
                    return assembly;
                }
            }
            return null;
        }

        private static void CopyTo(Stream source, Stream destination)
        {
            byte[] array = new byte[81920];
            int count;
            while ((count = source.Read(array, 0, array.Length)) != 0)
            {
                destination.Write(array, 0, count);
            }
        }

        private static Stream LoadStream(string fullname)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            if (fullname.EndsWith(".zip"))
            {
                using (Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(fullname))
                {
                    using (DeflateStream deflateStream = new DeflateStream(manifestResourceStream, CompressionMode.Decompress))
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        AssemblyLoader.CopyTo(deflateStream, memoryStream);
                        memoryStream.Position = 0L;
                        return memoryStream;
                    }
                }
            }
            return executingAssembly.GetManifestResourceStream(fullname);
        }

        private static Stream LoadStream(Dictionary<string, string> resourceNames, string name)
        {
            string fullname;
            if (resourceNames.TryGetValue(name, out fullname))
            {
                return AssemblyLoader.LoadStream(fullname);
            }
            return null;
        }

        private static byte[] ReadStream(Stream stream)
        {
            byte[] array = new byte[stream.Length];
            stream.Read(array, 0, array.Length);
            return array;
        }

        private static Assembly ReadFromEmbeddedResources(Dictionary<string, string> assemblyNames, AssemblyName requestedAssemblyName)
        {
            string text = requestedAssemblyName.Name.ToLowerInvariant();
            if (requestedAssemblyName.CultureInfo != null && !string.IsNullOrEmpty(requestedAssemblyName.CultureInfo.Name))
            {
                text = string.Format("{0}.{1}", requestedAssemblyName.CultureInfo.Name, text);
            }
            byte[] rawAssembly;
            using (Stream stream = AssemblyLoader.LoadStream(assemblyNames, text))
            {
                if (stream == null)
                {
                    Assembly result = null;
                    return result;
                }
                rawAssembly = AssemblyLoader.ReadStream(stream);
            }
            return Assembly.Load(rawAssembly);
        }
    }
}
