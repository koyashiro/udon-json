using UnityEngine;
using UdonSharp;
using Koyashiro.UdonJson;

public class UdonJsonDeserializerSample : UdonSharpBehaviour
{
    public void Start()
    {
        var jsonStr = @"{
            ""keyA"": ""valueA"",
            ""keyB"": 123,
            ""keyC"": true,
            ""keyD"": false,
            ""keyE"": null,
            ""keyF"": {
                ""keyFA"": ""valueFA"",
                ""keyFB"": ""valueFB"",
                ""keyFC"": ""valuFC""
            },
            ""keyG"": [0, 1, 2, 3]
        }";

        var result = UdonJsonDeserializer.TryDeserialize(jsonStr, out var json);

        // Deserialize result
        Debug.Log(result); // true

        // JSON value kind
        Debug.Log(json.GetKind()); // JsonValueKind.Object

        // String
        var valueA = json.GetValue("keyA");
        Debug.Log(valueA.GetKind()); // JsonValueKind.String
        Debug.Log(valueA.AsString()); // valueA

        // Number
        var valueB = json.GetValue("keyB");
        Debug.Log(valueB.GetKind()); // JsonValueKind.Number
        Debug.Log(valueB.AsNumber()); // 123

        // True
        var valueC = json.GetValue("keyC");
        Debug.Log(valueC.GetKind()); // JsonValueKind.True
        Debug.Log(valueC.AsBool()); // true

        // False
        var valueD = json.GetValue("keyD");
        Debug.Log(valueD.GetKind()); // JsonValueKind.False
        Debug.Log(valueD.AsBool()); // false

        // Null
        var valueE = json.GetValue("keyE");
        Debug.Log(valueE.GetKind()); // JsonValueKind.Null
        Debug.Log(valueE.AsNull()); // null

        // Object
        var valueF = json.GetValue("keyF");
        Debug.Log(valueF.GetKind()); // JsonValueKind.Object
        var valueFA = valueF.GetValue("keyFA");
        Debug.Log(valueFA.GetKind()); // JsonValueKind.String
        Debug.Log(valueFA.AsString()); // valueFA

        // Array
        var valueG = json.GetValue("keyG");
        Debug.Log(valueG.GetKind()); // JsonValueKind.Array
        var valueG0 = valueG.GetValue(0);
        Debug.Log(valueG0.GetKind()); // JsonValueKind.Number
        Debug.Log(valueG0.AsNumber()); // 0
    }
}
