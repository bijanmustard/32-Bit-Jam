using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authroed 9/15/21
 * Hitbox script to be attached to left hands.
 */

public class Hitbox : MonoBehaviour
{
    bool isContact;

    public bool IsContact => isContact;
    public Dictionary<GameObject, Vector3> contacts = new Dictionary<GameObject,Vector3>();
    public Hitbox_Controller myHitController;

    private void Awake()
    {
        myHitController = GetComponentInParent<Hitbox_Controller>();
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (!PauseController.IsPause && !GetComponentInParent<Fighter>().isFreeze)
        {

            //1. If other object meets criteria, add to contacts
            //1a. If mask contains object's layer..
            if (myHitController.mask == (myHitController.mask | (1 << other.gameObject.layer)))
            {
                if (other.gameObject.tag != "Hitbox")
                {
                    contacts.Add(other.gameObject, other.gameObject.transform.position);
                    //2. Call onHit
                    //If hit fighter, call current action value
                    Fighter fAct = other.gameObject.GetComponent<Fighter>();
                    if (fAct != null) myHitController.SignalHit(this, fAct);
                    else myHitController.SignalHit(this, other.gameObject.GetComponent<Hittable>());
                }
            }
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        //1. If in contacts, remove self
        if (contacts.ContainsKey(other.gameObject)) contacts.Remove(other.gameObject);
    }

    private void Update()
    {
        // 1. If there are contacts, set isContact
        if (contacts.Count > 0) isContact = true;
        else isContact = false;
    }

}
