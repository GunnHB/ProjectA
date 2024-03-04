using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using TMPro;

[RequireComponent(typeof(Image))]
public class UIButton : Button, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private UnityAction EnterAction;
    private UnityAction ExitAction;

    private UnityAction DownAction;
    private UnityAction UpAction;

    private UnityAction BeginAction;
    private UnityAction DragAction;
    private UnityAction EndAction;

    private UnityAction RightClickAction;

    private TextMeshProUGUI _buttonText;
    public TextMeshProUGUI ButtonText
    {
        get
        {
            if (_buttonText == null)
            {
                var tempText = GetComponentInChildren<TextMeshProUGUI>();

                if (tempText != null)
                    _buttonText = tempText;
            }

            return _buttonText;
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        // 우클릭 시 실행될 액션
        if (eventData.button == PointerEventData.InputButton.Right)
            RightClickAction?.Invoke();
    }

    public void SetEnterAndExit(UnityAction enter = null, UnityAction exit = null)
    {
        EnterAction = enter;
        ExitAction = exit;
    }

    public void SetRightClickAction(UnityAction action = null)
    {
        RightClickAction = action;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        EnterAction?.Invoke();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        ExitAction?.Invoke();
    }

    // public override void OnPointerDown(PointerEventData eventData)
    // {
    //     base.OnPointerDown(eventData);
    // }

    // public override void OnPointerUp(PointerEventData eventData)
    // {
    //     base.OnPointerUp(eventData);
    // }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // throw new System.NotImplementedException();
    }
}
