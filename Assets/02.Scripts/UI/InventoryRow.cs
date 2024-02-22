using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryRow : MonoBehaviour
{
    [SerializeField] private ObjectPool _itemSlotPool;

    public void Init(int rowIndex, bool remain)
    {
        var currTabType = ItemManager.Instance.CurrentCategoryTab.ThisData.type;
        var currDic = ItemManager.Instance.ThisInventoryData._inventoryDic;

        int startIndex = rowIndex * GameValue._inventoryRowAmount;
        int endIndex = remain ? currDic[currTabType].Count : startIndex + GameValue._inventoryRowAmount;

        _itemSlotPool.ReturnAllObject();

        for (int index = startIndex; index < endIndex; index++)
        {
            var data = currDic[currTabType][index];
            var slotObj = _itemSlotPool.GetObject();

            if (slotObj.TryGetComponent(out ItemSlot slot))
                slot.Init(data);
        }
    }
}