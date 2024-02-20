using System.Collections;
using System.Collections.Generic;

using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "ScriptableObject/Create ItemData", order = int.MinValue)]
public class ItemData : SerializedScriptableObject
{
    public string _itemName;
    public string _itemDesc;
    public string _itemImage;
    public GameObject _prefab;
}
