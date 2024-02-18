using System.IO;
using System.Linq;
using System.Text;

using System.Collections.Generic;

using ExcelDataReader;

public class JsonUtil
{
    private const string JSON_PATH = "Assets/Tables/Json/";
    private const string MODEL_PATH = "Assets/02.Scripts/Model/";

    // 저장될 데이터 딕셔너리
    // <변수명, 데이터>
    private static Dictionary<string, List<object>> _jsonData = new();
    // 모델 필드 생성용
    // <변수명, 타입>
    private static Dictionary<string, object> _fieldTypeData = new();

    public static void CreateJsonFile(string assetPath, string assetName)
    {
        string jsonFile = $"{JSON_PATH}{assetName}.json";

        var fileStream = new FileStream(assetPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

        var reader = ExcelReaderFactory.CreateReader(fileStream);
        var dataTable = reader.AsDataSet().Tables[0];   // 항상 첫번째 시트의 데이터를 사용함둥

        // 저장될 데이터 딕셔너리
        // Dictionary<string, List<object>> jsonData = new();
        _jsonData.Clear();
        _fieldTypeData.Clear();

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
                {
                    varType = data.ToString();

                    switch (varType)
                    {
                        case "int":
                            _fieldTypeData.Add(varName, typeof(System.Int32));
                            break;
                        case "long":
                            _fieldTypeData.Add(varName, typeof(System.Int64));
                            break;
                        case "float":
                            _fieldTypeData.Add(varName, typeof(System.Single));
                            break;
                        case "double":
                            _fieldTypeData.Add(varName, typeof(System.Double));
                            break;
                        case "string":
                            _fieldTypeData.Add(varName, typeof(System.String));
                            break;
                        case "bool":
                        case "boolean":
                            _fieldTypeData.Add(varName, typeof(System.Boolean));
                            break;
                            // enum도 추가해야쥐
                    }
                }
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
                    if (_jsonData.ContainsKey(varName))
                        _jsonData[varName] = dataList;
                    else
                        _jsonData.Add(varName, dataList);
                }
            }
        }

        File.WriteAllText(jsonFile, ToJson(_jsonData));
        CreateModelScript(assetName);

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

    public static void CreateModelScript(string assetName)
    {
        string scriptContent = ScriptContent(assetName);

        File.WriteAllText($"{MODEL_PATH}{assetName}Model.cs", scriptContent);
    }

    public static string ScriptContent(string assetName)
    {
        StringBuilder builder = new StringBuilder();

        // for using
        builder.Append("using UnityEngine;").Append("\n").Append("\n");

        // for class
        builder.Append($"public class {assetName}Model : MonoBehaviour ").Append("\n");
        builder.Append("{").Append("\n");

        // for fields
        builder.Append(GenerateField());

        // for methods
        builder.Append(GenerateMethod());

        builder.Append("\n").Append("}");

        return builder.ToString();
    }

    private static string GenerateField()
    {
        StringBuilder builder = new StringBuilder();

        if (_jsonData == null || _jsonData.Count == 0 ||
            _fieldTypeData == null || _fieldTypeData.Count == 0)
            return string.Empty;

        foreach (var typeKey in _fieldTypeData.Keys)
        {
            builder.Append("\t");
            builder.Append($"public {_fieldTypeData[typeKey]} {typeKey};");
            builder.Append("\n");
        }

        return builder.ToString();
    }

    private static string GenerateMethod()
    {
        StringBuilder builder = new StringBuilder();

        builder.Append(InitializeMethod()).Append("\n");
        builder.Append(GetModelMethod());

        return builder.ToString();
    }

    private static string InitializeMethod()
    {
        string mehtodString;

        mehtodString = @"
    /// <summary>
    /// 초기화하기
    /// </summary>
    public void Initialize()
    {

    }";

        return mehtodString;
    }

    private static string GetModelMethod()
    {
        string mehtodString;

        mehtodString = @"
    /// <summary>
    /// 모델 데이터 가져오기
    /// </summary>    
    public void GetModel()
    {
        
    }";

        return mehtodString;
    }
}
