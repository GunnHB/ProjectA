using System.IO;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using ExcelDataReader;

public class JsonUtil
{
    private const string JSON_PATH = "Assets/Tables/Json/";
    private const string MODEL_PATH = "Assets/02.Scripts/Model/";

    public static void CreateJsonFile(string assetPath, string assetName)
    {
        string jsonFile = $"{JSON_PATH}{assetName}.json";

        var fileStream = new FileStream(assetPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

        var reader = ExcelReaderFactory.CreateReader(fileStream);
        var dataTable = reader.AsDataSet().Tables[0];   // 항상 첫번째 시트의 데이터를 사용함둥

        // 저장될 데이터 딕셔너리
        Dictionary<string, List<object>> jsonData = new();

        for (int colIndex = 0; colIndex < dataTable.Columns.Count; colIndex++)
        {
            string varName = string.Empty;
            string varType = string.Empty;

            List<object> dataList = new();

            for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
            {
                var row = dataTable.Rows[rowIndex];
                var data = row.ItemArray[colIndex];

                // 변수명
                if (rowIndex == 0)
                    varName = data.ToString();
                // 변수 타입
                else if (rowIndex == 1)
                    varType = data.ToString();
                else
                {
                    if (varName != string.Empty && varType != string.Empty)
                    {
                        switch (varType)
                        {
                            case "int":
                            case "long":
                                dataList.Add(int.Parse(data.ToString()));
                                break;
                            case "float":
                            case "double":
                                dataList.Add(float.Parse(data.ToString()));
                                break;
                            case "string":
                                dataList.Add($"\"{data}\"");
                                break;
                            case "bool":
                            case "boolean":
                                {
                                    if (string.IsNullOrEmpty(data.ToString()))
                                        data = "false";

                                    dataList.Add(data.ToString());
                                }
                                break;
                                // enum도 추가해야쥐
                        }
                    }
                }

                if (dataList.Count != 0)
                {
                    if (jsonData.ContainsKey(varName))
                        jsonData[varName] = dataList;
                    else
                        jsonData.Add(varName, dataList);
                }
            }
        }

        File.WriteAllText(jsonFile, ToJson(jsonData));
        CreateModelScript(jsonFile, assetName);

        reader.Dispose();
        reader.Close();

        fileStream.Close();
    }

    public static string ToJson(Dictionary<string, List<object>> dataDic)
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("{");
        builder.Append("\n");
        builder.Append("\"").Append("Data").Append("\": [");
        builder.Append("\n");

        int lastIndex = dataDic.Values.First().Count;

        for (int index = 0; index < lastIndex; index++)
        {
            builder.Append("{");
            builder.Append("\n");

            foreach (var key in dataDic.Keys)
            {
                var item = dataDic[key][index];

                builder.Append("\"").Append(key).Append("\": ");
                builder.Append(item);

                if (dataDic.Keys.Last() != key)
                    builder.Append(",");

                builder.Append("\n");
            }

            if (index != lastIndex - 1)
                builder.Append("},");
            else
                builder.Append("}");

            builder.Append("\n");
        }

        builder.Append("]");
        builder.Append("\n");
        builder.Append("}");

        return builder.ToString();
    }

    public static void CreateModelScript(string jsonFile, string assetName)
    {
        string scriptContent = ScriptContent(jsonFile, assetName);

        File.WriteAllText($"{MODEL_PATH}{assetName}Model.cs", scriptContent);
    }

    public static string ScriptContent(string jsonFile, string assetName)
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("using UnityEngine;").Append("\n").Append("\n");

        builder.Append($"public class {assetName}Model : MonoBehaviour ");
        builder.Append("{").Append("\n");
        // 내부 구현해야 하잖스으음~~
        builder.Append("\n").Append("}");

        return builder.ToString();
    }
}
