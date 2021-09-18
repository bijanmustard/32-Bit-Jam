using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authored 9/17/21
 * Script for PauseController
*/


public class PauseController : MonoBehaviour
{
    //Ref to canvas obj
    public static PauseController instance;

    //Booleans
    public static bool canPause = false;
    private static bool isPause = false;
    public static bool IsPause => isPause;

    static PauseController()
    {

    }

    public static void PauseGame(bool tog)
    {
        isPause = tog;
        // 2. Enable canvas
        instance.GetComponent<Canvas>().enabled = tog;
        // 1. Set timescale to zero
        Time.timeScale = (tog ? 0 : 1);
    }

    private void Awake()
    {
        //1. If this isn't instance, destroy self
        if (instance != null && instance != this) Destroy(gameObject);
        //else set instance
        else instance = this;
        DontDestroyOnLoad(instance.gameObject);

        //2. Disable pause Canvas
        GetComponent<Canvas>().enabled = false;
    }
    private void Update()
    {
        // 1. If Pause button is pressed, toggle pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame(!isPause);
        }
    }


}
