namespace Newtonsoft.Json.Utilities
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Resources;

    internal static class DynamicWrapper
    {
        private static readonly object _lock = new object();
        private static System.Reflection.Emit.ModuleBuilder _moduleBuilder;
        private static readonly WrapperDictionary _wrapperDictionary = new WrapperDictionary();

        public static T CreateWrapper<T>(object realObject) where T: class
        {
            DynamicWrapperBase base2 = (DynamicWrapperBase) Activator.CreateInstance(GetWrapper(typeof(T), realObject.GetType()));
            base2.UnderlyingObject = realObject;
            return (base2 as T);
        }

        private static Type GenerateWrapperType(Type interfaceType, Type underlyingType)
        {
            TypeBuilder proxyBuilder = ModuleBuilder.DefineType("{0}_{1}_Wrapper".FormatWith(CultureInfo.InvariantCulture, new object[] { interfaceType.Name, underlyingType.Name }), TypeAttributes.Sealed, typeof(DynamicWrapperBase), new Type[] { interfaceType });
            WrapperMethodBuilder builder2 = new WrapperMethodBuilder(underlyingType, proxyBuilder);
            foreach (MethodInfo info in interfaceType.AllMethods())
            {
                builder2.Generate(info);
            }
            return proxyBuilder.CreateType();
        }

        private static byte[] GetStrongKey()
        {
            string name = "Newtonsoft.Json.Net35.Dynamic.snk";
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name))
            {
                if (stream == null)
                {
                    throw new MissingManifestResourceException("Should have a Newtonsoft.Json.Dynamic.snk as an embedded resource.");
                }
                int length = (int) stream.Length;
                byte[] buffer = new byte[length];
                stream.Read(buffer, 0, length);
                return buffer;
            }
        }

        public static object GetUnderlyingObject(object wrapper)
        {
            DynamicWrapperBase base2 = wrapper as DynamicWrapperBase;
            if (base2 == null)
            {
                throw new ArgumentException("Object is not a wrapper.", "wrapper");
            }
            return base2.UnderlyingObject;
        }

        public static Type GetWrapper(Type interfaceType, Type realObjectType)
        {
            Type wrapperType = _wrapperDictionary.GetType(interfaceType, realObjectType);
            if (wrapperType == null)
            {
                lock (_lock)
                {
                    wrapperType = _wrapperDictionary.GetType(interfaceType, realObjectType);
                    if (wrapperType == null)
                    {
                        wrapperType = GenerateWrapperType(interfaceType, realObjectType);
                        _wrapperDictionary.SetType(interfaceType, realObjectType, wrapperType);
                    }
                }
            }
            return wrapperType;
        }

        private static void Init()
        {
            if (_moduleBuilder == null)
            {
                lock (_lock)
                {
                    if (_moduleBuilder == null)
                    {
                        AssemblyName name = new AssemblyName("Newtonsoft.Json.Dynamic") {
                            KeyPair = new StrongNameKeyPair(GetStrongKey())
                        };
                        _moduleBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.Run).DefineDynamicModule("Newtonsoft.Json.DynamicModule", false);
                    }
                }
            }
        }

        private static System.Reflection.Emit.ModuleBuilder ModuleBuilder
        {
            get
            {
                Init();
                return _moduleBuilder;
            }
        }
    }
}

