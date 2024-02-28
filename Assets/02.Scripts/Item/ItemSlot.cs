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
    [SerializeField] private Image _frameImage;
    [SerializeField] private UIButton _slotButton;

    private ModelItem.Data _itemData;
    public ModelItem.Data ItemData => _itemData;

    public void Init(ModelItem.Data itemData)
    {
        _itemData = itemData;

        if (itemData == null)
            return;

        SetButtonAction();

        SetImage();
        SetEquip();
        SetAmount();

        SetSelect(false);
    }

    private void SetButtonAction()
    {
        _slotButton.onClick.AddListener(SelectAction);
        _slotButton.SetEnterAndExit(EnterAction, ExitAction);

        _slotButton.SetRightClickAction(() =>
        {
            var itemMenu = UIManager.Instance.OpenUI<ItemMenu>();

            if (itemMenu != null)
                itemMenu.InitButtons(this);

            if (_itemData != null && _itemData.id != 0)
                SelectAction();
        });
    }

    private void EnterAction()
    {
        if (ItemManager.Instance.CurrentItemSlot == this)
            return;

        ActiveFrame(true);

        _frameImage.color = new Color(1f, 1f, 1f, .75f);
    }

    private void ExitAction()
    {
        if (ItemManager.Instance.CurrentItemSlot == this)
            return;

        ActiveFrame(false);
    }

    private void SetImage()
    {
        _itemImage.gameObject.SetActive(false);

        if (_itemData.sprite != null)
        {
            var itemSprite = AtlasManager.Instance.GetSpriteByInventory(_itemData.sprite);

            if (itemSprite != null)
            {
                _itemImage.sprite = itemSprite;
                _itemImage.gameObject.SetActive(true);
            }
        }
    }

    private void SetEquip()
    {
        _equipObj.SetActive(false);
    }

    private void SetAmount()
    {
        var amountDic = ItemManager.Instance.ThisInventoryData._itemAmount;

        if (!amountDic.ContainsKey(_itemData) || amountDic[_itemData] == 1)
            _amountText.gameObject.SetActive(false);
        else
        {
            if (!_itemData.stackable)
                _amountText.gameObject.SetActive(false);
            else
                _amountText.text = amountDic[_itemData].ToString();
        }
    }

    private void ActiveFrame(bool active)
    {
        _frameImage.gameObject.SetActive(active);
    }

    public void SetSelect(bool active)
    {
        ActiveFrame(active);

        if (active)
            _frameImage.color = new Color(1f, 1f, 1f, 1f);
    }

    private void SelectAction()
    {
        ItemManager.Instance.ChangeCurrentItemSlot(this);
    }
}
