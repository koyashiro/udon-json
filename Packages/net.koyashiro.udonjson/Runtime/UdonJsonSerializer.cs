using UdonSharp;

namespace Koyashiro.UdonJson
{
    using Koyashiro.UdonList;

    [UnityEngine.AddComponentMenu("")]
    public class UdonJsonSerializer : UdonSharpBehaviour
    {
        private static UdonJsonSerializer New(UdonJsonValue input)
        {
            var buf = UdonCharList.New();
            return (UdonJsonSerializer)(object)(new object[] { input, buf });
        }

        public static string Serialize(UdonJsonValue input)
        {
            var ser = UdonJsonSerializer.New(input);
            ser.Serialize();
            return ser.GetOutput();
        }
    }
}
