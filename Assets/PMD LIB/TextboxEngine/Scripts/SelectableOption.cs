using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
 * Code © Bijan Pourmand
 * Authored 9/14/21
 * Listener script for selectable options in TextboxEngine.
 */

public class SelectableOption : Selectable
{

    public Action onSelectedEvent;
    bool isSelected;

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        isSelected = true;
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        isSelected = false;
    }

    private void Update()
    {
        // 1. If is selected and event system's confirm input is pressed, press option.
        if(isSelected && Input.GetButtonDown(EventSystem.current.GetComponent<StandaloneInputModule>().submitButton))
            {
                Debug.Log(name + " option was pressed");
                //1. Call delegate event
                onSelectedEvent.Invoke();
            }
    }


}
