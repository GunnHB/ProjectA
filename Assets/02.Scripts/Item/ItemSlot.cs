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
    [SerializeField] private UIButton _slotButton;

    private ModelItem.Data _itemData;

    public void Init(ModelItem.Data itemData)
    {
        _itemData = itemData;

        if (itemData == null)
            return;

        SetImage();
        SetEquip();
        SetAmount();

        SetSelect(false);
    }

    private void SetImage()
    {
        if (_itemData.sprite != null)
            AtlasManager.Instance.GetSpriteByInventory(_itemData.sprite);
    }

    private void SetEquip()
    {
        _equipObj.SetActive(false);
    }

    private void SetAmount()
    {
        // var amountDic = ItemManager.Instance.ThisInventoryData._itemAmount;

        // if (!amountDic.ContainsKey(_itemData) || amountDic[_itemData] == 1)
        //     _amountText.gameObject.SetActive(false);
        // else
        //     _amountText.text = amountDic[_itemData].ToString();
    }

    public void SetSelect(bool active)
    {
        _frameObj.SetActive(active);
    }
}
