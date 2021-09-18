using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authored 9/15/21
 * Script for pick ups
 */

public class KeyItem_Interactable : Interactable
{
    [SerializeField]
    string itemName;
    Dialogue dialogue;

    private void Awake()
    {
        dialogue = GetComponent<Dialogue>();
    }

    public override void OnInteract()
    {
        Inventory.SetKeyItem(itemName, true);
        TextboxManager.StartDialogue(dialogue);
    }
}
