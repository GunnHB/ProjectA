using System.IO;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class AssetBundleManager : SingletonObject<AssetBundleManager>
{
    private const string BUNDLE_UI = "uibundle";
    private const string BUNDLE_ATLAS = "atlasbundle";

    private AssetBundle _uiBundle;
    private AssetBundle _atlasBundle;

    public AssetBundle UIBundle { get => GetAssetBundle(ref _uiBundle, BUNDLE_UI); }
    public AssetBundle AtlasBundle { get => GetAssetBundle(ref _atlasBundle, BUNDLE_ATLAS); }

    protected override void Awake()
    {
        base.Awake();
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
}
