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
    private const string INVENTORY_RENDER_TEXTURE = "InventoryRenderTexture";
    private const string CATEGORY = "Category";
    private const string SLOTS = "Slots";
    private const string ITEM_DESC = "ItemDesc";

    [BoxGroup(CATEGORY), SerializeField]
    private ObjectPool _categoryPool;

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

    private Dictionary<GameValue.ItemType, List<ModelItem.Data>> _inventoryDic;

    private List<DOTweenAnimation> _tweenAnimations;

    public void Init()
    {
        _inventoryDic = ItemManager.Instance.ThisInventoryData._inventoryDic;

        ItemManager.Instance.TabAction = null;
        ItemManager.Instance.TabAction = InitSlots;

        ItemManager.Instance.SlotAction = null;
        ItemManager.Instance.SlotAction = SetDesc;

        _tweenAnimations = _descObj.GetComponentsInChildren<DOTweenAnimation>().ToList();

        InitCategory();
    }

    private void InitRenderTexture()
    {
        if (_playerRawImage == null)
            return;

        var renderTexture = AssetBundleManager.Instance.GetUIBundle().LoadAsset<RenderTexture>(INVENTORY_RENDER_TEXTURE);

        if (renderTexture == null)
            return;

        _playerRawImage.texture = renderTexture;
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

    private void SetDesc(ModelItem.Data itemData)
    {
        if (itemData == null || itemData.id == 0)
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

        _itemNameText.text = itemData.name;
        _itemDescText.text = itemData.desc;

        InitRenderTexture();
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
}
