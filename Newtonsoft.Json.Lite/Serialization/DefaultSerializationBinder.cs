namespace Newtonsoft.Json.Serialization
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Utilities;
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;

    public class DefaultSerializationBinder : SerializationBinder
    {
        private readonly ThreadSafeStore<TypeNameKey, Type> _typeCache = new ThreadSafeStore<TypeNameKey, Type>(new Func<TypeNameKey, Type>(DefaultSerializationBinder.GetTypeFromTypeNameKey));
        internal static readonly DefaultSerializationBinder Instance = new DefaultSerializationBinder();

        public override Type BindToType(string assemblyName, string typeName)
        {
            return this._typeCache.Get(new TypeNameKey(assemblyName, typeName));
        }

        private static Type GetTypeFromTypeNameKey(TypeNameKey typeNameKey)
        {
            string assemblyName = typeNameKey.AssemblyName;
            string typeName = typeNameKey.TypeName;
            if (assemblyName != null)
            {
                Assembly assembly = Assembly.LoadWithPartialName(assemblyName);
                if (assembly == null)
                {
                    throw new JsonSerializationException("Could not load assembly '{0}'.".FormatWith(CultureInfo.InvariantCulture, new object[] { assemblyName }));
                }
                Type type = assembly.GetType(typeName);
                if (type == null)
                {
                    throw new JsonSerializationException("Could not find type '{0}' in assembly '{1}'.".FormatWith(CultureInfo.InvariantCulture, new object[] { typeName, assembly.FullName }));
                }
                return type;
            }
            return Type.GetType(typeName);
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct TypeNameKey : IEquatable<DefaultSerializationBinder.TypeNameKey>
        {
            internal readonly string AssemblyName;
            internal readonly string TypeName;
            public TypeNameKey(string assemblyName, string typeName)
            {
                this.AssemblyName = assemblyName;
                this.TypeName = typeName;
            }

            public override int GetHashCode()
            {
                return (((this.AssemblyName != null) ? this.AssemblyName.GetHashCode() : 0) ^ ((this.TypeName != null) ? this.TypeName.GetHashCode() : 0));
            }

            public override bool Equals(object obj)
            {
                return ((obj is DefaultSerializationBinder.TypeNameKey) && this.Equals((DefaultSerializationBinder.TypeNameKey) obj));
            }

            public bool Equals(DefaultSerializationBinder.TypeNameKey other)
            {
                return ((this.AssemblyName == other.AssemblyName) && (this.TypeName == other.TypeName));
            }
        }
    }
}

