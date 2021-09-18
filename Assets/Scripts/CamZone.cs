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
    public Vector3 minBounds, maxBounds;

    public Vector3 eulerRotation;
    public Vector3 pivotFwd;

    public CamMode myCamMode;

    [SerializeField]
    Vector3 stickyPoint;
    [SerializeField]
    bool useSticky;
    [SerializeField]
    bool isLookAt;


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
        Gizmos.DrawSphere((Vector3)stickyPoint, 2f);
         
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //1. If player enters trigger...
        if(other.gameObject.layer == 11 && other.tag == "Player")
        {
            Debug.Log($"Player entered {name}");
            // 2. Update Camera_Controller
            // 2a. Set mode
            Camera_Controller.mode = myCamMode;
            //2b. Set sticky point
            Camera_Controller.useSticky = useSticky;
            if (useSticky) Camera_Controller.curSticky = stickyPoint;
            else Camera_Controller.curSticky = null;
            //2c. Set look at
            Camera_Controller.isLookAt = isLookAt;
            // 2d. Set rotation
            Camera_Controller.camRotation = Quaternion.Euler(eulerRotation);
            // 2d. Set pivot fwd
            Camera_Controller.pivotFwd = pivotFwd.normalized;

           
        }
    }

}
