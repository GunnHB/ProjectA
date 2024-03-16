using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIRawImage : RawImage, IPointerMoveHandler
{
    public void OnPointerMove(PointerEventData eventData)
    {
        if (eventData.IsPointerMoving())
            Debug.Log("AAA");
    }
}
