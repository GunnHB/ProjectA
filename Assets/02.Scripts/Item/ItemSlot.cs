using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private GameObject _equipObj;
    [SerializeField] private TextMeshProUGUI _amountText;
    [SerializeField] private GameObject _frameObj;

    private ItemData _itemData;

    public void Init(ItemData itemData)
    {
        _itemData = itemData;

        if (_itemData == null)
            return;

        AtlasManager.Instance.GetSpriteByInventory(_itemData._itemImage);
    }
}
