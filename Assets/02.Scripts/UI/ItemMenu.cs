using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMenu : UIPopupBase
{
    [SerializeField]
    private UIButton _useButton;
    [SerializeField]
    private UIButton _discardButton;
    [SerializeField]
    private UIButton _cancelButton;

    private ItemSlot _targetSlot;

    public void InitButtons(ItemSlot slot)
    {
        _targetSlot = slot;

        if (_targetSlot == null || _targetSlot.ItemData == null || _targetSlot.ItemData.id == 0)
            return;

        SetPosition();

        _useButton.onClick.AddListener(() => { Debug.Log($"{_targetSlot.ItemData} use!"); });
        _discardButton.onClick.AddListener(() => { Debug.Log($"{_targetSlot.ItemData} discard!"); });
        _cancelButton.onClick.AddListener(Close);
    }

    private void SetPosition()
    {
        Debug.Log($"{_targetSlot.transform.position} posision");
        Debug.Log($"{_targetSlot.transform.localPosition} localPosition");
    }
}
