using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIButton : Button, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public UnityAction EnterAction;
    public UnityAction ExitAction;

    public UnityAction DownAction;
    public UnityAction UpAction;

    public UnityAction BeginAction;
    public UnityAction DragAction;
    public UnityAction EndAction;

    public void SetEnterAndExit(UnityAction enter = null, UnityAction exit = null)
    {
        EnterAction = enter;
        ExitAction = exit;
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
