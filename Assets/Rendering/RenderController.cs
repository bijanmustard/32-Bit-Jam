using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 * Code © Bijan Pourmand
 * Authored 9/24/21
 * Script for RenderController. Handles RenderCanvas and different render texture layers.
 */

public static class RenderController
{

    #region Refs & Vars
    private static Canvas canvas;

    public static Camera outputCam;
    public static Camera worldCam;
    public static Camera UICam;
    public static Camera textboxCam;

    static string fp_rendCanvas = "Rendering/Render_Canvas";
    static string fp_worldCam = "Rendering/Cameras/WorldCamera";
    static string fp_UICam = "Rendering/Cameras/UICamera";
    static string fp_textboxCam = "Rendering/Cameras/TextboxCamera";


    static public RenderTexture rt_worldCam;
    static public RenderTexture rt_UICam;
    static public RenderTexture rt_textboxCam;

    //Output resolution
    public static Vector2Int outputResolution = new Vector2Int(320, 240);

    #endregion

    static RenderController()
    {
        //Initialize();
    }

    public static void Initialize()
    {
        Debug.Log("Initializing RenderController");
        //1. Set cameras
        //1a. World Cam
        if(GameObject.FindObjectOfType<WorldCamera>() != null) worldCam = GameObject.FindObjectOfType<WorldCamera>().GetComponent<Camera>();
        else worldCam = GameObject.Instantiate(Resources.Load<GameObject>(fp_worldCam)).GetComponent<Camera>();
        Object.DontDestroyOnLoad(worldCam.gameObject);

        //1b. UICamera
        if(GameObject.FindGameObjectWithTag("UICamera") != null)
            UICam = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
       else UICam = Object.Instantiate(Resources.Load<GameObject>(fp_UICam)).GetComponent<Camera>();
        GameObject.DontDestroyOnLoad(UICam.gameObject);

        ////1c. TextboxCamera
        //if (GameObject.FindGameObjectWithTag("TextboxCamera") != null)
        //    textboxCam = GameObject.FindGameObjectWithTag("TextboxCamera").GetComponent<Camera>();
        //else textboxCam = Object.Instantiate(Resources.Load<GameObject>(fp_textboxCam)).GetComponent<Camera>();
        //Object.DontDestroyOnLoad(textboxCam.gameObject);

        //2. Set canvas
        //2a. Find or spawn canvas reference
        if (Object.FindObjectOfType<Render_Canvas>() != null)
            canvas = Object.FindObjectOfType<Render_Canvas>().GetComponent<Canvas>();
        else canvas = Object.Instantiate(Resources.Load<GameObject>(fp_rendCanvas)).GetComponent<Canvas>();
        
        //3. Set sceneloaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
        outputCam = worldCam;
        canvas.worldCamera = outputCam;

        //4. Update render textures to match output resolution
        rt_worldCam.width = outputResolution.x;
        rt_worldCam.height = outputResolution.y;
        rt_UICam.width = outputResolution.x;
        rt_UICam.height = outputResolution.y;
        

    }

    static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //1. Reset output to camera main
        outputCam = worldCam;
        canvas.worldCamera = outputCam;
    }

}
