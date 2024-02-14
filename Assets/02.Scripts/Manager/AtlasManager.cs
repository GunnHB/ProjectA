using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.U2D;

public class AtlasManager : SingletonObject<AtlasManager>
{
    private SpriteAtlas _inventoryAtlas;

    protected override void Awake()
    {
        base.Awake();

        _inventoryAtlas = AssetBundleManager.Instance.AtlasBundle.LoadAsset<SpriteAtlas>("InventoryAtlas");
    }
}
