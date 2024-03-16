using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIRawImage : RawImage, IPointerClickHandler
{
    public enum EventType
    {
        None = -1,
        Click = 1 << 0,
        EnterExit = 1 << 1,
        Drag = 1 << 2,
        UpDown = 1 << 3,
        Move = 1 << 4,
    }

    private const string GROUP_OPTIONS = "Options";

    [BoxGroup(GROUP_OPTIONS), SerializeField]
    private bool _needClickEvent;

    [BoxGroup(GROUP_OPTIONS), SerializeField, ShowIf(nameof(_needClickEvent))]
    private EventType _eventType;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_needClickEvent)
            return;


    }
}
