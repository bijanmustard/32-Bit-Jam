using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authored 9/26/21
 * Script for interactbale events
 */

public abstract class InteractableEvent : MonoBehaviour
{
    //Ref to interactable
    protected Interactable myInteractable;

    private void Awake()
    {
        myInteractable = GetComponent<Interactable>();
    }

    //Function for event
    protected abstract void InteractEvent();


}
