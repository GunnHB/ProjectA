using System.Collections.Generic;

public class InventoryItemData
{
    public ModelItem.Data _itemData;
    public int _amount;
    public bool _isEquip;

    public InventoryItemData()
    {
        ClearData();
    }

    public InventoryItemData(ModelItem.Data itemData)
    {
        _itemData = itemData;
        _amount = 1;
        _isEquip = false;
    }

    public void ClearData()
    {
        _itemData = new ModelItem.Data();
        _amount = 0;
        _isEquip = false;
    }

    public bool IsEmpty()
    {
        return _itemData == null || _itemData.id == 0;
    }
}

public class InventoryData
{
    // 아이템 타입 / 타입에 따른 아이템 데이터 리스트
    public Dictionary<GameValue.ItemType, List<InventoryItemData>> _inventoryDic = new();
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
    private List<InventoryItemData> ItemDataList(int amount)
    {
        var list = new List<InventoryItemData>();

        for (int index = 0; index < amount; index++)
            list.Add(new InventoryItemData());

        return list;
    }
}
