using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class DropItemData
{
    public ModelItem.Data _itemData;
    public int _amount;

    public DropItemData()
    {
        ClearData();
    }

    public DropItemData(ModelItem.Data data, int amount)
    {
        _itemData = data;
        _amount = amount;
    }

    public void ClearData()
    {
        _itemData = new ModelItem.Data();
        _amount = 0;
    }
}

public class DropItemObject : MonoBehaviour
{
    private Dictionary<ModelItem.Data, DropItemData> _dropItemDic = new();

    public void AddData(ModelItem.Data data, int amount)
    {
        if (_dropItemDic.ContainsKey(data))
            _dropItemDic[data]._amount += amount;
        else
            _dropItemDic.Add(data, new DropItemData(data, amount));
    }

    public void RemoveData(ModelItem.Data data)
    {
        if (_dropItemDic.ContainsKey(data))
            _dropItemDic.Remove(data);
    }
}
