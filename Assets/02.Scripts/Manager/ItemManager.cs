using System.Collections;
using System.Collections.Generic;
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

    public UnityAction<ModelCategoryTab.Data> TabAction;
    public UnityAction<ModelItem.Data> SlotAction;

    public UnityAction<ItemSlot> GoToSlotAction;

    private ItemMenu _itemMenu;
    public ItemMenu ThisItemMenu => _itemMenu;

    // private GameObject _rightHolder;
    // private GameObject _renderRightHolder;

    // public GameObject RightHolder
    // {
    //     get
    //     {
    //         if (_rightHolder == null)
    //         {
    //             var player = GameObject.Find("Player");

    //             if (player == null)
    //                 return null;

    //             foreach (var tran in player.GetComponentsInChildren<Transform>())
    //             {
    //                 if (tran.name == RIGHT_HOLDER)
    //                 {
    //                     _rightHolder = tran.gameObject;
    //                     break;
    //                 }
    //             }
    //         }

    //         return _rightHolder;
    //     }
    // }

    // public GameObject RenderRightHolder
    // {
    //     get
    //     {
    //         if (_renderRightHolder == null)
    //         {
    //             var player = GameObject.Find("RenderTexturePlayer");

    //             if (player == null)
    //                 return null;

    //             foreach (var tran in player.GetComponentsInChildren<Transform>())
    //             {
    //                 if (tran.name == RIGHT_HOLDER)
    //                 {
    //                     _renderRightHolder = tran.gameObject;
    //                     break;
    //                 }
    //             }
    //         }

    //         return _renderRightHolder;
    //     }
    // }

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
        AddItem(ModelItem.Model.DataList[2]);
        AddItem(ModelItem.Model.DataList[3]);
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
