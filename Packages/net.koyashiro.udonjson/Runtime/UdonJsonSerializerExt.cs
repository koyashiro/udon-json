using UdonSharp;

namespace Koyashiro.UdonJson
{
    using Koyashiro.UdonList;

    static public class UdonJsonSerializerExt
    {
        public static void Serialize(this UdonJsonSerializer ser)
        {
            var input = ser.GetInput();
            ser.Write(input);
        }

        public static string GetOutput(this UdonJsonSerializer ser)
        {
            var chars = new char[ser.GetBuf().Count()];
            for (var i = 0; i < chars.Length; i++)
            {
                chars[i] = (char)ser.GetBuf().GetItem(i);
            }
            return new string(chars);
        }

        private static object[] AsArray(this UdonJsonSerializer ser)
        {
            return (object[])(object)ser;
        }

        private static UdonJsonValue GetInput(this UdonJsonSerializer ser)
        {
            return (UdonJsonValue)ser.AsArray()[0];
        }

        private static UdonList GetBuf(this UdonJsonSerializer ser)
        {
            return (UdonList)ser.AsArray()[1];
        }

        [RecursiveMethod]
        private static void Write(this UdonJsonSerializer ser, UdonJsonValue v)
        {
            switch (v.GetKind())
            {
                case UdonJsonValueKind.String:
                    ser.Write('"');
                    ser.Write(v.AsString());
                    ser.Write('"');
                    break;
                case UdonJsonValueKind.Number:
                    ser.Write(v.AsNumber().ToString());
                    break;
                case UdonJsonValueKind.Object:
                    ser.Write('{');
                    for (var i = 0; i < v.Count(); i++)
                    {
                        var key = v.GetKey(i);
                        ser.Write('"');
                        ser.Write(key);
                        ser.Write('"');
                        ser.Write(':');

                        var value = v.GetValue(key);
                        ser.Write(value);

                        if (i != v.Count() - 1)
                        {
                            ser.Write(',');
                        }
                    }
                    ser.Write('}');
                    break;
                case UdonJsonValueKind.Array:
                    ser.Write('[');
                    for (var i = 0; i < v.Count(); i++)
                    {
                        var value = v.GetValue(i);
                        ser.Write(value);

                        if (i != v.Count() - 1)
                        {
                            ser.Write(',');
                        }
                    }
                    ser.Write(']');
                    break;
                case UdonJsonValueKind.True:
                    ser.Write("true");
                    break;
                case UdonJsonValueKind.False:
                    ser.Write("false");
                    break;
                case UdonJsonValueKind.Null:
                    ser.Write("null");
                    break;
            }
        }

        private static void Write(this UdonJsonSerializer ser, char c)
        {
            ser.GetBuf().Add(c);
        }

        private static void Write(this UdonJsonSerializer ser, string s)
        {
            for (var i = 0; i < s.Length; i++)
            {
                ser.GetBuf().Add(s[i]);
            }
        }
    }
}
