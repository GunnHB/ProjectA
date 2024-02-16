using System;
using System.IO;

using System.Collections.Generic;

using UnityEngine;

using UnityEditor;

using ExcelDataReader;
using System.Text;
using System.Linq;

public class ImportProcessor : AssetPostprocessor
{
    private const string EXCEL_PATH = "Assets/Tables/Excel/";
    private const string JSON_PATH = "Assets/Tables/Json/";

    private static Dictionary<string, DateTime> _previousExcelWriteTime = new();

    private static string _targetAsset;
    private static string _targetAssetName;

    private static string _targetSheetName;

    public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        ImportExcel(importedAssets);
        DeleteExcel(deletedAssets);
    }

    #region About excels
    private static void ImportExcel(string[] importedAssets)
    {
        foreach (string assetPath in importedAssets)
        {
            if (assetPath.Contains(EXCEL_PATH) && assetPath.EndsWith(".xlsx"))
            {
                _targetAsset = assetPath;
                _targetAssetName = Path.GetFileName(_targetAsset).Replace(".xlsx", string.Empty);

                CheckExcels();
            }
        }
    }

    private static void DeleteExcel(string[] deletedAssets)
    {
        foreach (string assetPath in deletedAssets)
        {
            if (assetPath.Contains(EXCEL_PATH) && assetPath.EndsWith(".xlsx"))
                _previousExcelWriteTime.Remove(assetPath);
        }
    }

    private static void CheckExcels()
    {
        var lastWriteTime = File.GetLastWriteTime(_targetAsset);

        // 이전 파일의 마지막 수정 기록이 있음
        if (_previousExcelWriteTime.ContainsKey(_targetAsset))
        {
            // 기존 파일이 수정됨
            if (_previousExcelWriteTime[_targetAsset] != lastWriteTime)
                CreateJsonFile();
        }
        else
        {
            _previousExcelWriteTime.Add(_targetAsset, File.GetLastWriteTime(_targetAsset));

            // json 파일 생성
            CreateJsonFile();
        }
    }

    private static void CreateJsonFile()
    {
        string jsonFile = $"{JSON_PATH}{_targetAssetName}.json";

        var fileStream = new FileStream(_targetAsset, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

        var reader = ExcelReaderFactory.CreateReader(fileStream);
        var dataTable = reader.AsDataSet().Tables[0];

        _targetSheetName = dataTable.TableName;

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
                                dataList.Add(bool.Parse(data.ToString()));
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

        var json = ConvertToJson(jsonData);

        File.WriteAllText(jsonFile, json);

        reader.Dispose();
        reader.Close();

        fileStream.Close();
    }

    private static string ConvertToJson(Dictionary<string, List<object>> dataDic)
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("{");
        builder.Append("\n");
        builder.Append("\"").Append(_targetSheetName).Append("\": [");
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
    #endregion
}
