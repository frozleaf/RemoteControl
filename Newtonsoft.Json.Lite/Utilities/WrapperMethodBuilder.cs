namespace Newtonsoft.Json.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    internal class WrapperMethodBuilder
    {
        private readonly Type _realObjectType;
        private readonly TypeBuilder _wrapperBuilder;

        public WrapperMethodBuilder(Type realObjectType, TypeBuilder proxyBuilder)
        {
            this._realObjectType = realObjectType;
            this._wrapperBuilder = proxyBuilder;
        }

        private void ExecuteMethod(MethodBase newMethod, Type[] parameterTypes, ILGenerator ilGenerator)
        {
            MethodInfo method = this.GetMethod(newMethod, parameterTypes);
            if (method == null)
            {
                throw new MissingMethodException("Unable to find method " + newMethod.Name + " on " + this._realObjectType.FullName);
            }
            ilGenerator.Emit(OpCodes.Call, method);
        }

        public void Generate(MethodInfo newMethod)
        {
            if (newMethod.IsGenericMethod)
            {
                newMethod = newMethod.GetGenericMethodDefinition();
            }
            FieldInfo field = typeof(DynamicWrapperBase).GetField("UnderlyingObject", BindingFlags.NonPublic | BindingFlags.Instance);
            ParameterInfo[] parameters = newMethod.GetParameters();
            Type[] parameterTypes = (from parameter in parameters select parameter.ParameterType).ToArray<Type>();
            MethodBuilder builder = this._wrapperBuilder.DefineMethod(newMethod.Name, MethodAttributes.Virtual | MethodAttributes.Public, newMethod.ReturnType, parameterTypes);
            if (newMethod.IsGenericMethod)
            {
                builder.DefineGenericParameters((from arg in newMethod.GetGenericArguments() select arg.Name).ToArray<string>());
            }
            ILGenerator iLGenerator = builder.GetILGenerator();
            LoadUnderlyingObject(iLGenerator, field);
            PushParameters(parameters, iLGenerator);
            this.ExecuteMethod(newMethod, parameterTypes, iLGenerator);
            Return(iLGenerator);
        }

        private MethodInfo GetMethod(MethodBase realMethod, Type[] parameterTypes)
        {
            if (realMethod.IsGenericMethod)
            {
                return this._realObjectType.GetGenericMethod(realMethod.Name, parameterTypes);
            }
            return this._realObjectType.GetMethod(realMethod.Name, parameterTypes);
        }

        private static void LoadUnderlyingObject(ILGenerator ilGenerator, FieldInfo srcField)
        {
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldfld, srcField);
        }

        private static void PushParameters(ICollection<ParameterInfo> parameters, ILGenerator ilGenerator)
        {
            for (int i = 1; i < (parameters.Count + 1); i++)
            {
                ilGenerator.Emit(OpCodes.Ldarg, i);
            }
        }

        private static void Return(ILGenerator ilGenerator)
        {
            ilGenerator.Emit(OpCodes.Ret);
        }
    }
}

