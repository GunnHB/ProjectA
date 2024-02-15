using System.IO;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

public class ImportProcessor : AssetPostprocessor
{
    public void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        Debug.Log("YAAAAA");
    }
}
