using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authored 9/15/21
 * Script for doors
 */

public class Door_Interactable : Interactable
{
    Door_Dialogue dialogue;
    public bool isLocked = true;
    public bool requiresItem;
    [SerializeField]
    protected string required = "(none)";
    public string requiredItem => required;

    private void Awake()
    {
        dialogue = GetComponent<Door_Dialogue>();
    }

    public override void OnInteract()
    {
        //1. If door is locked..
        if (isLocked)
        {
            //2. If door requires item and player has item in inventory, unlock door
           Debug.Log(Inventory.KeyItems.Count);
            if (required != "(none)" && Inventory.HasKeyItem(required))
                TextboxManager.StartDialogue(dialogue, 7);
            //3. else display locked message
            else TextboxManager.StartDialogue(dialogue, 6);
        }
    }
}
