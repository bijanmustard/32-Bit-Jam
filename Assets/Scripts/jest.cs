using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jest : MonoBehaviour
{
    Rigidbody rb;
    bool jump;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();   
    }

    // Update is called once per frame
    void Update()
    {
        jump = Input.GetKeyDown(KeyCode.Space);
    }

    private void FixedUpdate()
    {
        if (jump)
        {
            rb.velocity = new Vector3(0, 10, 0);
        }
    }
}
