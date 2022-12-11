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
            dic.Set("first", UdonJsonValue.NewString("str"));
            dic.Set("second", UdonJsonValue.NewNumber(123));
            var dicThird = UdonDictionary<string, object>.New();
            dicThird.Set("thirdA", UdonJsonValue.NewString("str"));
            dicThird.Set("thirdB", UdonJsonValue.NewNumber(123));
            dicThird.Set("thirdC", UdonJsonValue.NewObject());
            dicThird.Set("thirdD", UdonJsonValue.NewArray());
            dicThird.Set("thirdE", UdonJsonValue.NewTrue());
            dicThird.Set("thirdF", UdonJsonValue.NewFalse());
            dicThird.Set("thirdG", UdonJsonValue.NewNull());
            dic.Set("third", UdonJsonValue.NewObject(dicThird));
            var listFourth = UdonList<object>.New();
            listFourth.Add(UdonJsonValue.NewString("str"));
            listFourth.Add(UdonJsonValue.NewNumber(123));
            listFourth.Add(UdonJsonValue.NewObject());
            listFourth.Add(UdonJsonValue.NewArray());
            listFourth.Add(UdonJsonValue.NewTrue());
            listFourth.Add(UdonJsonValue.NewFalse());
            listFourth.Add(UdonJsonValue.NewNull());
            dic.Set("fourth", UdonJsonValue.NewArray(listFourth));
            dic.Set("fifth", UdonJsonValue.NewTrue());
            dic.Set("sixth", UdonJsonValue.NewFalse());
            dic.Set("seventh", UdonJsonValue.NewNull());
            Assert.Equal(new object[] { UdonJsonValueKind.Object, dic }, output, this);
            Assert.Equal(null, error, this);
        }
    }
}
