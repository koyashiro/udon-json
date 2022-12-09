namespace Koyashiro.UdonJson
{
    using Koyashiro.UdonException;
    using Koyashiro.UdonList;
    using Koyashiro.UdonDictionary;
    using YamlDotNet.Core.Tokens;

    public static class UdonJsonValueExt
    {
        private const string ERR_INVALID_KIND = "Invalid kind";

        public static UdonJsonValueKind GetKind(this UdonJsonValue value)
        {
            return (UdonJsonValueKind)(value.AsRawArray()[0]);
        }

        public static string AsString(this UdonJsonValue v)
        {
            if (v.GetKind() != UdonJsonValueKind.String)
            {
                UdonException.ThrowArgumentException(ERR_INVALID_KIND);
            }

            return (string)v.GetValueUnchecked();
        }

        public static double AsNumber(this UdonJsonValue v)
        {
            if (v.GetKind() != UdonJsonValueKind.Number)
            {
                UdonException.ThrowArgumentException(ERR_INVALID_KIND);
            }

            return (double)v.GetValueUnchecked();
        }

        public static bool AsBool(this UdonJsonValue v)
        {
            if (v.GetKind() != UdonJsonValueKind.True && v.GetKind() != UdonJsonValueKind.False)
            {
                UdonException.ThrowArgumentException(ERR_INVALID_KIND);
            }

            return (bool)v.GetValueUnchecked();
        }

        public static object AsNull(this UdonJsonValue v)
        {
            if (v.GetKind() != UdonJsonValueKind.Null)
            {
                UdonException.ThrowArgumentException(ERR_INVALID_KIND);
            }

            return null;
        }

        public static int Count(this UdonJsonValue v)
        {
            if (v.GetKind() == UdonJsonValueKind.Object)
            {
                return v.AsDictionary<string, object>().Count();
            }

            if (v.GetKind() == UdonJsonValueKind.Array)
            {
                return v.AsList().Count();
            }

            UdonException.ThrowArgumentException(ERR_INVALID_KIND);
            return default;
        }

        //public static string GetKey(this UdonJsonValue v, int index)
        //{
        //    if (v.GetKind() != UdonJsonValueKind.Object)
        //    {
        //        UdonException.ThrowArgumentException(ERR_INVALID_KIND);
        //    }
        //
        //    return v.AsDictionary<string, object>().GetKey(index);
        //}

        public static string[] Keys(this UdonJsonValue v)
        {
            if (v.GetKind() != UdonJsonValueKind.Object)
            {
                UdonException.ThrowArgumentException(ERR_INVALID_KIND);
            }

            return v.AsDictionary<string, object>().Keys();
        }

        public static UdonJsonValue GetValue(this UdonJsonValue v, string key)
        {
            if (v.GetKind() != UdonJsonValueKind.Object)
            {
                UdonException.ThrowArgumentException(ERR_INVALID_KIND);
            }

            return (UdonJsonValue)v.AsDictionary<string, object>().GetValue(key);
        }

        public static UdonJsonValue GetValue(this UdonJsonValue v, int key)
        {
            if (v.GetKind() != UdonJsonValueKind.Array)
            {
                UdonException.ThrowArgumentException(ERR_INVALID_KIND);
            }

            return (UdonJsonValue)(v.AsList().GetItem(key));
        }

        public static void SetValue(this UdonJsonValue v, string key, UdonJsonValue value)
        {
            if (v.GetKind() != UdonJsonValueKind.Object)
            {
                UdonException.ThrowArgumentException(ERR_INVALID_KIND);
            }

            v.AsDictionary<string, object>().SetValue(key, value);
        }

        public static void SetValue(this UdonJsonValue v, int key, UdonJsonValue value)
        {
            if (v.GetKind() != UdonJsonValueKind.Array)
            {
                UdonException.ThrowArgumentException(ERR_INVALID_KIND);
            }

            v.AsList().SetItem(key, value);
        }

        public static void SetValue(this UdonJsonValue v, string key, string value)
        {
            v.SetValue(key, UdonJsonValue.NewString(value));
        }

        public static void SetValue(this UdonJsonValue v, int index, string value)
        {
            v.SetValue(index, UdonJsonValue.NewString(value));
        }

        public static void SetValue(this UdonJsonValue v, string key, double value)
        {
            v.SetValue(key, UdonJsonValue.NewNumber(value));
        }

        public static void SetValue(this UdonJsonValue v, int index, double value)
        {
            v.SetValue(index, UdonJsonValue.NewNumber(value));
        }

        public static void SetValue(this UdonJsonValue v, string key, bool value)
        {
            if (value)
            {
                v.SetValue(key, UdonJsonValue.NewTrue());
            }
            else
            {
                v.SetValue(key, UdonJsonValue.NewFalse());
            }
        }

        public static void SetValue(this UdonJsonValue v, int index, bool value)
        {
            if (value)
            {
                v.SetValue(index, UdonJsonValue.NewTrue());
            }
            else
            {
                v.SetValue(index, UdonJsonValue.NewFalse());
            }
        }

        public static void SetNullValue(this UdonJsonValue v, string key)
        {
            v.SetValue(key, UdonJsonValue.NewNull());
        }

        public static void SetNullValue(this UdonJsonValue v, int index)
        {
            v.SetValue(index, UdonJsonValue.NewNull());
        }

        public static void AddValue(this UdonJsonValue v, UdonJsonValue value)
        {
            if (v.GetKind() != UdonJsonValueKind.Array)
            {
                UdonException.ThrowArgumentException(ERR_INVALID_KIND);
            }

            v.AsList().Add(value);
        }

        public static void AddValue(this UdonJsonValue v, string value)
        {
            v.AddValue(UdonJsonValue.NewString(value));
        }

        public static void AddValue(this UdonJsonValue v, double value)
        {
            v.AddValue(UdonJsonValue.NewNumber(value));
        }

        public static void AddValue(this UdonJsonValue v, bool value)
        {
            if (value)
            {
                v.AddValue(UdonJsonValue.NewTrue());
            }
            else
            {
                v.AddValue(UdonJsonValue.NewFalse());
            }
        }

        public static void AddNullValue(this UdonJsonValue v)
        {
            v.AddValue(UdonJsonValue.NewNull());
        }

        private static UdonDictionary<TKey, TValue> AsDictionary<TKey, TValue>(this UdonJsonValue v)
        {
            return (UdonDictionary<TKey, TValue>)v.GetValueUnchecked();
        }

        private static UdonList<object> AsList(this UdonJsonValue v)
        {

            return (UdonList<object>)v.GetValueUnchecked();
        }

        private static object GetValueUnchecked(this UdonJsonValue v)
        {
            return (UdonJsonValueKind)(v.AsRawArray()[1]);
        }

        private static object[] AsRawArray(this UdonJsonValue v)
        {
            return (object[])(object)v;
        }
    }
}
