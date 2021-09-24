using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Code © Bijan Pourmand
 * Authored 9/22/21
 * Script for GameUI, UI exclusive to gameplay. 
 */


public class GameUI : MonoBehaviour
{
    protected virtual void OnShow(bool useAnimation) { }
    protected virtual void OnHide(bool useAnimation) { }

    protected bool isVisible;
    public bool IsVisible => isVisible;
    
    public void ToggleGameUI(bool toggle, bool useAnimation)
    {
        if (toggle) OnShow(useAnimation);
        else OnHide(useAnimation);
    }
}
