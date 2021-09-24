using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Code © Bijan Pourmand
 * Authored 9/16/21
 * Script for GameController, handles gameplay bools and vars, etc.
 */

public static class GameController
{
    private static bool isActive = true;
    public static bool IsActive => isActive;

    public static Game_Canvas gameCanvas;
    static string fp_gameCanvas = "Canvas/Game_Canvas";

    public static float gameDeltaTime => Time.deltaTime * gameTimeScale;
    public static float gameTimeScale = 1;

    public static int goToID;

    public static int lastExitID;
    public static string lastMap;

    //Static Initializer
    static GameController()
    {
        Debug.Log("GameController initialized.");
        InitializeCanvas(new Scene(), LoadSceneMode.Single);
    }

    //Toggle for isActive; Spawns/Despawns game-related objects (e.g. game canvas)
    public static void SetIsActive(bool tog)
    {
        if (!isActive && tog == true)
        {
            Debug.Log("Enabling GameController.");
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneLoaded += InitializeCanvas;
            isActive = tog;
        }
        else if (isActive && tog == false)
        {
            Debug.Log("Disabling GameController.");
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded -= InitializeCanvas;
            isActive = tog;
        }

    }



    //Delegates for OnSceneLoaded
    public static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
            //1. Check for exit to spawn from.
            var exits = GameObject.FindObjectsOfType<Map_Exit>();
            Map_Exit spawn = null;
            foreach (Map_Exit mx in exits)
                if (mx.exitID == goToID)
                {
                    spawn = mx;
                    break;
                }
            //2. If no spawn was found..; else put player in spawn
            if (spawn == null)
            {
                Debug.Log("Spawn not found! Spawning player at (0,0,0)");
                Player.Current.transform.position = Vector3.zero;
            }
            else spawn.EnterAtSpawn(Player.Current);
     }

    public static void InitializeCanvas(Scene scene, LoadSceneMode mode)
    {
        //1. If game canvas is not present...
        if(GameObject.FindObjectOfType<Game_Canvas>() == null)
        {
            //2. Create game canvas
            GameObject.Instantiate(Resources.Load<GameObject>(fp_gameCanvas));
        }
        //3. Remove delegate
        SceneManager.sceneLoaded -= InitializeCanvas;
    }
        
    


    
}
