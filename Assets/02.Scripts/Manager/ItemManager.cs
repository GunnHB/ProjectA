using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemManager : SingletonObject<ItemManager>
{
    private const string INVENTORY_DATA_PATH = "Assets/08.Tables/Json/InventoryData.json";

    private CategoryTab _currentCategoryTab;
    private ItemSlot _currentItemSlot;

    public CategoryTab CurrentCategoryTab => _currentCategoryTab;
    public ItemSlot CurrentItemSlot => _currentItemSlot;

    private InventoryData _inventoryData = null;
    public InventoryData ThisInventoryData => _inventoryData;

    public UnityAction<ModelCategoryTab.Data> TabAction;
    public UnityAction<ModelItem.Data> SlotAction;

    public UnityAction<ItemSlot> GoToSlotAction;

    private ItemMenu _itemMenu;
    public ItemMenu ThisItemMenu => _itemMenu;

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

    private void LoadInventory()
    {
        Debug.Log("이쓰요");
    }

    private void CreateInventory()
    {
        Debug.Log("업쓰요");

        _inventoryData = new InventoryData();

        // test
        AddItem(ModelItem.Model.DataList[0]);
        AddItem(ModelItem.Model.DataList[1]);
    }

    public void SetCurrentItemSlot(ItemSlot newSlot)
    {
        _currentItemSlot = newSlot;
    }

    public void ChangeCurrentItemSlot(ItemSlot newSlot)
    {
        if (_currentItemSlot != null)
        {
            if (CurrentItemSlot == newSlot)
                return;

            _currentItemSlot.SetSelect(false);
        }

        SetCurrentItemSlot(newSlot);

        _currentItemSlot.SetSelect(true);

        SlotAction?.Invoke(_currentItemSlot.ItemData);
    }

    public void SetCurrentCategoryTab(CategoryTab newTab)
    {
        _currentCategoryTab = newTab;
    }

    public void ChangeCurrentCategoryTab(CategoryTab newTab)
    {
        if (newTab == _currentCategoryTab)
            return;

        _currentCategoryTab.SetSelect(false);

        SetCurrentCategoryTab(newTab);

        _currentCategoryTab.SetSelect(true);
    }

    public void AddItem(ModelItem.Data itemData)
    {
        if (_inventoryData == null)
            return;

        for (int index = 0; index < _inventoryData._inventoryDic[itemData.type].Count; index++)
        {
            var slotData = _inventoryData._inventoryDic[itemData.type][index];

            if (slotData.id == 0)
            {
                _inventoryData._inventoryDic[itemData.type][index] = itemData;
                break;
            }
            else
                continue;
        }

        if (_inventoryData._itemAmount.ContainsKey(itemData))
            _inventoryData._itemAmount[itemData] += 1;
        else
            _inventoryData._itemAmount.Add(itemData, 1);
    }

    public void SetItemMenu(ItemMenu newMenu)
    {
        _itemMenu = newMenu;
    }
}
