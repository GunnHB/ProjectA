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

    public Sprite GetSpriteByInventory(string spriteName)
    {
        if (_inventoryAtlas == null)
        {
            Debug.Log("아틀라스 확인하세요~!");
            return null;
        }

        Sprite resultSprite = _inventoryAtlas.GetSprite(spriteName);

        if (resultSprite == null)
            Debug.Log("스프라이트가 없습니다.");

        return resultSprite;
    }
}
