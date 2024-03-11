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

    // private EquipmentData _equipmentData = null;
    // public EquipmentData ThisEquipmentData => _equipmentData;

    // 현재 장착 데이터
    private EquipmentData _equipWeaponData = null;
    public EquipmentData EquipWeaponData => _equipWeaponData;

    private EquipmentData _equipShieldData = null;
    public EquipmentData EquipShieldData => _equipShieldData;

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

            // _equipmentData = new EquipmentData();
            _equipWeaponData = new EquipmentData();
            _equipShieldData = new EquipmentData();
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

    public void DrawWeapon()
    {
        var weaponItemData = _equipWeaponData._invenItemData;
        var shieldItemData = _equipShieldData._invenItemData;

        // 무기 아이템 데이터가 없으면 리턴
        if (weaponItemData.IsEmpty())
            return;

        var weaponData = ModelWeapon.Model.DataDic[weaponItemData._itemData.ref_id];            // 무기 데이터

        // 손에 있는건 켜고 보관함에 있는건 끄기
        ActiveEquipment(weaponItemData._itemData, isSheath: false, isActive: true);
        ActiveEquipment(weaponItemData._itemData, isSheath: true, isActive: false);

        if (!shieldItemData.IsEmpty())
        {
            // 방패와 함께 장착할 수 있는 무기라면
            if (weaponData.able_equip_shield)
            {
                ActiveEquipment(shieldItemData._itemData, isSheath: false, isActive: true);
                ActiveEquipment(shieldItemData._itemData, isSheath: true, isActive: false);
            }
            else
            {
                // 손에 있는건 끄고 보관함에 있는건 켜기
                ActiveEquipment(shieldItemData._itemData, isSheath: true, isActive: true);
                ActiveEquipment(shieldItemData._itemData, isSheath: false, isActive: false);
            }
        }
    }

    public void SheathWeapon()
    {
        var weaponItemData = _equipWeaponData._invenItemData;
        var shieldItemData = _equipShieldData._invenItemData;

        if (weaponItemData.IsEmpty())
            return;

        var weaponData = ModelWeapon.Model.DataDic[weaponItemData._itemData.ref_id];

        // 보관함에 있는건 켜고 손에 있는건 끄기
        ActiveEquipment(weaponItemData._itemData, isSheath: true, isActive: true);
        ActiveEquipment(weaponItemData._itemData, isSheath: false, isActive: false);

        if (!shieldItemData.IsEmpty())
        {
            if (weaponData.able_equip_shield)
            {
                ActiveEquipment(shieldItemData._itemData, isSheath: false, isActive: true);
                ActiveEquipment(shieldItemData._itemData, isSheath: true, isActive: false);
            }

            ActiveEquipment(shieldItemData._itemData, isSheath: true, isActive: true);
            ActiveEquipment(shieldItemData._itemData, isSheath: false, isActive: false);
        }
    }

    /// <summary>
    /// 아이템 활성화시키기
    /// </summary>
    /// <param name="holder"></param>
    /// <param name="equipmentName"></param>
    /// <param name="active"></param>
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

    /// <summary>
    /// 아이템 활성화
    /// </summary>
    /// <param name="itemData"></param>
    /// <param name="isSheath"></param>
    /// <param name="isActive"></param>
    public void ActiveEquipment(ModelItem.Data itemData, bool isSheath, bool isActive)
    {
        var playerHolder = GetHolderObj(itemData, isPlayer: true, isSheath);
        var renderHolder = GetHolderObj(itemData, isPlayer: false, isSheath);

        if (playerHolder != null)
            ActualActiveEquipment(playerHolder, itemData.prefab, isActive);

        if (renderHolder != null)
            ActualActiveEquipment(renderHolder, itemData.prefab, isActive);
    }

    /// <summary>
    /// 아이템의 고유 홀더 오브젝트 찾기
    /// </summary>
    /// <param name="itemData">활성화할 아이템 데이터</param>
    /// <param name="isPlayer">실제 플레이어? / 렌더 텍스쳐의 플레이어?</param>
    /// <param name="isSheath">SheathHolder? / HandHolder?</param>
    /// <returns></returns>
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

    /// <summary>
    /// 홀더의 이름 찾기 (string을 기반으로 오브젝트를 찾습니다.)
    /// </summary>
    /// <param name="itemData"></param>
    /// <param name="isSheath"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 홀더 찾기
    /// </summary>
    /// <param name="playerObj"></param>
    /// <param name="holderName"></param>
    /// <param name="holderDic"></param>
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
    }

    private Vector3 DropItemPosition()
    {
        var randomSite = Random.insideUnitCircle.normalized;

        return GameManager.Instance.PlayerObj.transform.localPosition + new Vector3(randomSite.x, 0f, randomSite.y);
    }

    private void ClearItemDataInInventory(InventoryItemData invenItemData)
    {
        var invenList = _inventoryData._inventoryDic[invenItemData._itemData.type];

        if (invenList.Contains(invenItemData))
        {
            // 장착 중인 아이템은 비활성화 시키기 (sheathHolder)
            if (invenItemData._isEquip)
                ActiveEquipment(invenItemData._itemData, isSheath: true, isActive: false);

            var index = invenList.IndexOf(invenItemData);

            invenList[index].ClearData();
        }
    }

    public void EquipWeaon(ItemSlot newSlot)
    {
        switch (newSlot.InvenItemData._itemData.type)
        {
            case GameValue.ItemType.Weapon:
                RefreshEquipment(newSlot, ref _equipWeaponData);
                break;
            case GameValue.ItemType.Shield:
                RefreshEquipment(newSlot, ref _equipShieldData);
                break;
        }
    }

    public void RemoveWeapon(InventoryItemData itemData)
    {
        switch (itemData._itemData.type)
        {
            case GameValue.ItemType.Weapon:
                RemoveEquipment(ref _equipWeaponData);
                break;
            case GameValue.ItemType.Shield:
                RemoveEquipment(ref _equipShieldData);
                break;
        }
    }

    private void RefreshEquipment(ItemSlot newSlot, ref EquipmentData equipmentData)
    {
        RemoveEquipment(ref equipmentData);                  // 해제
        SetEquipment(newSlot, ref equipmentData);            // 장착
    }

    private void RemoveEquipment(ref EquipmentData equipmentData)
    {
        if (equipmentData._invenItemData.IsEmpty())
        {
            equipmentData.ClearData();
            return;
        }

        equipmentData._invenItemData._isEquip = false;

        ActiveEquipment(equipmentData._invenItemData._itemData, isSheath: true, isActive: false);

        RefreshSlot(equipmentData._invenItemData);
    }

    private void SetEquipment(ItemSlot newSlot, ref EquipmentData equipmentData)
    {
        newSlot.InvenItemData._isEquip = true;
        equipmentData._invenItemData = newSlot.InvenItemData;

        ActiveEquipment(equipmentData._invenItemData._itemData, isSheath: true, isActive: true);

        RefreshSlot(equipmentData._invenItemData);
    }

    // private void SetPrefab(EquipmentData equipmentData)
    // {
    //     switch (equipmentData._invenItemData._itemData.type)
    //     {
    //         case GameValue.ItemType.Weapon:
    //             {
    //                 var weaponData = ModelWeapon.Model.DataDic[equipmentData._invenItemData._itemData.ref_id];

    //                 if (weaponData == null)
    //                     return;

    //                 for (int index = 0; index < _playerHolderDic[weaponData.hand_holder].transform.childCount; index++)
    //                 {
    //                     var item = _playerHolderDic[weaponData.hand_holder].transform.GetChild(index);

    //                     if (item.gameObject.activeInHierarchy)
    //                     {
    //                         equipmentData._itemPrefab = item.gameObject;
    //                         return;
    //                     }
    //                 }

    //                 equipmentData._itemPrefab = null;
    //             }
    //             break;
    //         case GameValue.ItemType.Shield:
    //             {
    //                 var shieldData = ModelShield.Model.DataDic[equipmentData._invenItemData._itemData.ref_id];

    //                 if (shieldData == null)
    //                     return;

    //                 for (int index = 0; index < _playerHolderDic[shieldData.hand_holder].transform.childCount; index++)
    //                 {
    //                     var item = _playerHolderDic[shieldData.hand_holder].transform.GetChild(index);

    //                     if (item.gameObject.activeInHierarchy)
    //                     {
    //                         equipmentData._itemPrefab = item.gameObject;
    //                         return;
    //                     }
    //                 }

    //                 equipmentData._itemPrefab = null;
    //             }
    //             break;
    //     }
    // }
}
