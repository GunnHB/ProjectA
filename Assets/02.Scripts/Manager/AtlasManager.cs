using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.U2D;

public class AtlasManager : SingletonObject<AtlasManager>
{
    private SpriteAtlas _inventoryAtlas;
    public SpriteAtlas InventoryAtlas => _inventoryAtlas;

    protected override void Awake()
    {
        base.Awake();

        _inventoryAtlas = AssetBundleManager.Instance.GetAtlasBundle().LoadAsset<SpriteAtlas>("InventoryAtlas");
    }
}
