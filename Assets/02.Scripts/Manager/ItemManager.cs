using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ItemManager : SingletonObject<ItemManager>
{
    private const string INVENTORY_DATA_PATH = "Assets/08.Tables/Json/InventoryData.json";

    // private const string RIGHT_HOLDER = "RightHolder";
    private const string PLAYER = "Player";
    private const string PLAYER_RENDER_TEXTURE = "RenderTexturePlayer";

    private CategoryTab _currentCategoryTab;
    private ItemSlot _currentItemSlot;

    public CategoryTab CurrentCategoryTab => _currentCategoryTab;
    public ItemSlot CurrentItemSlot => _currentItemSlot;

    private InventoryData _inventoryData = null;
    public InventoryData ThisInventoryData => _inventoryData;

    private EquipmentData _equipmentData = null;
    public EquipmentData ThisEquipmentData => _equipmentData;

    public UnityAction<ModelCategoryTab.Data> TabAction;
    public UnityAction<InventoryItemData> SlotAction;
    public UnityAction<bool> EquipAction;

    // public UnityAction<ItemSlot> GoToSlotAction;

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

        // test
        {
            _inventoryData = new InventoryData();

            AddItem(new InventoryItemData(ModelItem.Model.DataList[0]));
            AddItem(new InventoryItemData(ModelItem.Model.DataList[1]));
            AddItem(new InventoryItemData(ModelItem.Model.DataList[2]));
            AddItem(new InventoryItemData(ModelItem.Model.DataList[3]));
            AddItem(new InventoryItemData(ModelItem.Model.DataList[0]));

            _equipmentData = new EquipmentData();
        }
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

        SlotAction?.Invoke(_currentItemSlot.InvenItemData);
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

    // public void AddItem(ModelItem.Data itemData)
    public void AddItem(InventoryItemData invenItemData)
    {
        if (_inventoryData == null)
            return;

        var existItem = _inventoryData._inventoryDic[invenItemData._itemData.type].Where(x => x._itemData == invenItemData._itemData).FirstOrDefault();

        if (existItem != null && existItem._itemData != null)
        {
            if (existItem._itemData.stackable)
            {
                existItem._amount += invenItemData._amount;
                return;
            }
        }

        for (int index = 0; index < _inventoryData._inventoryDic[invenItemData._itemData.type].Count; index++)
        {
            var slotData = _inventoryData._inventoryDic[invenItemData._itemData.type][index];

            // 빈 슬롯에 추가하기
            if (slotData._itemData == null || slotData._itemData.id == 0)
            {
                _inventoryData._inventoryDic[invenItemData._itemData.type][index] = invenItemData;
                break;
            }
        }
    }

    public void SetItemMenu(ItemMenu newMenu)
    {
        _itemMenu = newMenu;
    }

    public void ActiveEquipment(GameObject holder, string weaponString, bool active = true)
    {
        for (int index = 0; index < holder.transform.childCount; index++)
        {
            var item = holder.transform.GetChild(index);

            if (item.name == weaponString)
            {
                item.gameObject.SetActive(active);
                return;
            }
        }
    }

    public GameObject GetHolderObj(ModelItem.Data itemData, bool isPlayer)
    {
        string holderName = string.Empty;

        // 홀더 이름 찾기
        switch (itemData.type)
        {
            case GameValue.ItemType.Weapon:
                {
                    ModelWeapon.Data weaponData = ModelWeapon.Model.DataDic[itemData.ref_id];

                    if (weaponData == null)
                        return null;

                    holderName = weaponData.equip_holder;
                }
                break;
            case GameValue.ItemType.Shield:
                {
                    ModelShield.Data shieldData = ModelShield.Model.DataDic[itemData.ref_id];

                    if (shieldData == null)
                        return null;

                    holderName = shieldData.equip_holder;
                }
                break;
        }

        if (holderName == string.Empty)
            return null;

        // 렌더 텍스쳐에 보이는 플레이어인지 실제 조종하는 플레이어인지
        GameObject playerObj = GameObject.Find(isPlayer ? PLAYER : PLAYER_RENDER_TEXTURE);

        if (playerObj == null)
            return null;

        // 홀더 이름 기반으로 오브젝트 찾기
        foreach (var obj in playerObj.GetComponentsInChildren<Transform>())
        {
            if (obj.name == holderName)
                return obj.gameObject;
        }

        return null;
    }
}
