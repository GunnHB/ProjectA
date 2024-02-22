using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryData
{
    // 아이템 타입 / 타입에 따른 아이템 데이터 리스트
    public Dictionary<GameValue.ItemType, List<ModelItem.Data>> _inventoryDic = new();
    public int _gold;

    public InventoryData()
    {
        // 아이템 데이터
        _inventoryDic.Add(GameValue.ItemType.Weapon, ItemDataList(GameValue._initWeaponItemAmount));
        _inventoryDic.Add(GameValue.ItemType.Shield, ItemDataList(GameValue._initShieldItemAmount));
        _inventoryDic.Add(GameValue.ItemType.Bow, ItemDataList(GameValue._initBowItemAmount));
        _inventoryDic.Add(GameValue.ItemType.Armor, ItemDataList(GameValue._initArmorItemAmount));
        _inventoryDic.Add(GameValue.ItemType.Food, ItemDataList(GameValue._initFoodItemAmount));
        _inventoryDic.Add(GameValue.ItemType.Default, ItemDataList(GameValue._initDefaultItemAmount));

        // 골드 데이터
        _gold = 0;
    }

    // 공갈 데이터 리스트
    private List<ModelItem.Data> ItemDataList(int amount)
    {
        List<ModelItem.Data> list = new();

        for (int index = 0; index < amount; index++)
            list.Add(new ModelItem.Data());

        return list;
    }
}
