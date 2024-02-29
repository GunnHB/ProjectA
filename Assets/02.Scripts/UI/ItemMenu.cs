using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ItemMenu : UIPopupBase
{
    private const string GROUP_MENU = "MenuGroup";

    [BoxGroup(GROUP_MENU), SerializeField]
    private Transform _parentTr;
    [BoxGroup(GROUP_MENU), SerializeField]
    private UIButton _useButton;
    [BoxGroup(GROUP_MENU), SerializeField]
    private UIButton _discardButton;
    [BoxGroup(GROUP_MENU), SerializeField]
    private UIButton _cancelButton;

    private ItemSlot _targetSlot;

    public override void Init()
    {
        base.Init();

        ItemManager.Instance.SetItemMenu(this);
    }

    public void InitButtons(ItemSlot slot)
    {
        if (_targetSlot != null && _targetSlot == slot)
            return;

        _targetSlot = slot;

        SetPosition();

        _useButton.onClick.RemoveAllListeners();
        _discardButton.onClick.RemoveAllListeners();
        _cancelButton.onClick.RemoveAllListeners();

        _useButton.onClick.AddListener(OnClickUse);
        _discardButton.onClick.AddListener(OnClickDiscard);
        _cancelButton.onClick.AddListener(Close);
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
        // Debug.Log($"{_targetSlot.ItemData.name} use!");

        ActiveEquipment(_targetSlot.ItemData.prefab, true);
    }

    private void OnClickDiscard()
    {
        // Debug.Log($"{_targetSlot.ItemData.name} discard!");
    }

    private void ActiveEquipment(string equipmentName, bool active)
    {
        var playersHolder = ItemManager.Instance.GetHolderObj(_targetSlot.ItemData, true);
        var rendersHolder = ItemManager.Instance.GetHolderObj(_targetSlot.ItemData, false);

        ItemManager.Instance.ActiveEquipment(playersHolder, equipmentName, active);
        ItemManager.Instance.ActiveEquipment(rendersHolder, equipmentName, active);

        // ItemManager.Instance.ActiveEquipment(ItemManager.Instance.RightHolder, equipmentName, active);
        // ItemManager.Instance.ActiveEquipment(ItemManager.Instance.RenderRightHolder, equipmentName, active);
    }
}
