using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIEventTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private Action onClickHandler = null;
    private Action onEnterHandler = null;
    private Action onExitHandler = null;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (onClickHandler != null)
        {
            onClickHandler();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (onEnterHandler != null)
        {
            onEnterHandler();
        }
        this.transform.localScale = new Vector3(1.2f, 1.2f, 1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (onExitHandler != null)
        {
            onExitHandler();
        }
        this.transform.localScale = new Vector3(1f, 1f, 1);
    }

    public void setClickHandler(Action handler)
    {
        onClickHandler = handler;
    }
    public void setEnterHandler(Action handler)
    {
        onEnterHandler = handler;
    }
    public void setExitHandler(Action handler)
    {
        onExitHandler = handler;
    }

}

