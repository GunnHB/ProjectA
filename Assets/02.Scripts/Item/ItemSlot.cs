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

    }

    private void SetAmount()
    {

    }

    public void SetSelect(bool active)
    {
        _frameObj.SetActive(active);
    }
}
