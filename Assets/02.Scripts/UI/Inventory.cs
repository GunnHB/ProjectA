using System.Collections;
using System.Collections.Generic;

using Sirenix.OdinInspector;

using UnityEngine;

public class Inventory : MonoBehaviour
{
    private const string CATEGORY = "Category";
    private const string SLOTS = "Slots";

    [BoxGroup(CATEGORY), SerializeField]
    private ObjectPool _categoryPool;
    [BoxGroup(CATEGORY), SerializeField]
    private List<Sprite> _normalList = new();
    [BoxGroup(CATEGORY), SerializeField]
    private List<Sprite> _selectList = new();

    [BoxGroup(SLOTS), SerializeField]
    private ObjectPool _rowPool;

    public void Init()
    {

    }
}
