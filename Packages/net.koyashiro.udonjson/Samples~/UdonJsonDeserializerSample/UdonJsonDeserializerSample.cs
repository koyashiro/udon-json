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
        Debug.Log(result); // True

        // JSON value kind
        Debug.Log(json.GetKind()); // 2
        Debug.Log(json.GetKind().ToKindString()); // Object

        // String
        var valueA = json.GetValue("keyA");
        Debug.Log(valueA.GetKind()); // 0
        Debug.Log(valueA.GetKind().ToKindString()); // Object
        Debug.Log(valueA.AsString()); // valueA

        // Number
        var valueB = json.GetValue("keyB");
        Debug.Log(valueB.GetKind()); // 1
        Debug.Log(valueB.GetKind().ToKindString()); // Number
        Debug.Log(valueB.AsNumber()); // 123

        // Object
        var valueF = json.GetValue("keyF");
        Debug.Log(valueF.GetKind()); // 2
        Debug.Log(valueF.GetKind().ToKindString()); // Object
        var valueFA = valueF.GetValue("keyFA");
        Debug.Log(valueFA.GetKind()); // 0
        Debug.Log(valueFA.GetKind().ToKindString()); // String
        Debug.Log(valueFA.AsString()); // valueFA

        // Array
        var valueG = json.GetValue("keyG");
        Debug.Log(valueG.GetKind()); // 3
        Debug.Log(valueG.GetKind().ToKindString()); // Array
        var valueG0 = valueG.GetValue(0);
        Debug.Log(valueG0.GetKind()); // 1
        Debug.Log(valueG0.GetKind().ToKindString()); // Number
        Debug.Log(valueG0.AsNumber()); // 0

        // True
        var valueC = json.GetValue("keyC");
        Debug.Log(valueC.GetKind()); // 4
        Debug.Log(valueC.GetKind().ToKindString()); // True
        Debug.Log(valueC.AsBool()); // True

        // False
        var valueD = json.GetValue("keyD");
        Debug.Log(valueD.GetKind()); // 5
        Debug.Log(valueD.GetKind().ToKindString()); // False
        Debug.Log(valueD.AsBool()); // false

        // Null
        var valueE = json.GetValue("keyE");
        Debug.Log(valueE.GetKind()); // 6
        Debug.Log(valueE.GetKind().ToKindString()); // Null
        Debug.Log(valueE.AsNull()); // Null
    }
}
