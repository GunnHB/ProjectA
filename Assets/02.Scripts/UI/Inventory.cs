using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using Sirenix.OdinInspector;

using TMPro;

using DG.Tweening;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private const string PLAYER_RENDER_TEXTURE = "RenderTexturePlayer";
    private const string CAMERA_RENDER_TEXTURE = "RenderTextureCamera";

    private const string CATEGORY = "Category";
    private const string SLOTS = "Slots";
    private const string ITEM_DESC = "ItemDesc";

    [BoxGroup(CATEGORY), SerializeField]
    private ObjectPool _categoryPool;

    [BoxGroup(SLOTS), SerializeField]
    private ScrollRect _scroll;
    [BoxGroup(SLOTS), SerializeField]
    private ObjectPool _rowPool;

    [BoxGroup(ITEM_DESC), SerializeField]
    private RawImage _playerRawImage;
    [BoxGroup(ITEM_DESC), SerializeField]
    private GameObject _descObj;
    [BoxGroup(ITEM_DESC), SerializeField]
    private TextMeshProUGUI _itemNameText;
    [BoxGroup(ITEM_DESC), SerializeField]
    private TextMeshProUGUI _itemDescText;

    private Dictionary<GameValue.ItemType, List<InventoryItemData>> _inventoryDic;

    private List<DOTweenAnimation> _tweenAnimations;

    public void Init()
    {
        _inventoryDic = ItemManager.Instance.ThisInventoryData._inventoryDic;

        ItemManager.Instance.TabAction = null;
        ItemManager.Instance.TabAction = InitSlots;

        ItemManager.Instance.SlotAction = null;
        ItemManager.Instance.SlotAction = SetDesc;

        ItemManager.Instance.GoToSlotAction = null;
        ItemManager.Instance.GoToSlotAction = GoToSelectSlot;

        _tweenAnimations = _descObj.GetComponentsInChildren<DOTweenAnimation>().ToList();

        InitCategory();
        InitRenderTexture();
    }

    private void InitRenderTexture()
    {
        if (_playerRawImage == null)
            return;

        var renderTexture = new RenderTexture(256, 256, 24);
        Camera renderCam = GameObject.Find(PLAYER_RENDER_TEXTURE).transform.Find(CAMERA_RENDER_TEXTURE).GetComponent<Camera>();

        if (renderCam == null)
            return;

        renderCam.targetTexture = renderTexture;
        _playerRawImage.texture = renderTexture;

        if (!renderCam.gameObject.activeInHierarchy)
            renderCam.gameObject.SetActive(true);
    }

    private void InitCategory()
    {
        var sortedList = ModelCategoryTab.Model.DataList.OrderBy(x => x.order).ToList();

        for (int index = 0; index < sortedList.Count; index++)
        {
            var item = sortedList[index];
            var catePrefab = _categoryPool.GetObject();

            if (catePrefab.TryGetComponent(out CategoryTab tab))
            {
                tab.Init(item);

                if (index == 0)
                {
                    ItemManager.Instance.SetCurrentCategoryTab(tab);
                    tab.SetSelect(true);
                }
            }
        }
    }

    private void InitSlots(ModelCategoryTab.Data cateData)
    {
        if (_inventoryDic[cateData.type].Count == 0)
            return;

        ItemManager.Instance.SetCurrentItemSlot(null);
        ItemManager.Instance.SlotAction?.Invoke(null);

        // 메뉴 창 닫기
        if (ItemManager.Instance.ThisItemMenu != null)
            UIManager.Instance.CloseUI(ItemManager.Instance.ThisItemMenu);

        int count = _inventoryDic[cateData.type].Count / GameValue._inventoryRowAmount;   // inventoryrow 개수
        int remain = _inventoryDic[cateData.type].Count % GameValue._inventoryRowAmount;  // 슬롯의 나머지

        _rowPool.ReturnAllObject();

        for (int index = 0; index < count + 1; index++)
        {
            bool isLastIndex = index == count;

            if (isLastIndex && remain == 0)
                break;

            GetRowObject(index, index == count && remain != 0);
        }
    }

    private void GetRowObject(int rowIndex, bool doRemain)
    {
        var rowObj = _rowPool.GetObject();

        if (rowObj.TryGetComponent(out InventoryRow invenRow))
        {
            invenRow.Init(rowIndex, doRemain);

            rowObj.transform.SetAsLastSibling();
        }
    }

    // private void SetDesc(ModelItem.Data itemData)
    private void SetDesc(InventoryItemData invenItemData)
    {
        if (invenItemData == null || invenItemData._itemData.id == 0)
        {
            if (_descObj.activeInHierarchy)
                DoTweenPlay(false);

            return;
        }

        if (_descObj.TryGetComponent(out CanvasGroup group))
        {
            if (group.alpha == 0)
            {
                _descObj.SetActive(true);
                DoTweenPlay(true);
            }
        }

        _itemNameText.text = invenItemData._itemData.name;
        _itemDescText.text = invenItemData._itemData.desc;
    }

    private void DoTweenPlay(bool forward)
    {
        foreach (var anim in _tweenAnimations)
        {
            if (forward)
                anim.DORestart();
            else
                anim.DOPlayBackwards();
        }
    }

    // 해당 슬롯으로 이동
    private void GoToSelectSlot(ItemSlot slot)
    {
        // Debug.Log(slot.ItemData.name);
        float scrollValue = (slot.transform as RectTransform).rect.height /
                            _scroll.content.rect.height - (_scroll.transform as RectTransform).rect.height;

        _scroll.verticalNormalizedPosition = scrollValue;
    }
}
