using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Code © Bijan Pourmand
 * Authored 9/16/21
 * Script for GameController, handles gameplay bools and vars, etc.
 */

public class GameController : MonoBehaviour
{
    public static int goToID;

    public static int lastExitID;
    public static string lastMap;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //Delegate for OnSceneLoaded
    public static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        //1. Check for exit to spawn from.
        var exits = FindObjectsOfType<Map_Exit>();
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
}
