using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authored 9/15/21
 * Base script for objects that react to getting attacked.
 */

public abstract class Hittable : MonoBehaviour
{
    private void Update()
    {
        
    }

    public abstract void OnHit(Hitbox other);
}
