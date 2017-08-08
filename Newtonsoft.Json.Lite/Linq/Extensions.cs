using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Newtonsoft.Json.Linq
{
    public static class Extensions
    {
        public static IJEnumerable<JToken> Ancestors<T>(this IEnumerable<T> source) where T : JToken
        {
            ValidationUtils.ArgumentNotNull(source, "source");
            return source.SelectMany((T j) => j.Ancestors()).AsJEnumerable();
        }

        public static IJEnumerable<JToken> Descendants<T>(this IEnumerable<T> source) where T : JContainer
        {
            ValidationUtils.ArgumentNotNull(source, "source");
            return source.SelectMany((T j) => j.Descendants()).AsJEnumerable();
        }

        public static IJEnumerable<JProperty> Properties(this IEnumerable<JObject> source)
        {
            ValidationUtils.ArgumentNotNull(source, "source");
            return source.SelectMany((JObject d) => d.Properties()).AsJEnumerable<JProperty>();
        }

        public static IJEnumerable<JToken> Values(this IEnumerable<JToken> source, object key)
        {
            return source.Values(key).AsJEnumerable();
        }

        public static IJEnumerable<JToken> Values(this IEnumerable<JToken> source)
        {
            return source.Values(null);
        }

        public static IEnumerable<U> Values<U>(this IEnumerable<JToken> source, object key)
        {
            return source.Values<JToken,U>(key);
        }

        public static IEnumerable<U> Values<U>(this IEnumerable<JToken> source)
        {
            return source.Values<JToken, U>(null);
        }

        public static U Value<U>(this IEnumerable<JToken> value)
        {
            return value.Value<JToken, U>();
        }

        public static U Value<T, U>(this IEnumerable<T> value) where T : JToken
        {
            ValidationUtils.ArgumentNotNull(value, "source");
            JToken jToken = value as JToken;
            if (jToken == null)
            {
                throw new ArgumentException("Source value must be a JToken.");
            }
            return jToken.Convert<JToken, U>();
        }

        internal static IEnumerable<U> Values<T, U>(this IEnumerable<T> source, object key) where T : JToken
        {
            ValidationUtils.ArgumentNotNull(source, "source");
            foreach (JToken jToken in source)
            {
                if (key == null)
                {
                    if (jToken is JValue)
                    {
                        yield return ((JValue)jToken).Convert<JValue, U>();
                    }
                    else
                    {
                        foreach (JToken current in jToken.Children())
                        {
                            yield return current.Convert<JToken, U>();
                        }
                    }
                }
                else
                {
                    JToken jToken2 = jToken[key];
                    if (jToken2 != null)
                    {
                        yield return jToken2.Convert<JToken, U>();
                    }
                }
            }
            yield break;
        }

        public static IJEnumerable<JToken> Children<T>(this IEnumerable<T> source) where T : JToken
        {
            return source.Children<T, JToken>().AsJEnumerable();
        }

        public static IEnumerable<U> Children<T, U>(this IEnumerable<T> source) where T : JToken
        {
            ValidationUtils.ArgumentNotNull(source, "source");
            return source.SelectMany((T c) => c.Children()).Convert<JToken, U>();
        }

        internal static IEnumerable<U> Convert<T, U>(this IEnumerable<T> source) where T : JToken
        {
            ValidationUtils.ArgumentNotNull(source, "source");
            bool cast = typeof(JToken).IsAssignableFrom(typeof(U));
            foreach (JToken token in source)
            {
                yield return token.Convert<JToken, U>(cast);
            }
            yield break;
        }

        internal static U Convert<T, U>(this T token) where T : JToken
        {
            bool cast = typeof(JToken).IsAssignableFrom(typeof(U));
            return token.Convert<T,U>(cast);
        }

        internal static U Convert<T, U>(this T token, bool cast) where T : JToken
        {
            U result;
            if (cast)
            {
                result = (U)((object)token);
            }
            else if (token == null)
            {
                result = default(U);
            }
            else
            {
                JValue jValue = token as JValue;
                if (jValue == null)
                {
                    throw new InvalidCastException("Cannot cast {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						token.GetType(),
						typeof(T)
					}));
                }
                if (jValue.Value is U)
                {
                    result = (U)((object)jValue.Value);
                }
                else
                {
                    Type type = typeof(U);
                    if (ReflectionUtils.IsNullableType(type))
                    {
                        if (jValue.Value == null)
                        {
                            result = default(U);
                            return result;
                        }
                        type = Nullable.GetUnderlyingType(type);
                    }
                    result = (U)((object)System.Convert.ChangeType(jValue.Value, type, CultureInfo.InvariantCulture));
                }
            }
            return result;
        }

        public static IJEnumerable<JToken> AsJEnumerable(this IEnumerable<JToken> source)
        {
            return source.AsJEnumerable<JToken>();
        }

        public static IJEnumerable<T> AsJEnumerable<T>(this IEnumerable<T> source) where T : JToken
        {
            IJEnumerable<T> result;
            if (source == null)
            {
                result = null;
            }
            else if (source is IJEnumerable<T>)
            {
                result = (IJEnumerable<T>)source;
            }
            else
            {
                result = new JEnumerable<T>(source);
            }
            return result;
        }

    }
}
