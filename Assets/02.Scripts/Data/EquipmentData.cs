using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class EquipmentData
{
    public InventoryItemData _invenItemData;
    public GameObject _itemPrefab;

    public EquipmentData()
    {
        ClearData();
    }

    public void ClearData()
    {
        _invenItemData = new InventoryItemData();
        _itemPrefab = null;
    }
}
