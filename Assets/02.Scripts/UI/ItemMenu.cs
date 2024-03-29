using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class ItemMenu : UIPopupBase
{
    private const string GROUP_MENU = "MenuGroup";

    [BoxGroup(GROUP_MENU), SerializeField]
    private Transform _parentTr;
    [BoxGroup(GROUP_MENU), SerializeField]
    private UIButton _useButton;
    [BoxGroup(GROUP_MENU), SerializeField]
    private UIButton _dropButton;
    [BoxGroup(GROUP_MENU), SerializeField]
    private UIButton _cancelButton;

    private ItemSlot _targetSlot;

    private bool IsEquipment
    {
        get
        {
            return _targetSlot.InvenItemData._itemData.type != GameValue.ItemType.Food &&
                    _targetSlot.InvenItemData._itemData.type != GameValue.ItemType.Default;
        }
    }

    public override void Init()
    {
        base.Init();

        _useButton.onClick.RemoveAllListeners();
        _dropButton.onClick.RemoveAllListeners();
        _cancelButton.onClick.RemoveAllListeners();

        ItemManager.Instance.SetItemMenu(this);
    }

    public void InitButtons(ItemSlot slot)
    {
        if (_targetSlot != null && _targetSlot == slot)
            return;

        _targetSlot = slot;

        SetPosition();

        if (!_targetSlot.InvenItemData._isEquip)
        {
            _useButton.onClick.AddListener(OnClickUse);

            if (IsEquipment)
                _useButton.ButtonText.text = "EQUIP";
            else
                _useButton.ButtonText.text = "USE";
        }
        else
        {
            _useButton.onClick.AddListener(OnClickRemove);
            _useButton.ButtonText.text = "REMOVE";
        }

        _dropButton.onClick.AddListener(OnClickDrop);
        _cancelButton.onClick.AddListener(OnClickCancel);

        _dropButton.ButtonText.text = "DROP";
    }

    private void SetPosition()
    {
        var thisRect = _parentTr as RectTransform;
        var targetRect = _targetSlot.transform as RectTransform;

        thisRect.anchorMin = Vector2.zero;
        thisRect.anchorMax = Vector2.zero;
        thisRect.pivot = targetRect.pivot;

        // 약간은 하드코딩임
        float modiY = targetRect.position.y > 400f ? -targetRect.rect.height : targetRect.rect.height;

        thisRect.anchoredPosition = new Vector2(targetRect.position.x, targetRect.position.y + modiY);
    }

    public override void Close()
    {
        ItemManager.Instance.SetItemMenu(null);

        base.Close();
    }

    private void OnClickUse()
    {
        // 장비 착용 시 실행
        if (IsEquipment)
            ItemManager.Instance.EquipWeaon(_targetSlot);

        UIManager.Instance.CloseUI(this);
    }

    private void OnClickRemove()
    {
        // 착용한 장비 해제
        ItemManager.Instance.RemoveWeapon(_targetSlot.InvenItemData);

        UIManager.Instance.CloseUI(this);
    }

    private void OnClickDrop()
    {
        ItemManager.Instance.DropItem(_targetSlot.InvenItemData);

        UIManager.Instance.CloseUI(this);
    }

    private void OnClickCancel()
    {
        UIManager.Instance.CloseUI(this);
    }
}
