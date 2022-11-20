using UdonSharp;
using Koyashiro.UdonTest;

namespace Koyashiro.UdonJson.Tests
{
    public class SerializeTest : UdonSharpBehaviour
    {
        public void Start()
        {
            UdonJsonValue json;

            json = UdonJsonValue.NewString("str");
            Assert.Equal(@"""str""", UdonJsonSerializer.Serialize(json));

            json = UdonJsonValue.NewNumber(123);
            Assert.Equal("123", UdonJsonSerializer.Serialize(json));

            json = UdonJsonValue.NewObject();
            Assert.Equal("{}", UdonJsonSerializer.Serialize(json));

            json = UdonJsonValue.NewObject();
            json.SetValue("keyA", UdonJsonValue.NewString("valueA"));
            Assert.Equal(@"{""keyA"":""valueA""}", UdonJsonSerializer.Serialize(json));

            json = UdonJsonValue.NewObject();
            json.SetValue("keyA", UdonJsonValue.NewString("valueA"));
            json.SetValue("keyB", UdonJsonValue.NewNumber(123));
            Assert.Equal(@"{""keyA"":""valueA"",""keyB"":123}", UdonJsonSerializer.Serialize(json));

            json = UdonJsonValue.NewArray();
            Assert.Equal("[]", UdonJsonSerializer.Serialize(json));

            json = UdonJsonValue.NewArray();
            json.AddValue(UdonJsonValue.NewString("first"));
            Assert.Equal(@"[""first""]", UdonJsonSerializer.Serialize(json));

            json = UdonJsonValue.NewArray();
            json.AddValue(UdonJsonValue.NewString("first"));
            json.AddValue(UdonJsonValue.NewString("second"));
            Assert.Equal(@"[""first"",""second""]", UdonJsonSerializer.Serialize(json));

            json = UdonJsonValue.NewTrue();
            Assert.Equal("true", UdonJsonSerializer.Serialize(json));

            json = UdonJsonValue.NewFalse();
            Assert.Equal("false", UdonJsonSerializer.Serialize(json));

            json = UdonJsonValue.NewNull();
            Assert.Equal("null", UdonJsonSerializer.Serialize(json));
        }
    }
}
