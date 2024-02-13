using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.U2D;

public class AtlasManager : SingletonObject<AtlasManager>
{
    private const string BUNDLE_ATLAS = "atlasbundle";

    private SpriteAtlas _inventoryAtlas;

    private AssetBundle _loadedAssetBundle;

    protected override void Awake()
    {
        base.Awake();
    }

    private void LoadFromFile()
    {
        _loadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, BUNDLE_ATLAS));

        if (_loadedAssetBundle == null)
            Debug.Log("fail to load asset bundle!!!");
    }
}
