namespace Newtonsoft.Json.Utilities
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Reflection.Emit;

    internal class DynamicReflectionDelegateFactory : ReflectionDelegateFactory
    {
        public static DynamicReflectionDelegateFactory Instance = new DynamicReflectionDelegateFactory();

        public override Func<T> CreateDefaultConstructor<T>(Type type)
        {
            DynamicMethod method = CreateDynamicMethod("Create" + type.FullName, typeof(object), Type.EmptyTypes, type);
            method.InitLocals = true;
            ILGenerator iLGenerator = method.GetILGenerator();
            if (type.IsValueType)
            {
                iLGenerator.DeclareLocal(type);
                iLGenerator.Emit(OpCodes.Ldloc_0);
                iLGenerator.Emit(OpCodes.Box, type);
            }
            else
            {
                ConstructorInfo con = type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
                if (con == null)
                {
                    throw new Exception("Could not get constructor for {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { type }));
                }
                iLGenerator.Emit(OpCodes.Newobj, con);
            }
            iLGenerator.Return();
            return (Func<T>) method.CreateDelegate(typeof(Func<T>));
        }

        private static DynamicMethod CreateDynamicMethod(string name, Type returnType, Type[] parameterTypes, Type owner)
        {
            return (!owner.IsInterface ? new DynamicMethod(name, returnType, parameterTypes, owner, true) : new DynamicMethod(name, returnType, parameterTypes, owner.Module, true));
        }

        public override Func<T, object> CreateGet<T>(FieldInfo fieldInfo)
        {
            DynamicMethod method = CreateDynamicMethod("Get" + fieldInfo.Name, typeof(T), new Type[] { typeof(object) }, fieldInfo.DeclaringType);
            ILGenerator iLGenerator = method.GetILGenerator();
            if (!fieldInfo.IsStatic)
            {
                iLGenerator.PushInstance(fieldInfo.DeclaringType);
            }
            iLGenerator.Emit(OpCodes.Ldfld, fieldInfo);
            iLGenerator.BoxIfNeeded(fieldInfo.FieldType);
            iLGenerator.Return();
            return (Func<T, object>) method.CreateDelegate(typeof(Func<T, object>));
        }

        public override Func<T, object> CreateGet<T>(PropertyInfo propertyInfo)
        {
            MethodInfo getMethod = propertyInfo.GetGetMethod(true);
            if (getMethod == null)
            {
                throw new Exception("Property '{0}' does not have a getter.".FormatWith(CultureInfo.InvariantCulture, new object[] { propertyInfo.Name }));
            }
            DynamicMethod method = CreateDynamicMethod("Get" + propertyInfo.Name, typeof(T), new Type[] { typeof(object) }, propertyInfo.DeclaringType);
            ILGenerator iLGenerator = method.GetILGenerator();
            if (!getMethod.IsStatic)
            {
                iLGenerator.PushInstance(propertyInfo.DeclaringType);
            }
            iLGenerator.CallMethod(getMethod);
            iLGenerator.BoxIfNeeded(propertyInfo.PropertyType);
            iLGenerator.Return();
            return (Func<T, object>) method.CreateDelegate(typeof(Func<T, object>));
        }

        public override MethodCall<T, object> CreateMethodCall<T>(MethodBase method)
        {
            DynamicMethod method2 = CreateDynamicMethod(method.ToString(), typeof(object), new Type[] { typeof(object), typeof(object[]) }, method.DeclaringType);
            ILGenerator iLGenerator = method2.GetILGenerator();
            ParameterInfo[] parameters = method.GetParameters();
            Label label = iLGenerator.DefineLabel();
            iLGenerator.Emit(OpCodes.Ldarg_1);
            iLGenerator.Emit(OpCodes.Ldlen);
            iLGenerator.Emit(OpCodes.Ldc_I4, parameters.Length);
            iLGenerator.Emit(OpCodes.Beq, label);
            iLGenerator.Emit(OpCodes.Newobj, typeof(TargetParameterCountException).GetConstructor(Type.EmptyTypes));
            iLGenerator.Emit(OpCodes.Throw);
            iLGenerator.MarkLabel(label);
            if (!(method.IsConstructor || method.IsStatic))
            {
                iLGenerator.PushInstance(method.DeclaringType);
            }
            for (int i = 0; i < parameters.Length; i++)
            {
                iLGenerator.Emit(OpCodes.Ldarg_1);
                iLGenerator.Emit(OpCodes.Ldc_I4, i);
                iLGenerator.Emit(OpCodes.Ldelem_Ref);
                iLGenerator.UnboxIfNeeded(parameters[i].ParameterType);
            }
            if (method.IsConstructor)
            {
                iLGenerator.Emit(OpCodes.Newobj, (ConstructorInfo) method);
            }
            else if (!(!method.IsFinal && method.IsVirtual))
            {
                iLGenerator.CallMethod((MethodInfo) method);
            }
            Type type = method.IsConstructor ? method.DeclaringType : ((MethodInfo) method).ReturnType;
            if (type != typeof(void))
            {
                iLGenerator.BoxIfNeeded(type);
            }
            else
            {
                iLGenerator.Emit(OpCodes.Ldnull);
            }
            iLGenerator.Return();
            return (MethodCall<T, object>) method2.CreateDelegate(typeof(MethodCall<T, object>));
        }

        public override Action<T, object> CreateSet<T>(FieldInfo fieldInfo)
        {
            DynamicMethod method = CreateDynamicMethod("Set" + fieldInfo.Name, null, new Type[] { typeof(object), typeof(object) }, fieldInfo.DeclaringType);
            ILGenerator iLGenerator = method.GetILGenerator();
            if (!fieldInfo.IsStatic)
            {
                iLGenerator.PushInstance(fieldInfo.DeclaringType);
            }
            iLGenerator.Emit(OpCodes.Ldarg_1);
            iLGenerator.UnboxIfNeeded(fieldInfo.FieldType);
            iLGenerator.Emit(OpCodes.Stfld, fieldInfo);
            iLGenerator.Return();
            return (Action<T, object>) method.CreateDelegate(typeof(Action<T, object>));
        }

        public override Action<T, object> CreateSet<T>(PropertyInfo propertyInfo)
        {
            MethodInfo setMethod = propertyInfo.GetSetMethod(true);
            DynamicMethod method = CreateDynamicMethod("Set" + propertyInfo.Name, null, new Type[] { typeof(object), typeof(object) }, propertyInfo.DeclaringType);
            ILGenerator iLGenerator = method.GetILGenerator();
            if (!setMethod.IsStatic)
            {
                iLGenerator.PushInstance(propertyInfo.DeclaringType);
            }
            iLGenerator.Emit(OpCodes.Ldarg_1);
            iLGenerator.UnboxIfNeeded(propertyInfo.PropertyType);
            iLGenerator.CallMethod(setMethod);
            iLGenerator.Return();
            return (Action<T, object>) method.CreateDelegate(typeof(Action<T, object>));
        }
    }
}

