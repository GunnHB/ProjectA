using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ItemManager : SingletonObject<ItemManager>
{
    private const string INVENTORY_DATA_PATH = "Assets/08.Tables/Json/InventoryData.json";

    private const string LIST_OBJECT = "ItemObjects";

    // 현재 선택된 인벤토리 탭
    private CategoryTab _currentCategoryTab;
    public CategoryTab CurrentCategoryTab => _currentCategoryTab;

    // 현재 선택된 아이템 슬롯
    private ItemSlot _currentItemSlot;
    public ItemSlot CurrentItemSlot => _currentItemSlot;

    // 현재 인벤토리 데이터
    private InventoryData _inventoryData = null;
    public InventoryData ThisInventoryData => _inventoryData;

    // 인벤토리의 ui
    private Inventory _inventory = null;
    public Inventory ThisInventory => _inventory;

    // 현재 장착 데이터
    private EquipmentData _equipmentData = null;
    public EquipmentData ThisEquipmentData => _equipmentData;

    public UnityAction<ModelCategoryTab.Data> TabAction;
    public UnityAction<InventoryItemData> SlotAction;

    private ItemMenu _itemMenu;
    public ItemMenu ThisItemMenu => _itemMenu;

    private Dictionary<string, GameObject> _playerHolderDic = new();
    private Dictionary<string, GameObject> _renderHolderDic = new();

    private Dictionary<string, GameObject> _itemDic = new();
    public Dictionary<string, GameObject> ItemDic
    {
        get
        {
            if (_itemDic == null || _itemDic.Count == 0)
            {
                GameObject listObj = GameObject.Find(LIST_OBJECT);

                if (listObj != null)
                {
                    for (int index = 0; index < listObj.transform.childCount; index++)
                        _itemDic.Add(listObj.transform.GetChild(index).name, listObj.transform.GetChild(index).gameObject);
                }
            }

            return _itemDic;
        }
    }

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
        // test
        {
            _inventoryData = new InventoryData();

            AddItem(new InventoryItemData(ModelItem.Model.DataList[0]));
            AddItem(new InventoryItemData(ModelItem.Model.DataList[1]));
            AddItem(new InventoryItemData(ModelItem.Model.DataList[2]));
            AddItem(new InventoryItemData(ModelItem.Model.DataList[3]));
            AddItem(new InventoryItemData(ModelItem.Model.DataList[0]));

            for (int index = 0; index < 3; index++)
                AddItem(new InventoryItemData(ModelItem.Model.DataList[9]));

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

    public void AddItem(InventoryItemData invenItemData)
    {
        if (_inventoryData == null)
            return;

        // 인벤토리에 있는 아이템인지
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

    public void ActiveEquipment(InventoryItemData invenItemData, bool active)
    {
        var playerHolder = GetHolderObj(invenItemData._itemData, true);
        var renderHolder = GetHolderObj(invenItemData._itemData, false);

        ActualActiveEquipment(playerHolder, invenItemData._itemData.prefab, active);
        ActualActiveEquipment(renderHolder, invenItemData._itemData.prefab, active);
    }

    private void ActualActiveEquipment(GameObject holder, string equipmentName, bool active)
    {
        for (int index = 0; index < holder.transform.childCount; index++)
        {
            var item = holder.transform.GetChild(index);

            if (item.name == equipmentName)
            {
                item.gameObject.SetActive(active);
                return;
            }
        }
    }

    private GameObject GetHolderObj(ModelItem.Data itemData, bool isPlayer)
    {
        string holderName = GetHolderName(itemData);

        if (holderName == string.Empty)
            return null;

        if (isPlayer)
        {
            if (!_playerHolderDic.ContainsKey(holderName))
                FindHolderObj(GameManager.Instance.PlayerObj, holderName, ref _playerHolderDic);

            if (_playerHolderDic.ContainsKey(holderName))
                return _playerHolderDic[holderName];
        }
        else
        {
            if (!_renderHolderDic.ContainsKey(holderName))
                FindHolderObj(GameManager.Instance.RenderPlayerObj, holderName, ref _renderHolderDic);

            if (_renderHolderDic.ContainsKey(holderName))
                return _renderHolderDic[holderName];
        }

        return null;
    }

    private string GetHolderName(ModelItem.Data itemData)
    {
        string holderName = string.Empty;

        switch (itemData.type)
        {
            case GameValue.ItemType.Weapon:
                {
                    ModelWeapon.Data weaponData = ModelWeapon.Model.DataDic[itemData.ref_id];

                    if (weaponData != null)
                        holderName = weaponData.equip_holder;
                }
                break;
            case GameValue.ItemType.Shield:
                {
                    ModelShield.Data shieldData = ModelShield.Model.DataDic[itemData.ref_id];

                    if (shieldData != null)
                        holderName = shieldData.equip_holder;
                }
                break;
        }

        return holderName;
    }

    private void FindHolderObj(GameObject playerObj, string holderName, ref Dictionary<string, GameObject> holderDic)
    {
        if (playerObj == null)
            return;

        foreach (var item in playerObj.GetComponentsInChildren<Transform>())
        {
            if (item.name == holderName)
            {
                if (!holderDic.ContainsKey(holderName))
                    holderDic.Add(holderName, item.gameObject);
                else
                    holderDic[holderName] = item.gameObject;

                return;
            }
        }
    }

    public void SetInventory(Inventory inventory)
    {
        _currentCategoryTab = null;
        _currentItemSlot = null;

        _inventory = inventory;
    }

    public void RefreshSlot(ItemSlot itemSlot)
    {
        if (itemSlot != null)
            itemSlot.Refresh();
    }

    public void RefreshSlot(InventoryItemData invenItemData)
    {
        var itemSlot = GetItemSlot(invenItemData);

        RefreshSlot(itemSlot);
    }

    public ItemSlot GetItemSlot(InventoryItemData invenItemData)
    {
        return _inventory.SlotList.Where(x => x.InvenItemData == invenItemData).FirstOrDefault();
    }

    public void DropItem(InventoryItemData invenItemData)
    {
        // uimanager에서 수량 조절 창 띄우기
        if (invenItemData._amount > 1)
        {
            var popup = UIManager.Instance.OpenUI<UISlideOptionPopup>();

            if (popup != null)
                popup.InitUI("NOTICE",
                            "Please choose quantity to drop",
                            invenItemData._amount,
                            () => { ActualDropItem(invenItemData); });
        }
        else
            ActualDropItem(invenItemData);
    }

    private void ActualDropItem(InventoryItemData invenItemData)
    {
        if (!ItemDic.ContainsKey(invenItemData._itemData.prefab))
        {
            Debug.Log("itemList does not have this prefab! please check list");
            return;
        }
        else
        {
            // 버린 아이템 오브젝트 활성화
            SetDropItemPosition(invenItemData);

            // 인벤토리의 아이템 데이터 지우기
            ClearItemDataInInventory(invenItemData);

            // 인벤토리 슬롯 갱신
            if (_inventory != null)
                _inventory.RefreshInventory();
        }
    }

    private void SetDropItemPosition(InventoryItemData invenItemData)
    {
        var position = GameManager.Instance.PlayerObj.transform.localPosition + (-Vector3.up * .75f);
        var itemObject = ItemDic[invenItemData._itemData.prefab];
        var randomSite = Random.insideUnitCircle.normalized;

        itemObject.transform.localPosition = new Vector3(randomSite.x, 0, randomSite.y) + position;

        ItemDic[invenItemData._itemData.prefab].SetActive(true);
    }

    private void ClearItemDataInInventory(InventoryItemData invenItemData)
    {
        var invenList = _inventoryData._inventoryDic[invenItemData._itemData.type];

        if (invenList.Contains(invenItemData))
        {
            // 장착 중인 아이템은 비활성화 시키기
            if (invenItemData._isEquip)
                ActiveEquipment(invenItemData, false);

            var index = invenList.IndexOf(invenItemData);

            invenList[index].ClearData();
        }
    }
}
