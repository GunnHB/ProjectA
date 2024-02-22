using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

using UnityEngine;

public class Inventory : MonoBehaviour
{
    private const string CATEGORY = "Category";
    private const string SLOTS = "Slots";

    [BoxGroup(CATEGORY), SerializeField]
    private ObjectPool _categoryPool;

    [BoxGroup(SLOTS), SerializeField]
    private ObjectPool _rowPool;

    private Dictionary<GameValue.ItemType, List<ModelItem.Data>> _inventoryDic;

    public void Init()
    {
        _inventoryDic = ItemManager.Instance.ThisInventoryData._inventoryDic;

        ItemManager.Instance.TabAction = null;
        ItemManager.Instance.TabAction = InitSlots;

        InitCategory();
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
            invenRow.Init(rowIndex, doRemain);
    }
}
