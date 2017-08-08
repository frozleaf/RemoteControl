namespace Newtonsoft.Json.Utilities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal static class CollectionUtils
    {
        public static bool AddDistinct<T>(this IList<T> list, T value)
        {
            return list.AddDistinct<T>(value, EqualityComparer<T>.Default);
        }

        public static bool AddDistinct<T>(this IList<T> list, T value, IEqualityComparer<T> comparer)
        {
            if (list.ContainsValue<T>(value, comparer))
            {
                return false;
            }
            list.Add(value);
            return true;
        }

        public static void AddRange<T>(this IList<T> initial, IEnumerable<T> collection)
        {
            if (initial == null)
            {
                throw new ArgumentNullException("initial");
            }
            if (collection != null)
            {
                foreach (T local in collection)
                {
                    initial.Add(local);
                }
            }
        }

        public static void AddRange(this IList initial, IEnumerable collection)
        {
            ValidationUtils.ArgumentNotNull(initial, "initial");
            new ListWrapper<object>(initial).AddRange<object>(collection.Cast<object>());
        }

        public static bool AddRangeDistinct<T>(this IList<T> list, IEnumerable<T> values)
        {
            return list.AddRangeDistinct<T>(values, EqualityComparer<T>.Default);
        }

        public static bool AddRangeDistinct<T>(this IList<T> list, IEnumerable<T> values, IEqualityComparer<T> comparer)
        {
            bool flag = true;
            foreach (T local in values)
            {
                if (!list.AddDistinct<T>(local, comparer))
                {
                    flag = false;
                }
            }
            return flag;
        }

        public static IEnumerable<T> CastValid<T>(this IEnumerable enumerable)
        {
            ValidationUtils.ArgumentNotNull(enumerable, "enumerable");
            return (from o in enumerable.Cast<object>()
                where o is T
                select o).Cast<T>();
        }

        public static bool ContainsValue<TSource>(this IEnumerable<TSource> source, TSource value, IEqualityComparer<TSource> comparer)
        {
            if (comparer == null)
            {
                comparer = EqualityComparer<TSource>.Default;
            }
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            foreach (TSource local in source)
            {
                if (comparer.Equals(local, value))
                {
                    return true;
                }
            }
            return false;
        }

        public static object CreateAndPopulateList(Type listType, Action<IList, bool> populateList)
        {
            IList list;
            ValidationUtils.ArgumentNotNull(listType, "listType");
            ValidationUtils.ArgumentNotNull(populateList, "populateList");
            bool flag = false;
            if (listType.IsArray)
            {
                list = new List<object>();
                flag = true;
            }
            else
            {
                Type type;
                if (!ReflectionUtils.InheritsGenericDefinition(listType, typeof(ReadOnlyCollection<>), out type))
                {
                    if (typeof(IList).IsAssignableFrom(listType))
                    {
                        if (ReflectionUtils.IsInstantiatableType(listType))
                        {
                            list = (IList) Activator.CreateInstance(listType);
                        }
                        else if (listType == typeof(IList))
                        {
                            list = new List<object>();
                        }
                        else
                        {
                            list = null;
                        }
                    }
                    else if (ReflectionUtils.ImplementsGenericDefinition(listType, typeof(ICollection<>)))
                    {
                        if (ReflectionUtils.IsInstantiatableType(listType))
                        {
                            list = CreateCollectionWrapper(Activator.CreateInstance(listType));
                        }
                        else
                        {
                            list = null;
                        }
                    }
                    else
                    {
                        list = null;
                    }
                }
                else
                {
                    Type type2 = type.GetGenericArguments()[0];
                    Type type3 = ReflectionUtils.MakeGenericType(typeof(IEnumerable<>), new Type[] { type2 });
                    bool flag2 = false;
                    foreach (ConstructorInfo info in listType.GetConstructors())
                    {
                        IList<ParameterInfo> parameters = info.GetParameters();
                        if ((parameters.Count == 1) && type3.IsAssignableFrom(parameters[0].ParameterType))
                        {
                            flag2 = true;
                            break;
                        }
                    }
                    if (!flag2)
                    {
                        throw new Exception("Read-only type {0} does not have a public constructor that takes a type that implements {1}.".FormatWith(CultureInfo.InvariantCulture, new object[] { listType, type3 }));
                    }
                    list = CreateGenericList(type2);
                    flag = true;
                }
            }
            if (list == null)
            {
                throw new Exception("Cannot create and populate list type {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { listType }));
            }
            populateList(list, flag);
            if (flag)
            {
                if (listType.IsArray)
                {
                    list = ToArray(((List<object>) list).ToArray(), ReflectionUtils.GetCollectionItemType(listType));
                }
                else if (ReflectionUtils.InheritsGenericDefinition(listType, typeof(ReadOnlyCollection<>)))
                {
                    list = (IList) ReflectionUtils.CreateInstance(listType, new object[] { list });
                }
                return list;
            }
            if (list is IWrappedCollection)
            {
                return ((IWrappedCollection) list).UnderlyingCollection;
            }
            return list;
        }

        public static IWrappedCollection CreateCollectionWrapper(object list)
        {
            Func<Type, IList<object>, object> func2 = null;
            Type collectionDefinition;
            ValidationUtils.ArgumentNotNull(list, "list");
            if (ReflectionUtils.ImplementsGenericDefinition(list.GetType(), typeof(ICollection<>), out collectionDefinition))
            {
                Type collectionItemType = ReflectionUtils.GetCollectionItemType(collectionDefinition);
                if (func2 == null)
                {
                    func2 = (t, a) => t.GetConstructor(new Type[] { collectionDefinition }).Invoke(new object[] { list });
                }
                Func<Type, IList<object>, object> instanceCreator = func2;
                return (IWrappedCollection) ReflectionUtils.CreateGeneric(typeof(CollectionWrapper<>), new Type[] { collectionItemType }, instanceCreator, new object[] { list });
            }
            if (!(list is IList))
            {
                throw new Exception("Can not create ListWrapper for type {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { list.GetType() }));
            }
            return new CollectionWrapper<object>((IList) list);
        }

        public static IWrappedDictionary CreateDictionaryWrapper(object dictionary)
        {
            Func<Type, IList<object>, object> func2 = null;
            Type dictionaryDefinition;
            ValidationUtils.ArgumentNotNull(dictionary, "dictionary");
            if (ReflectionUtils.ImplementsGenericDefinition(dictionary.GetType(), typeof(IDictionary<,>), out dictionaryDefinition))
            {
                Type dictionaryKeyType = ReflectionUtils.GetDictionaryKeyType(dictionaryDefinition);
                Type dictionaryValueType = ReflectionUtils.GetDictionaryValueType(dictionaryDefinition);
                if (func2 == null)
                {
                    func2 = (t, a) => t.GetConstructor(new Type[] { dictionaryDefinition }).Invoke(new object[] { dictionary });
                }
                Func<Type, IList<object>, object> instanceCreator = func2;
                return (IWrappedDictionary) ReflectionUtils.CreateGeneric(typeof(DictionaryWrapper<,>), new Type[] { dictionaryKeyType, dictionaryValueType }, instanceCreator, new object[] { dictionary });
            }
            if (!(dictionary is IDictionary))
            {
                throw new Exception("Can not create DictionaryWrapper for type {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { dictionary.GetType() }));
            }
            return new DictionaryWrapper<object, object>((IDictionary) dictionary);
        }

        public static IDictionary CreateGenericDictionary(Type keyType, Type valueType)
        {
            ValidationUtils.ArgumentNotNull(keyType, "keyType");
            ValidationUtils.ArgumentNotNull(valueType, "valueType");
            return (IDictionary) ReflectionUtils.CreateGeneric(typeof(Dictionary<,>), keyType, new object[] { valueType });
        }

        public static IList CreateGenericList(Type listType)
        {
            ValidationUtils.ArgumentNotNull(listType, "listType");
            return (IList) ReflectionUtils.CreateGeneric(typeof(List<>), listType, new object[0]);
        }

        public static List<T> CreateList<T>(params T[] values)
        {
            return new List<T>(values);
        }

        public static List<T> CreateList<T>(ICollection collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            T[] array = new T[collection.Count];
            collection.CopyTo(array, 0);
            return new List<T>(array);
        }

        public static IWrappedList CreateListWrapper(object list)
        {
            Func<Type, IList<object>, object> func2 = null;
            Type listDefinition;
            ValidationUtils.ArgumentNotNull(list, "list");
            if (ReflectionUtils.ImplementsGenericDefinition(list.GetType(), typeof(IList<>), out listDefinition))
            {
                Type collectionItemType = ReflectionUtils.GetCollectionItemType(listDefinition);
                if (func2 == null)
                {
                    func2 = (t, a) => t.GetConstructor(new Type[] { listDefinition }).Invoke(new object[] { list });
                }
                Func<Type, IList<object>, object> instanceCreator = func2;
                return (IWrappedList) ReflectionUtils.CreateGeneric(typeof(ListWrapper<>), new Type[] { collectionItemType }, instanceCreator, new object[] { list });
            }
            if (!(list is IList))
            {
                throw new Exception("Can not create ListWrapper for type {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { list.GetType() }));
            }
            return new ListWrapper<object>((IList) list);
        }

        public static List<T> Distinct<T>(List<T> collection)
        {
            List<T> list = new List<T>();
            foreach (T local in collection)
            {
                if (!list.Contains(local))
                {
                    list.Add(local);
                }
            }
            return list;
        }

        public static List<List<T>> Flatten<T>(params IList<T>[] lists)
        {
            List<List<T>> flattenedResult = new List<List<T>>();
            Dictionary<int, T> currentSet = new Dictionary<int, T>();
            Recurse<T>(new List<IList<T>>(lists), 0, currentSet, flattenedResult);
            return flattenedResult;
        }

        public static T GetSingleItem<T>(IList<T> list)
        {
            return GetSingleItem<T>(list, false);
        }

        public static T GetSingleItem<T>(IList<T> list, bool returnDefaultIfEmpty)
        {
            if (list.Count == 1)
            {
                return list[0];
            }
            if (!returnDefaultIfEmpty || (list.Count != 0))
            {
                throw new Exception("Expected single {0} in list but got {1}.".FormatWith(CultureInfo.InvariantCulture, new object[] { typeof(T), list.Count }));
            }
            return default(T);
        }

        public static Dictionary<K, List<V>> GroupBy<K, V>(ICollection<V> source, Func<V, K> keySelector)
        {
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }
            Dictionary<K, List<V>> dictionary = new Dictionary<K, List<V>>();
            foreach (V local in source)
            {
                List<V> list;
                K key = keySelector(local);
                if (!dictionary.TryGetValue(key, out list))
                {
                    list = new List<V>();
                    dictionary.Add(key, list);
                }
                list.Add(local);
            }
            return dictionary;
        }

        public static int IndexOf<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
        {
            int num = 0;
            foreach (T local in collection)
            {
                if (predicate(local))
                {
                    return num;
                }
                num++;
            }
            return -1;
        }

        public static int IndexOf<TSource>(this IEnumerable<TSource> list, TSource value) where TSource: IEquatable<TSource>
        {
            return list.IndexOf<TSource>(value, EqualityComparer<TSource>.Default);
        }

        public static int IndexOf<TSource>(this IEnumerable<TSource> list, TSource value, IEqualityComparer<TSource> comparer)
        {
            int num = 0;
            foreach (TSource local in list)
            {
                if (comparer.Equals(local, value))
                {
                    return num;
                }
                num++;
            }
            return -1;
        }

        public static bool IsCollectionType(Type type)
        {
            ValidationUtils.ArgumentNotNull(type, "type");
            return (type.IsArray || (typeof(ICollection).IsAssignableFrom(type) || ReflectionUtils.ImplementsGenericDefinition(type, typeof(ICollection<>))));
        }

        public static bool IsDictionaryType(Type type)
        {
            ValidationUtils.ArgumentNotNull(type, "type");
            return (typeof(IDictionary).IsAssignableFrom(type) || ReflectionUtils.ImplementsGenericDefinition(type, typeof(IDictionary<,>)));
        }

        public static bool IsListType(Type type)
        {
            ValidationUtils.ArgumentNotNull(type, "type");
            return (type.IsArray || (typeof(IList).IsAssignableFrom(type) || ReflectionUtils.ImplementsGenericDefinition(type, typeof(IList<>))));
        }

        public static bool IsNullOrEmpty<T>(ICollection<T> collection)
        {
            if (collection != null)
            {
                return (collection.Count == 0);
            }
            return true;
        }

        public static bool IsNullOrEmpty(ICollection collection)
        {
            if (collection != null)
            {
                return (collection.Count == 0);
            }
            return true;
        }

        public static bool IsNullOrEmptyOrDefault<T>(IList<T> list)
        {
            return (IsNullOrEmpty<T>(list) || ReflectionUtils.ItemsUnitializedValue<T>(list));
        }

        public static bool ListEquals<T>(IList<T> a, IList<T> b)
        {
            if ((a == null) || (b == null))
            {
                return ((a == null) && (b == null));
            }
            if (a.Count != b.Count)
            {
                return false;
            }
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < a.Count; i++)
            {
                if (!comparer.Equals(a[i], b[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static IList<T> Minus<T>(IList<T> list, IList<T> minus)
        {
            ValidationUtils.ArgumentNotNull(list, "list");
            List<T> list2 = new List<T>(list.Count);
            foreach (T local in list)
            {
                if (!((minus != null) && minus.Contains(local)))
                {
                    list2.Add(local);
                }
            }
            return list2;
        }

        private static void Recurse<T>(IList<IList<T>> global, int current, Dictionary<int, T> currentSet, List<List<T>> flattenedResult)
        {
            IList<T> list = global[current];
            for (int i = 0; i < list.Count; i++)
            {
                currentSet[current] = list[i];
                if (current == (global.Count - 1))
                {
                    List<T> item = new List<T>();
                    for (int j = 0; j < currentSet.Count; j++)
                    {
                        item.Add(currentSet[j]);
                    }
                    flattenedResult.Add(item);
                }
                else
                {
                    Recurse<T>(global, current + 1, currentSet, flattenedResult);
                }
            }
        }

        public static IList<T> Slice<T>(IList<T> list, int? start, int? end)
        {
            return Slice<T>(list, start, end, null);
        }

        public static IList<T> Slice<T>(IList<T> list, int? start, int? end, int? step)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            if (step == 0)
            {
                throw new ArgumentException("Step cannot be zero.", "step");
            }
            List<T> list2 = new List<T>();
            if (list.Count != 0)
            {
                int? nullable = step;
                int num = nullable.HasValue ? nullable.GetValueOrDefault() : 1;
                nullable = start;
                int num2 = nullable.HasValue ? nullable.GetValueOrDefault() : 0;
                nullable = end;
                int num3 = nullable.HasValue ? nullable.GetValueOrDefault() : list.Count;
                num2 = (num2 < 0) ? (list.Count + num2) : num2;
                num3 = (num3 < 0) ? (list.Count + num3) : num3;
                num2 = Math.Max(num2, 0);
                num3 = Math.Min(num3, list.Count - 1);
                for (int i = num2; i < num3; i += num)
                {
                    list2.Add(list[i]);
                }
            }
            return list2;
        }

        public static Array ToArray(Array initial, Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            Array destinationArray = Array.CreateInstance(type, initial.Length);
            Array.Copy(initial, 0, destinationArray, 0, initial.Length);
            return destinationArray;
        }

        public static bool TryGetSingleItem<T>(IList<T> list, out T value)
        {
            return TryGetSingleItem<T>(list, false, out value);
        }

        public static bool TryGetSingleItem<T>(IList<T> list, bool returnDefaultIfEmpty, out T value)
        {
            return MiscellaneousUtils.TryAction<T>(() => GetSingleItem<T>(list, returnDefaultIfEmpty), out value);
        }
    }
}

