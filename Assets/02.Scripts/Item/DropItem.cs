using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField]
    private ObjectPool _dropItemPool;

    public ObjectPool GetObjectPool()
    {
        return _dropItemPool;
    }
}
