using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPool
{
    public GameObject _prefabObj;
    public GameObject _parentObj;

    private Queue<GameObject> _poolQueue = new();

    public void CreateNewObject()
    {

    }

    public void GetObject()
    {
        if (_poolQueue.Count > 0)
        {
            // var tempObj = _poolQueue
        }
    }

    public void ReturnObject()
    {

    }

    public void ReturnAllObject()
    {

    }
}
