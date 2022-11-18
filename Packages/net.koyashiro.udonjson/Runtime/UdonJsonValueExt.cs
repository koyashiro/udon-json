namespace Koyashiro.UdonJson
{
    using Koyashiro.UdonList;
    using Koyashiro.UdonDictionary;

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

        public static UdonJsonValue GetValue(this UdonJsonValue v, string key)
        {
            return (UdonJsonValue)v.AsDictionary().GetValue(key);
        }

        public static UdonJsonValue GetValue(this UdonJsonValue v, int key)
        {
            return (UdonJsonValue)(v.AsList().GetItem(key));
        }

        public static void SetValue(this UdonJsonValue v, string key)
        {
            v.AsDictionary().GetValue(key);
        }

        public static void SetValue(this UdonJsonValue v, int key, UdonJsonValue value)
        {
            v.AsList().SetItem(key, value);
        }

        public static void AddValue(this UdonJsonValue v, UdonJsonValue value)
        {
            v.AsList().Add(value);
        }

        private static UdonDictionary AsDictionary(this UdonJsonValue v)
        {
            if (v.GetKind() != UdonJsonValueKind.Object)
            {
                UdonException.ThrowArgumentException(ERR_INVALID_KIND);
            }

            return (UdonDictionary)v.GetValueUnchecked();
        }

        private static UdonList AsList(this UdonJsonValue v)
        {
            if (v.GetKind() != UdonJsonValueKind.Array)
            {
                UdonException.ThrowArgumentException(ERR_INVALID_KIND);
            }

            return (UdonList)v.GetValueUnchecked();
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
