using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : SingletonObject<ItemManager>
{
    private const string INVENTORY_DATA_PATH = "Assets/Tables/Json/InventoryData.json";

    private CategoryTab _currentCategoryTab;
    private ItemSlot _currentItemSlot;

    public CategoryTab CurrentCategoryTab => _currentCategoryTab;
    public ItemSlot CurrentItemSlot => _currentItemSlot;

    protected override void Awake()
    {
        base.Awake();

        InitInventoryData();
    }

    private void InitInventoryData()
    {
        if (JsonUtil.IsExist(INVENTORY_DATA_PATH))
            LoadInventory();
        else
            CreateInventory();
    }

    private static void LoadInventory()
    {
        Debug.Log("이쓰요");
    }

    private static void CreateInventory()
    {
        Debug.Log("업쓰요");

        var inventoryData = new InventoryData();
    }

    public void SetCurrentItemSlot(ItemSlot newSlot)
    {
        _currentItemSlot = newSlot;
    }

    public void ChangeCurrentItemSlot(ItemSlot newSlot)
    {
        _currentItemSlot.SetSelect(false);

        SetCurrentItemSlot(newSlot);

        _currentItemSlot.SetSelect(true);
    }

    public void SetCurrentCategoryTab(CategoryTab newTab)
    {
        _currentCategoryTab = newTab;
    }

    public void ChangeCurrentCategoryTab(CategoryTab newTab)
    {
        _currentCategoryTab.SetSelect(false);

        SetCurrentCategoryTab(newTab);

        _currentCategoryTab.SetSelect(true);
    }
}
