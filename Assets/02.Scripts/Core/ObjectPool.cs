using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[System.Serializable]
public class ObjectPool
{
    public GameObject _prefabObj;
    public GameObject _parentObj;

    private Queue<GameObject> _poolQueue = new();

    public GameObject CreateNewObject()
    {
        GameObject tempObj = GameObject.Instantiate(_prefabObj);

        tempObj.SetActive(false);

        if (_parentObj != null)
            tempObj.transform.SetParent(_parentObj.transform);

        return tempObj;
    }

    public GameObject GetObject()
    {
        GameObject tempObj;

        if (_poolQueue.Count > 0)
            tempObj = _poolQueue.Dequeue();
        else
            tempObj = CreateNewObject();

        tempObj.SetActive(true);

        return tempObj;
    }

    public void ReturnObject(GameObject obj)
    {
        _poolQueue.Enqueue(obj);

        if (_parentObj != null)
            obj.transform.SetParent(_parentObj.transform);

        obj.SetActive(false);
    }

    public void ReturnAllObject()
    {
        if (_parentObj == null)
            return;

        for (int index = 0; index < _parentObj.transform.childCount; index++)
        {
            var temp = _parentObj.transform.GetChild(index);

            if (temp.gameObject.activeInHierarchy)
                ReturnObject(temp.gameObject);
        }
    }
}
