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
    private static bool isActive = false;
    public static bool IsActive => isActive;
    public static float gameDeltaTime => Time.deltaTime * gameTimeScale;
    public static float gameTimeScale = 1;

    public static int goToID;

    public static int lastExitID;
    public static string lastMap;

    //Static Initializer
    static GameController()
    {
        //3. Add OnSceneLoaded to onSceneLoaded
        SetIsActive(true);
    }


    //Delegate for OnSceneLoaded
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
        
    //Toggle for isActive
    public static void SetIsActive(bool tog)
    {
        Debug.Log($"Setting active.. {isActive}, {tog}");
        if (!isActive && tog == true)
        {
            Debug.Log("Enabling gameController");
            SceneManager.sceneLoaded += OnSceneLoaded;
            isActive = tog;
        }
        else if (isActive && tog == false)
        {
            Debug.Log("Disabling gameController");
            SceneManager.sceneLoaded -= OnSceneLoaded;
            isActive = tog;
        }
        
    }


    
}
