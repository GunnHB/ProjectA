using System.IO;
using System.Linq;
using System.Text;

using System.Collections.Generic;

using ExcelDataReader;
using System.Reflection;

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

        File.WriteAllText(jsonFile, Serialize(_jsonData));
        CreateModelScript(assetName);

        reader.Dispose();
        reader.Close();

        fileStream.Close();
    }

    public static string Serialize(Dictionary<string, List<object>> dataDic)
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

    public static void Deserialize<T>(string jsonData, List<T> list) where T : new()
    {
        list.Clear();

        var objectData = jsonData.Split("{");

        // 0, 1번은 의미없는 데이터
        for (int jsonIndex = 2; jsonIndex < objectData.Length; jsonIndex++)
        {
            var data = objectData[jsonIndex];

            var fieldData = data.Split(",");

            T genericData = new T();
            FieldInfo[] fieldInfos = genericData.GetType().GetFields();

            for (int dataIndex = 0; dataIndex < fieldData.Length; dataIndex++)
            {
                // 공백 없애기
                var item = fieldData[dataIndex].Trim();

                if (string.IsNullOrEmpty(item))
                    continue;

                var result = item.Replace("}", string.Empty).Replace("]", string.Empty);

                foreach (var field in fieldInfos)
                {
                    if (result.Contains(field.Name))
                    {
                        var fieldValue = result.Substring(result.IndexOf(":") + 1).Trim();
                        var fieldType = field.FieldType;

                        if (fieldType == typeof(System.Int32))
                            field.SetValue(genericData, int.Parse(fieldValue));
                        else if (fieldType == typeof(System.Int64))
                            field.SetValue(genericData, long.Parse(fieldValue));
                        else if (fieldType == typeof(System.Single))
                            field.SetValue(genericData, float.Parse(fieldValue));
                        else if (fieldType == typeof(System.Double))
                            field.SetValue(genericData, double.Parse(fieldValue));
                        else if (fieldType == typeof(System.Boolean))
                            field.SetValue(genericData, bool.Parse(fieldValue));
                        else if (fieldType == typeof(System.String))
                            field.SetValue(genericData, fieldValue.ToString().Replace("\"", string.Empty));

                        // 할당했으면 foreach문 나가기
                        break;
                    }
                }
            }

            list.Add(genericData);
        }
    }

    public static void CreateModelScript(string assetName)
    {
        string scriptContent = ScriptContent(assetName);

        File.WriteAllText($"{MODEL_PATH}Model{assetName}.cs", scriptContent);
    }

    public static string ScriptContent(string assetName)
    {
        StringBuilder builder = new StringBuilder();

        // for using
        builder.Append(GenerateUsing());

        // for class
        builder.Append($"public class Model{assetName}").Append("\n");
        builder.Append("{").Append("\n");

        // for fields
        builder.Append(GenerateField());

        // // for instance
        // builder.Append(GenerateInstance(assetName));

        // for collecitons
        builder.Append(GenerateCollections(assetName));

        // for methods
        builder.Append(GenerateMethod(assetName));

        builder.Append("\n").Append("}");

        return builder.ToString();
    }

    private static string GenerateUsing()
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("using System.IO;").Append("\n");
        builder.Append("using System.Collections.Generic;").Append("\n");
        builder.Append("\n");

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

        builder.Append("\n");

        return builder.ToString();
    }

    private static string GenerateInstance(string assetName)
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("\n");
        builder.Append("\t");
        builder.Append($"private static Model{assetName} _instance;").Append("\n");
        builder.Append("\t");
        builder.Append($"public static Model{assetName} Instance => _instance;").Append("\n");
        builder.Append("\n");

        return builder.ToString();
    }

    private static string GenerateCollections(string assetName)
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("\t");
        builder.Append($"private static List<Model{assetName}> modelList = new();").Append("\n");
        builder.Append("\t");
        builder.Append($"private static Dictionary<long, Model{assetName}> modelDic = new();").Append("\n");
        // builder.Append("\n");

        return builder.ToString();
    }

    private static string GenerateMethod(string assetName)
    {
        StringBuilder builder = new StringBuilder();

        builder.Append(InitializeMethod(assetName)).Append("\n");
        builder.Append(GetModelMethod(assetName));

        return builder.ToString();
    }

    private static string InitializeMethod(string assetName)
    {
        string mehtodString;

        mehtodString = $@"
    /// <summary>
    /// 초기화하기
    /// </summary>
    public static void Initialize()
    {{
        var jsonData = File.ReadAllText(""{JSON_PATH}{assetName}.json"");
        JsonUtil.Deserialize(jsonData, modelList);

        foreach(var item in modelList)
            modelDic.Add(item.id, item);
    }}";

        return mehtodString;
    }

    private static string GetModelMethod(string assetName)
    {
        string mehtodString;

        mehtodString = $@"
    /// <summary>
    /// 모델 리스트 가져오기
    /// </summary>
    public static List<Model{assetName}> GetModelList()
    {{
        return modelList;
    }}
    
    /// <summary>
    /// 모델 딕셔너리 가져오기
    /// </summary>
    public static Dictionary<long, Model{assetName}> GetDictionary()
    {{
        return modelDic;
    }}
    ";

        return mehtodString;
    }
}
