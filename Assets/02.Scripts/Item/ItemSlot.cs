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

    private InventoryItemData _invenItemData;
    public InventoryItemData InvenItemData => _invenItemData;

    public void Init(InventoryItemData itemData)
    {
        _invenItemData = itemData;

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
            ItemMenu itemMenu;

            if (UIManager.Instance.IsOpenedUI<ItemMenu>())
                itemMenu = UIManager.Instance.GetUI<ItemMenu>();
            else
                itemMenu = UIManager.Instance.OpenUI<ItemMenu>();

            if (itemMenu != null)
                itemMenu.InitButtons(this);

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

        if (_invenItemData._itemData.sprite != null)
        {
            var itemSprite = AtlasManager.Instance.GetSpriteByInventory(_invenItemData._itemData.sprite);

            if (itemSprite != null)
            {
                _itemImage.sprite = itemSprite;
                _itemImage.gameObject.SetActive(true);
            }
        }
    }

    private void SetEquip()
    {
        if (_invenItemData == null || _invenItemData._itemData.id == 0)
            _equipObj.SetActive(false);

        _equipObj.SetActive(_invenItemData._isEquip);
    }

    private void SetAmount()
    {
        if (_invenItemData == null || _invenItemData._amount == 1 || !_invenItemData._itemData.stackable)
            _amountText.gameObject.SetActive(false);
        else
            _amountText.text = _invenItemData._amount.ToString();
    }

    private void ActiveFrame(bool active)
    {
        _frameImage.gameObject.SetActive(active);
    }

    public void SetSelect(bool active)
    {
        ActiveFrame(active);

        if (active)
        {
            _frameImage.color = new Color(1f, 1f, 1f, 1f);
            // ItemManager.Instance.GoToSlotAction?.Invoke(this);
        }
    }

    private void SelectAction()
    {
        ItemManager.Instance.ChangeCurrentItemSlot(this);
    }

    public void Refresh()
    {
        SetEquip();
        SetAmount();
    }
}
