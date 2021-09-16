using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Hittable : Hittable
{
    int HP = 3;

    public override void OnHit(Hitbox other)
    {
        Debug.Log("Hey " + other.gameObject.name + ", cut that out!");
        HP--;
        if (HP < 1)
        {
            other.contacts.Remove(gameObject);
            Destroy(gameObject);
        }
        transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
        
    }
}
