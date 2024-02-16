using System;
using System.IO;

using System.Collections.Generic;

using UnityEngine;

using UnityEditor;

using ExcelDataReader;

public class ImportProcessor : AssetPostprocessor
{
    private static Dictionary<string, DateTime> _previousExcelWriteTime = new();
    private static Dictionary<string, DateTime> _preivousJsonWriteTime = new();
    private static string _targetAsset;

    public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string assetPath in importedAssets)
        {
            if (assetPath.Contains("Assets/Tables/Excel") && assetPath.EndsWith(".xlsx"))
            {
                _targetAsset = assetPath;

                CheckExcels();
            }
        }
    }

    #region About excels
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
        ReadExcel();
    }

    private static void ReadExcel()
    {
        var fileStream = new FileStream(_targetAsset, FileMode.Open, FileAccess.Read, FileShare.Read);

        var reader = ExcelReaderFactory.CreateReader(fileStream);
        var dataTable = reader.AsDataSet().Tables[0];

        // 1행 => 변수명 / 2행 => 타입
        for (int rowIndex = 2; rowIndex < dataTable.Rows.Count; rowIndex++)
        {
            // // 행
            // var row = dataTable.Rows[rowIndex];

            // for (int colIndex = 0; colIndex < row.ItemArray.Length; colIndex++)
            // {
            //     // 열
            //     var item = row.ItemArray[colIndex];
            // }
        }

        reader.Dispose();
        reader.Close();

        fileStream.Close();
    }
    #endregion
}
