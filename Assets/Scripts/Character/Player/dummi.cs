using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dummi : MonoBehaviour
{
    bool timescale = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        Debug.Log("I'm still being called!");
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            timescale = !timescale;
            Player.Current.SetFreeze(!timescale);
            //Time.timeScale = timescale ? 1: 0;
        }
    }
}
