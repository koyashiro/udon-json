using UnityEngine;
using UdonSharp;
using Koyashiro.UdonTest;

namespace Koyashiro.UdonJson.Tests
{
    using Koyashiro.UdonList;
    using Koyashiro.UdonDictionary;

    [AddComponentMenu("")]
    public class DeserializeTest : UdonSharpBehaviour
    {
        public void Start()
        {
            UdonJsonValue output;
            string error;

            Assert.True(UdonJsonDeserializer.TryDeserialize("\"str\"", out output, out error), this);
            Assert.Equal(new object[] { UdonJsonValueKind.String, "str" }, output, this);
            Assert.Equal(null, error, this);

            Assert.True(UdonJsonDeserializer.TryDeserialize("\"\\\"\\\\\\/\\b\\f\\n\\r\\t\"", out output, out error), this);
            Assert.Equal(new object[] { UdonJsonValueKind.String, "\"\\/\b\f\n\r\t" }, output, this);
            Assert.Equal(null, error, this);

            Assert.True(UdonJsonDeserializer.TryDeserialize("\"\\ud83c\\udf63\"", out output, out error), this);
            Assert.Equal(new object[] { UdonJsonValueKind.String, "🍣" }, output, this);
            Assert.Equal(null, error, this);

            Assert.True(UdonJsonDeserializer.TryDeserialize("\"aaa\\naaa\\ud83c\\udf63aaa\"", out output, out error), this);
            Assert.Equal(new object[] { UdonJsonValueKind.String, "aaa\naaa🍣aaa" }, output, this);
            Assert.Equal(null, error, this);

            Assert.False(UdonJsonDeserializer.TryDeserialize("\"\0\"", out output, out error), this);
            Assert.Equal(null, output, this);
            Assert.True(error.IndexOf("Bad control character in string literal") != -1, this);

            Assert.True(UdonJsonDeserializer.TryDeserialize("123", out output, out error), this);
            Assert.Equal(new object[] { UdonJsonValueKind.Number, 123d }, output, this);
            Assert.Equal(null, error, this);

            Assert.True(UdonJsonDeserializer.TryDeserialize("-123", out output, out error), this);
            Assert.Equal(new object[] { UdonJsonValueKind.Number, -123d }, output, this);
            Assert.Equal(null, error, this);

            Assert.True(UdonJsonDeserializer.TryDeserialize("0.123", out output, out error), this);
            Assert.Equal(new object[] { UdonJsonValueKind.Number, 0.123d }, output, this);
            Assert.Equal(null, error, this);

            Assert.True(UdonJsonDeserializer.TryDeserialize("-0.123", out output, out error), this);
            Assert.Equal(new object[] { UdonJsonValueKind.Number, -0.123d }, output, this);
            Assert.Equal(null, error, this);

            Assert.True(UdonJsonDeserializer.TryDeserialize("123.456", out output, out error), this);
            Assert.Equal(new object[] { UdonJsonValueKind.Number, 123.456d }, output, this);
            Assert.Equal(null, error, this);

            Assert.True(UdonJsonDeserializer.TryDeserialize("-123.456", out output, out error), this);
            Assert.Equal(new object[] { UdonJsonValueKind.Number, -123.456d }, output, this);
            Assert.Equal(null, error, this);

            Assert.True(UdonJsonDeserializer.TryDeserialize("-123.456e12", out output, out error), this);
            Assert.Equal(new object[] { UdonJsonValueKind.Number, -123.456e12d }, output, this);
            Assert.Equal(null, error, this);

            Assert.True(UdonJsonDeserializer.TryDeserialize("-123.456e-12", out output, out error), this);
            Assert.Equal(new object[] { UdonJsonValueKind.Number, -123.456e-12d }, output, this);
            Assert.Equal(null, error, this);

            Assert.True(UdonJsonDeserializer.TryDeserialize("-123.456e+12", out output, out error), this);
            Assert.Equal(new object[] { UdonJsonValueKind.Number, -123.456e+12d }, output, this);
            Assert.Equal(null, error, this);

            Assert.True(UdonJsonDeserializer.TryDeserialize("{}", out output), this);
            Assert.Equal(new object[] { UdonJsonValueKind.Object, UdonDictionary<string, object>.New() }, output, this);
            Assert.Equal(null, error, this);

            Assert.True(UdonJsonDeserializer.TryDeserialize("[]", out output, out error), this);
            Assert.Equal(new object[] { UdonJsonValueKind.Array, UdonList<object>.New() }, output, this);
            Assert.Equal(null, error, this);

            Assert.True(UdonJsonDeserializer.TryDeserialize("true", out output, out error), this);
            Assert.Equal(new object[] { UdonJsonValueKind.True, true }, output, this);
            Assert.Equal(null, error, this);

            Assert.True(UdonJsonDeserializer.TryDeserialize("false", out output, out error), this);
            Assert.Equal(new object[] { UdonJsonValueKind.False, false }, output, this);
            Assert.Equal(null, error, this);

            Assert.True(UdonJsonDeserializer.TryDeserialize("null", out output, out error), this);
            Assert.Equal(new object[] { UdonJsonValueKind.Null, null }, output, this);
            Assert.Equal(null, error, this);

            Assert.True(UdonJsonDeserializer.TryDeserialize(@"{
                ""first"": ""str"",
                ""second"": 123,
                ""third"": {
                    ""thirdA"": ""str"",
                    ""thirdB"": 123,
                    ""thirdC"": {},
                    ""thirdD"": [],
                    ""thirdE"": true,
                    ""thirdF"": false,
                    ""thirdG"": null
                },
                ""fourth"": [
                    ""str"",
                    123,
                    {},
                    [],
                    true,
                    false,
                    null
                ],
                ""fifth"": true,
                ""sixth"": false,
                ""seventh"": null
            }", out output, out error), this);
            var dic = UdonDictionary<string, object>.New();
            dic.SetValue("first", UdonJsonValue.NewString("str"));
            dic.SetValue("second", UdonJsonValue.NewNumber(123));
            var dicThird = UdonDictionary<string, object>.New();
            dicThird.SetValue("thirdA", UdonJsonValue.NewString("str"));
            dicThird.SetValue("thirdB", UdonJsonValue.NewNumber(123));
            dicThird.SetValue("thirdC", UdonJsonValue.NewObject());
            dicThird.SetValue("thirdD", UdonJsonValue.NewArray());
            dicThird.SetValue("thirdE", UdonJsonValue.NewTrue());
            dicThird.SetValue("thirdF", UdonJsonValue.NewFalse());
            dicThird.SetValue("thirdG", UdonJsonValue.NewNull());
            dic.SetValue("third", UdonJsonValue.NewObject(dicThird));
            var listFourth = UdonList<object>.New();
            listFourth.Add(UdonJsonValue.NewString("str"));
            listFourth.Add(UdonJsonValue.NewNumber(123));
            listFourth.Add(UdonJsonValue.NewObject());
            listFourth.Add(UdonJsonValue.NewArray());
            listFourth.Add(UdonJsonValue.NewTrue());
            listFourth.Add(UdonJsonValue.NewFalse());
            listFourth.Add(UdonJsonValue.NewNull());
            dic.SetValue("fourth", UdonJsonValue.NewArray(listFourth));
            dic.SetValue("fifth", UdonJsonValue.NewTrue());
            dic.SetValue("sixth", UdonJsonValue.NewFalse());
            dic.SetValue("seventh", UdonJsonValue.NewNull());
            Assert.Equal(new object[] { UdonJsonValueKind.Object, dic }, output, this);
            Assert.Equal(null, error, this);

            UdonJsonDeserializer.TryDeserialize("Infinity", out output, out error);
            Debug.LogWarning(output.AsNumber());
            Debug.LogWarning(error);
        }
    }
}
