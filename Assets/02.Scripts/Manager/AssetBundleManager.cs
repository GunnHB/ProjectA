using System.IO;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class AssetBundleManager : SingletonObject<AssetBundleManager>
{
    private const string BUNDLE_UI = "uibundle";
    private const string BUNDLE_ATLAS = "atlasbundle";
    private const string BUNDLE_MATERIAL = "materialbundle";

    private AssetBundle _uiBundle;
    private AssetBundle _atlasBundle;
    private AssetBundle _materialBundle;

    protected override void Awake()
    {
        base.Awake();

        GetUIBundle();
    }

    public AssetBundle GetUIBundle()
    {
        if (_uiBundle == null)
        {
            var matBundle = AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath, BUNDLE_MATERIAL));

            if (matBundle == null)
                return null;
            else
            {
                matBundle.completed += (AsyncOperation operation) =>
                {
                    _uiBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, BUNDLE_UI));
                };
            }
        }

        return _uiBundle;
    }

    public AssetBundle GetAtlasBundle()
    {
        if (_atlasBundle == null)
            GetAssetBundle(ref _atlasBundle, BUNDLE_ATLAS);

        return _atlasBundle;
    }

    public AssetBundle GetAssetBundle(ref AssetBundle bundle, string path)
    {
        if (bundle == null)
        {
            bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, path));

            if (bundle == null)
                Debug.Log("fail to load asset bundle!!!");
        }

        return bundle;
    }

    public AssetBundle GetAssetBundleByAsync(ref AssetBundle bundle1, ref AssetBundle bundle2, string path1, string path2)
    {
        if (bundle1 == null)
        {
            var bundle = AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath, path1));

            bundle.completed += (AsyncOperation operation) =>
            {

            };
        }

        return bundle1;
    }
}
