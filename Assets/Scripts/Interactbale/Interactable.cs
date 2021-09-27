using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Code © Bijan Pourmand
 * Authored 9/15/21
 * Base script for interactable items.
 */

public abstract class Interactable : MonoBehaviour
{
    public bool canInteract = true;


    /// <summary>
    /// Function to be called when interacted with by player.
    /// </summary>
    public abstract void OnInteract();

    protected void OnTriggerStay(Collider other)
    {
        if (canInteract)
        {
            if (other.tag == "Player")
            {
                if (Input.GetKeyDown(KeyCode.LeftControl))
                    OnInteract();
            }
        }
    }
}
