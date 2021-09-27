using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authored 9/17/21
 * Script for camera zones, which trigger state changes in camera controller.
 */

public class CamZone : MonoBehaviour
{

    public CameraSettings mySettings = new CameraSettings();

    //Gizmos vals
    Color unselectedColor = new Color(1,0,0,0.2f);
    Color selectedColor = new Color(0, 1, 0, 0.2f);

    private void OnDrawGizmos()
    {
        Gizmos.color = unselectedColor;
        Gizmos.matrix = transform.localToWorldMatrix;
        Collider col = GetComponent<Collider>();
        if (col.GetType() == typeof(BoxCollider))
        {
            //Gizmos.DrawCube(Vector3.zero, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, col.bounds.size);
        }
        else if (col.GetType() == typeof(SphereCollider))
        {
           //    Gizmos.DrawSphere(Vector3.zero, 1);
            Gizmos.DrawWireSphere(Vector3.zero, 1);
        }
        }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = selectedColor;
        Gizmos.matrix = transform.localToWorldMatrix;
        Collider col = GetComponent<Collider>();
        if (col.GetType() == typeof(BoxCollider)) Gizmos.DrawCube(transform.InverseTransformPoint(col.bounds.center), col.bounds.size);
        else if (col.GetType() == typeof(SphereCollider)) Gizmos.DrawSphere(Vector3.zero, 1);
        //Draw sticky point
        Gizmos.color = new Color(0, 0, 1, 0.5f);
        Gizmos.matrix = Matrix4x4.identity;
        if(mySettings.staticPoint != null) Gizmos.DrawSphere((Vector3)mySettings.staticPoint, 2f);
    }

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //1. If player enters trigger...
        if(other.gameObject.layer == 11 && other.tag == "Player")
        {
            Debug.Log($"Player entered {name}");
            // 2. Update active worldCam
            WorldCamera wCam = RenderController.worldCam.GetComponent<WorldCamera>();
            if (mySettings.followPlayer) mySettings.target = Player.Current.transform;
            wCam.myCamSettings = mySettings;
            
        }
    }

}
