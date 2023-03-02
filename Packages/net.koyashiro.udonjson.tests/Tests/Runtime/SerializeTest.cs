using UnityEngine;
using UdonSharp;
using Koyashiro.UdonTest;

namespace Koyashiro.UdonJson.Tests
{
    [AddComponentMenu("")]
    public class SerializeTest : UdonSharpBehaviour
    {
        public void Start()
        {
            UdonJsonValue json;

            json = UdonJsonValue.NewString("str");
            Assert.Equal("\"str\"", UdonJsonSerializer.Serialize(json), this);

            json = UdonJsonValue.NewString("\"\\/\b\f\n\r\t");
            Assert.Equal("\"\\\"\\\\/\\b\\f\\n\\r\\t\"", UdonJsonSerializer.Serialize(json), this);

            json = UdonJsonValue.NewString("\0");
            Assert.Equal("\"\\u0000\"", UdonJsonSerializer.Serialize(json), this);

            json = UdonJsonValue.NewString("aaa\naaa\0aaa");
            Assert.Equal("\"aaa\\naaa\\u0000aaa\"", UdonJsonSerializer.Serialize(json), this);

            json = UdonJsonValue.NewNumber(123);
            Assert.Equal("123", UdonJsonSerializer.Serialize(json), this);

            json = UdonJsonValue.NewObject();
            Assert.Equal("{}", UdonJsonSerializer.Serialize(json), this);

            json = UdonJsonValue.NewObject();
            json.SetValue("keyA", UdonJsonValue.NewString("valueA"));
            Assert.Equal(@"{""keyA"":""valueA""}", UdonJsonSerializer.Serialize(json), this);

            json = UdonJsonValue.NewObject();
            json.SetValue("keyA", UdonJsonValue.NewString("valueA"));
            json.SetValue("keyB", UdonJsonValue.NewNumber(123));
            Assert.Equal(@"{""keyA"":""valueA"",""keyB"":123}", UdonJsonSerializer.Serialize(json), this);

            json = UdonJsonValue.NewArray();
            Assert.Equal("[]", UdonJsonSerializer.Serialize(json), this);

            json = UdonJsonValue.NewArray();
            json.AddValue(UdonJsonValue.NewString("first"));
            Assert.Equal(@"[""first""]", UdonJsonSerializer.Serialize(json), this);

            json = UdonJsonValue.NewArray();
            json.AddValue(UdonJsonValue.NewString("first"));
            json.AddValue(UdonJsonValue.NewString("second"));
            Assert.Equal(@"[""first"",""second""]", UdonJsonSerializer.Serialize(json), this);

            json = UdonJsonValue.NewTrue();
            Assert.Equal("true", UdonJsonSerializer.Serialize(json), this);

            json = UdonJsonValue.NewFalse();
            Assert.Equal("false", UdonJsonSerializer.Serialize(json), this);

            json = UdonJsonValue.NewNull();
            Assert.Equal("null", UdonJsonSerializer.Serialize(json), this);
        }
    }
}
