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

            Assert.True(UdonJsonDeserializer.TryDeserialize("\"str\"", out output, out error));
            Assert.Equal(new object[] { UdonJsonValueKind.String, "str" }, output);
            Assert.Equal(null, error);

            Assert.True(UdonJsonDeserializer.TryDeserialize("123", out output, out error));
            Assert.Equal(new object[] { UdonJsonValueKind.Number, 123d }, output);
            Assert.Equal(null, error);

            Assert.True(UdonJsonDeserializer.TryDeserialize("-123", out output, out error));
            Assert.Equal(new object[] { UdonJsonValueKind.Number, -123d }, output);
            Assert.Equal(null, error);

            Assert.True(UdonJsonDeserializer.TryDeserialize("0.123", out output, out error));
            Assert.Equal(new object[] { UdonJsonValueKind.Number, 0.123d }, output);
            Assert.Equal(null, error);

            Assert.True(UdonJsonDeserializer.TryDeserialize("-0.123", out output, out error));
            Assert.Equal(new object[] { UdonJsonValueKind.Number, -0.123d }, output);
            Assert.Equal(null, error);

            Assert.True(UdonJsonDeserializer.TryDeserialize("123.456", out output, out error));
            Assert.Equal(new object[] { UdonJsonValueKind.Number, 123.456d }, output);
            Assert.Equal(null, error);

            Assert.True(UdonJsonDeserializer.TryDeserialize("-123.456", out output, out error));
            Assert.Equal(new object[] { UdonJsonValueKind.Number, -123.456d }, output);
            Assert.Equal(null, error);

            Assert.True(UdonJsonDeserializer.TryDeserialize("-123.456e12", out output, out error));
            Assert.Equal(new object[] { UdonJsonValueKind.Number, -123.456e12d }, output);
            Assert.Equal(null, error);

            Assert.True(UdonJsonDeserializer.TryDeserialize("-123.456e-12", out output, out error));
            Assert.Equal(new object[] { UdonJsonValueKind.Number, -123.456e-12d }, output);
            Assert.Equal(null, error);

            Assert.True(UdonJsonDeserializer.TryDeserialize("-123.456e+12", out output, out error));
            Assert.Equal(new object[] { UdonJsonValueKind.Number, -123.456e+12d }, output);
            Assert.Equal(null, error);

            Assert.True(UdonJsonDeserializer.TryDeserialize("{}", out output));
            Assert.Equal(new object[] { UdonJsonValueKind.Object, UdonDictionary.New() }, output);
            Assert.Equal(null, error);

            Assert.True(UdonJsonDeserializer.TryDeserialize("[]", out output, out error));
            Assert.Equal(new object[] { UdonJsonValueKind.Array, UdonObjectList.New() }, output);
            Assert.Equal(null, error);

            Assert.True(UdonJsonDeserializer.TryDeserialize("true", out output, out error));
            Assert.Equal(new object[] { UdonJsonValueKind.True, true }, output);
            Assert.Equal(null, error);

            Assert.True(UdonJsonDeserializer.TryDeserialize("false", out output, out error));
            Assert.Equal(new object[] { UdonJsonValueKind.False, false }, output);
            Assert.Equal(null, error);

            Assert.True(UdonJsonDeserializer.TryDeserialize("null", out output, out error));
            Assert.Equal(new object[] { UdonJsonValueKind.Null, null }, output);
            Assert.Equal(null, error);

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
            }", out output, out error));
            var dic = UdonDictionary.New();
            dic.SetValue("first", UdonJsonValue.NewString("str"));
            dic.SetValue("second", UdonJsonValue.NewNumber(123));
            var dicThird = UdonDictionary.New();
            dicThird.SetValue("thirdA", UdonJsonValue.NewString("str"));
            dicThird.SetValue("thirdB", UdonJsonValue.NewNumber(123));
            dicThird.SetValue("thirdC", UdonJsonValue.NewObject());
            dicThird.SetValue("thirdD", UdonJsonValue.NewArray());
            dicThird.SetValue("thirdE", UdonJsonValue.NewTrue());
            dicThird.SetValue("thirdF", UdonJsonValue.NewFalse());
            dicThird.SetValue("thirdG", UdonJsonValue.NewNull());
            dic.SetValue("third", UdonJsonValue.NewObject(dicThird));
            var listFourth = UdonObjectList.New();
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
            Assert.Equal(new object[] { UdonJsonValueKind.Object, dic }, output);
            Assert.Equal(null, error);
        }
    }
}
