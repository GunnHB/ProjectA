using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ItemManager : SingletonObject<ItemManager>
{
    private const string INVENTORY_DATA_PATH = "Assets/08.Tables/Json/InventoryData.json";

    private const string DROP_ITEM_PARENT = "DropItemParent";

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
    public UnityAction DropItemAction;

    private ItemMenu _itemMenu;
    public ItemMenu ThisItemMenu => _itemMenu;

    private Dictionary<string, GameObject> _playerHolderDic = new();
    private Dictionary<string, GameObject> _renderHolderDic = new();

    private DropItem _dropItem;
    public DropItem ThisDropItem { get { return GameManager.Instance.GetObject(DROP_ITEM_PARENT, ref _dropItem); } }

    // 현재 버린 아이템들
    private DropItemObject _currDropItemObject;

    protected override void Awake()
    {
        base.Awake();

        InitInventoryData();
    }

    private void Start()
    {
        InitInGameScene();
    }

    /// <summary>
    /// InGame 씬으로 넘어갔을 때 호출
    /// </summary>
    public void InitInGameScene()
    {
        if (_dropItem == null)
            ThisDropItem.GetObjectPool().Initialize(10);
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

    public void DrawWeapon()
    {
        var weaponData = _equipmentData._itemWeaponData;

        if (weaponData.IsEmpty())
            return;

        var playerHolder = GetHolderObj(weaponData._itemData, isPlayer: true, isSheath: false);
        var renderHolder = GetHolderObj(weaponData._itemData, isPlayer: false, isSheath: false);

        if (playerHolder != null)
            ActualActiveEquipment(playerHolder, weaponData._itemData.prefab, true);

        if (renderHolder != null)
            ActualActiveEquipment(renderHolder, weaponData._itemData.prefab, true);
    }

    public void ActiveEquipment(InventoryItemData invenItemData, bool active)
    {
        var playerHolder = GetHolderObj(invenItemData._itemData, isPlayer: true);
        var renderHolder = GetHolderObj(invenItemData._itemData, isPlayer: false);

        if (playerHolder != null)
            ActualActiveEquipment(playerHolder, invenItemData._itemData.prefab, active);

        if (renderHolder != null)
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

    private GameObject GetHolderObj(ModelItem.Data itemData, bool isPlayer, bool isSheath = true)
    {
        string holderName = GetHolderName(itemData, isSheath);

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

    private string GetHolderName(ModelItem.Data itemData, bool isSheath)
    {
        string holderName = string.Empty;

        switch (itemData.type)
        {
            case GameValue.ItemType.Weapon:
                {
                    ModelWeapon.Data weaponData = ModelWeapon.Model.DataDic[itemData.ref_id];

                    if (weaponData != null)
                        holderName = isSheath ? weaponData.equip_holder : weaponData.hand_holder;
                }
                break;
            case GameValue.ItemType.Shield:
                {
                    ModelShield.Data shieldData = ModelShield.Model.DataDic[itemData.ref_id];

                    if (shieldData != null)
                        holderName = isSheath ? shieldData.equip_holder : shieldData.hand_holder;
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
        // 버린 아이템이 있다면 주변에 오브젝트 활성화
        if (inventory == null)
        {
            if (_currDropItemObject != null)
            {
                var gameObj = _currDropItemObject.gameObject;
                gameObj.transform.localPosition = DropItemPosition();

                gameObj.SetActive(true);
            }
        }

        // 초기화
        _currentCategoryTab = null;
        _currentItemSlot = null;
        _currDropItemObject = null;

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
                            (int amount) => { AddToDropItemCollection(invenItemData, amount); });
        }
        else
            AddToDropItemCollection(invenItemData);
    }

    private void AddToDropItemCollection(InventoryItemData invenItemData, int dropAmount = 1)
    {
        if (dropAmount == 0)
            return;

        if (_currDropItemObject == null)
        {
            var temp = _dropItem.GetObjectPool().GetObject(false);

            if (temp == null)
                return;

            var dropObj = temp.GetComponent<DropItemObject>();

            if (dropObj == null)
                return;

            _currDropItemObject = dropObj;
        }

        _currDropItemObject.AddData(invenItemData._itemData, invenItemData._amount);

        invenItemData._amount -= dropAmount;

        // 남은 아이템의 수가 0이면 인벤토리 아이템 데이터 지우기
        if (invenItemData._amount < 1)
            ClearItemDataInInventory(invenItemData);

        // 인벤토리 슬롯 갱신
        if (_inventory != null)
            _inventory.RefreshInventory();

        // if (!ItemDic.ContainsKey(invenItemData._itemData.prefab))
        // {
        //     Debug.Log("itemList does not have this prefab! please check list");
        //     return;
        // }
        // else
        // {
        //     // 버린 아이템 오브젝트 활성화
        //     SetDropItemPosition(invenItemData);

        //     // 인벤토리의 아이템 데이터 지우기
        //     ClearItemDataInInventory(invenItemData);

        //     // 인벤토리 슬롯 갱신
        //     if (_inventory != null)
        //         _inventory.RefreshInventory();
        // }
    }

    private Vector3 DropItemPosition()
    {
        var randomSite = Random.insideUnitCircle.normalized;

        return GameManager.Instance.PlayerObj.transform.localPosition + new Vector3(randomSite.x, 0f, randomSite.y);
    }

    private void SetDropItemPosition(InventoryItemData invenItemData)
    {
        // var position = GameManager.Instance.PlayerObj.transform.localPosition + (-Vector3.up * .75f);
        // var itemObject = ItemDic[invenItemData._itemData.prefab];
        // var randomSite = Random.insideUnitCircle.normalized;

        // itemObject.transform.localPosition = new Vector3(randomSite.x, 0, randomSite.y) + position;

        // ItemDic[invenItemData._itemData.prefab].SetActive(true);
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
