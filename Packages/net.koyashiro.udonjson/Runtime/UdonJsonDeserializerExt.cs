using UdonSharp;

namespace Koyashiro.UdonJson
{
    using Koyashiro.UdonList;
    using Koyashiro.UdonDictionary;

    public static class UdonJsonDeserializerExt
    {
        private const string ERROR_UNEXPECTED_END = "Unexpected end";
        private const string ERROR_UNEXPECTED_TOKEN = "Unexpected token";
        private const string ERROR_EXPECTED_BRACE = "Expected {";

        public static char[] GetInput(this UdonJsonDeserializer des)
        {
            return (char[])(((object[])(object)des)[0]);
        }

        public static int GetPos(this UdonJsonDeserializer des)
        {
            return (int)(((object[])(object)des)[1]);
        }

        private static void SetPos(this UdonJsonDeserializer des, int pos)
        {
            ((object[])(object)des)[1] = pos;
        }

        public static object GetOutput(this UdonJsonDeserializer des)
        {
            return (UdonJsonValue)(((object[])(object)des)[2]);
        }

        private static void SetOutput(this UdonJsonDeserializer des, object output)
        {
            ((object[])(object)des)[2] = output;
        }

        public static string GetError(this UdonJsonDeserializer des)
        {
            return (string)(((object[])(object)des)[3]);
        }

        private static void SetError(this UdonJsonDeserializer des, string error)
        {
            ((object[])(object)des)[3] = error;
        }

        [RecursiveMethod]
        public static bool TryDeserializeValue(this UdonJsonDeserializer des)
        {
            while (true)
            {
                if (des.IsEnd())
                {
                    des.SetError(ERROR_UNEXPECTED_END);
                    return false;
                }

                des.ConsumeWhitespace();

                if (des.Current() == '"')
                {
                    if (!des.TryDeserializeString())
                    {
                        return false;
                    }
                    var s = (string)des.GetOutput();

                    des.SetOutput(UdonJsonValue.NewString(s));
                    return true;
                }

                if (des.Current() == '-' || ('0' <= des.Current() && des.Current() <= '9'))
                {
                    if (!des.TryDeserializeNumber())
                    {
                        return false;
                    }
                    var n = (double)des.GetOutput();

                    des.SetOutput(UdonJsonValue.NewNumber(n));
                    return true;
                }

                if (des.Current() == '{')
                {
                    if (!des.TryDeserializeObject())
                    {
                        return false;
                    }
                    var dic = (UdonDictionary)des.GetOutput();

                    des.SetOutput(UdonJsonValue.NewObject(dic));
                    return true;
                }

                if (des.Current() == '[')
                {
                    if (!des.TryDeserializeArray())
                    {
                        return false;
                    }
                    var array = (UdonList)des.GetOutput();

                    des.SetOutput(UdonJsonValue.NewArray(array));
                    return true;
                }

                if (des.Current() == 't')
                {
                    if (!des.TryDeserializeTrue())
                    {
                        return false;
                    }

                    des.SetOutput(UdonJsonValue.NewTrue());
                    return true;
                }

                if (des.Current() == 'f')
                {
                    if (!des.TryDeserializeFalse())
                    {
                        return false;
                    }

                    des.SetOutput(UdonJsonValue.NewFalse());
                    return true;
                }

                if (des.Current() == 'n')
                {
                    if (!des.TryDeserializeNull())
                    {
                        return false;
                    }

                    des.SetOutput(UdonJsonValue.NewNull());
                    return true;
                }

                des.SetError(ERROR_UNEXPECTED_TOKEN);
                return false;
            }
        }

        private static bool TryDeserializeString(this UdonJsonDeserializer des)
        {
            // "
            des.Next();

            var start = des.GetPos();

            while (true)
            {
                if (des.IsEnd())
                {
                    des.SetError(ERROR_UNEXPECTED_END);
                    return false;
                }

                // "
                if (des.Current() == '"')
                {
                    break;
                }

                des.Next();
            }


            des.SetOutput(new string(des.GetInput(), start, des.GetPos() - start));
            // "
            des.Next();
            return true;
        }

        private static bool TryDeserializeNumber(this UdonJsonDeserializer des)
        {
            var start = des.GetPos();

            if (des.Current() == '-')
            {
                des.Next();
            }

            if (des.IsEnd())
            {
                des.SetError(ERROR_UNEXPECTED_END);
                return false;
            }

            if (!des.IsDigit())
            {
                des.SetError(ERROR_UNEXPECTED_TOKEN);
                return false;
            }

            if (des.IsZero())
            {
                des.Next();
            }
            else
            {
                des.Next();

                while (true)
                {
                    if (des.IsEnd() || !des.IsDigit())
                    {
                        break;
                    }

                    des.Next();
                }
            }

            // fraction
            if (!des.IsEnd() && des.Current() == '.')
            {
                des.Next();

                while (true)
                {
                    if (des.IsEnd() || !des.IsDigit())
                    {
                        break;
                    }

                    des.Next();
                }
            }

            // exponent
            if (!des.IsEnd() && (des.Current() == 'e' || des.Current() == 'E'))
            {
                des.Next();

                if (des.IsEnd())
                {
                    des.SetError(ERROR_UNEXPECTED_END);
                    return false;
                }

                if (des.Current() == '-' || des.Current() == '+')
                {
                    des.Next();

                    if (des.IsEnd())
                    {
                        des.SetError(ERROR_UNEXPECTED_END);
                        return false;
                    }
                }

                if (!des.IsDigit())
                {
                    des.SetError(ERROR_UNEXPECTED_TOKEN);
                    return false;
                }

                while (true)
                {
                    if (des.IsEnd() || !des.IsDigit())
                    {
                        break;
                    }

                    des.Next();
                }
            }

            if (!double.TryParse(new string(des.GetInput(), start, des.GetPos() - start), out var number))
            {
                des.SetError(ERROR_UNEXPECTED_TOKEN);
                return false;
            }

            des.SetOutput(number);
            return true;
        }

        [RecursiveMethod]
        private static bool TryDeserializeObject(this UdonJsonDeserializer des)
        {
            // {
            des.Next();

            var dic = UdonDictionary.New();

            while (true)
            {
                des.ConsumeWhitespace();

                if (des.IsEnd())
                {
                    des.SetError(ERROR_UNEXPECTED_END);
                    return false;
                }

                if (des.Current() == '}')
                {
                    break;
                }

                if (dic.Count() > 0)
                {
                    if (!des.TryConsume(','))
                    {
                        return false;
                    }
                }

                des.ConsumeWhitespace();

                if (!des.TryDeserializeString())
                {
                    return false;
                }
                var key = (string)des.GetOutput();

                if (!des.TryConsume(':'))
                {
                    return false;
                }

                if (!des.TryDeserializeValue())
                {
                    return false;
                }
                var value = (UdonJsonValue)des.GetOutput();

                dic.SetValue(key, value);
            }

            // }
            des.Next();

            des.SetOutput(dic);
            return true;
        }

        [RecursiveMethod]
        private static bool TryDeserializeArray(this UdonJsonDeserializer des)
        {
            // [
            des.Next();

            var list = UdonList.New();

            while (true)
            {
                if (des.IsEnd())
                {
                    des.SetError(ERROR_UNEXPECTED_END);
                    return false;
                }

                des.ConsumeWhitespace();

                if (des.Current() == ']')
                {
                    break;
                }

                if (list.Count() > 0)
                {
                    if (!des.TryConsume(','))
                    {
                        return false;
                    }
                }

                if (!des.TryDeserializeValue())
                {
                    return false;
                }
                var value = des.GetOutput();

                list.Add(value);
            }

            // ]
            des.Next();

            des.SetOutput(list);
            return true;
        }

        private static bool TryDeserializeTrue(this UdonJsonDeserializer des)
        {
            // t
            des.Next();

            // r, u, e
            foreach (var c in "rue")
            {
                if (des.IsEnd())
                {
                    des.SetError(ERROR_UNEXPECTED_END);
                    return false;
                }

                if (des.IsEnd() || des.Current() != c)
                {
                    des.SetError(ERROR_UNEXPECTED_TOKEN);
                    return false;
                }
                des.Next();
            }

            return true;
        }

        private static bool TryDeserializeFalse(this UdonJsonDeserializer des)
        {
            // f
            des.Next();

            // a, l, s, e
            foreach (var c in "alse")
            {
                if (des.IsEnd())
                {
                    des.SetError(ERROR_UNEXPECTED_END);
                    return false;
                }

                if (des.IsEnd() || des.Current() != c)
                {
                    des.SetError(ERROR_UNEXPECTED_TOKEN);
                    return false;
                }
                des.Next();
            }

            return true;
        }

        private static bool TryDeserializeNull(this UdonJsonDeserializer des)
        {
            // n
            des.Next();

            // u, l, l
            foreach (var c in "ull")
            {
                if (des.IsEnd())
                {
                    des.SetError(ERROR_UNEXPECTED_END);
                    return false;
                }

                if (des.IsEnd() || des.Current() != c)
                {
                    des.SetError(ERROR_UNEXPECTED_TOKEN);
                    return false;
                }
                des.Next();
            }

            return true;
        }

        private static bool IsEnd(this UdonJsonDeserializer des)
        {
            return des.GetInput().Length == des.GetPos();
        }

        private static bool IsWhitespace(this UdonJsonDeserializer des)
        {
            var c = des.GetInput()[des.GetPos()];
            return c == ' ' || c == '\n' || c == '\r' || c == '\t';
        }

        private static bool IsDigit(this UdonJsonDeserializer des)
        {
            var c = des.GetInput()[des.GetPos()];
            return '0' <= c && c <= '9';
        }

        private static bool IsZero(this UdonJsonDeserializer des)
        {
            var c = des.GetInput()[des.GetPos()];
            return c == '0';
        }

        private static char Current(this UdonJsonDeserializer des)
        {
            return des.GetInput()[des.GetPos()];
        }

        private static char Next(this UdonJsonDeserializer des)
        {
            var current = des.Current();
            des.SetPos(des.GetPos() + 1);
            return current;
        }

        private static void ConsumeWhitespace(this UdonJsonDeserializer des)
        {
            while (true)
            {
                if (des.IsEnd())
                {
                    return;
                }

                if (!des.IsWhitespace())
                {
                    return;
                }

                des.Next();
            }
        }

        private static bool TryConsume(this UdonJsonDeserializer des, char c)
        {
            des.ConsumeWhitespace();

            if (des.IsEnd())
            {
                des.SetError(ERROR_UNEXPECTED_END);
                return false;
            }

            if (des.Current() != c)
            {
                des.SetError(ERROR_UNEXPECTED_TOKEN);
                return false;
            }

            des.Next();
            return true;
        }
    }
}
