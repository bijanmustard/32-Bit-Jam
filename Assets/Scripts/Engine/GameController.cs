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

    //Int representing player character
    //0 = Chas, 1 = Liz
    public static int playerCharacter = 1;
    static string fp_players = "Prefabs/Players/";
    static GameObject prefab_Chas, prefab_Liz;

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
        Debug.Log("GameController initializing.");
        //1. Load player prefabs
        prefab_Chas = Resources.Load<GameObject>(fp_players + "Player_Chas");
        prefab_Liz = Resources.Load<GameObject>(fp_players + "Player_Liz");
    }

    //Toggle for isActive; Spawns/Despawns game-related objects (e.g. game canvas)
    public static void SetIsActive(bool tog)
    {
        if (!isActive && tog == true)
        {
            Debug.Log("Enabling GameController.");
            //1. Add sceneLoaded event to spawn canvas.
            SceneManager.sceneLoaded += InitializeCanvas;
            //2. Add OnSceneLoaded
            SceneManager.sceneLoaded += OnSceneLoaded;
            isActive = tog;
        }
        else if (isActive && tog == false)
        {
            Debug.Log("Disabling GameController.");
            //1. Add sceneLoaded event to despawn canvas.
            SceneManager.sceneLoaded += DestroyCanvas;
            //2. Add OnSceneLoaded
            SceneManager.sceneLoaded -= OnSceneLoaded;
            isActive = tog;
        }

    }

    //SetCharacter sets the character.
    public static void SetCharacter(int player)
    {
        playerCharacter = Mathf.Clamp(player, 0, 1);
    }

    //Delegates for OnSceneLoaded
    public static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (IsActive)
        {
            //0. If not in loading scene..
            if (Object.FindObjectOfType<Loading_Scene>() == null)
            {
                //Instantiate GameCanvas
                InitializeCanvas(new Scene(), LoadSceneMode.Single);
                //Instantiate player
                GameObject.Instantiate((playerCharacter == 0 ? prefab_Chas : prefab_Liz));
                Debug.Log("Player instantiated");
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
        }
     }

    /// <summary>
    /// SceneLoaded event to initialize canvas on next scene load. 
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    static void InitializeCanvas(Scene scene, LoadSceneMode mode)
    {
        if (Object.FindObjectOfType<Loading_Scene>() == null)
        {
            if (gameCanvas == null)
            {
                //1. If game canvas is not present...
                if (GameObject.FindObjectOfType<Game_Canvas>() == null)
                {
                    //2. Create game canvas
                    GameObject.Instantiate(Resources.Load<GameObject>(fp_gameCanvas));
                }
                //3. Else, set gameCanvas to reference
                else gameCanvas = Object.FindObjectOfType<Game_Canvas>();
            }

            //4. Remove self from onSceneLoaded
            SceneManager.sceneLoaded -= InitializeCanvas;
        }
    }

    static void DestroyCanvas(Scene scene, LoadSceneMode mode)
    {
        //1. Destroy game canvas
        if (gameCanvas != null) GameObject.Destroy(gameCanvas.gameObject);
        gameCanvas = null;
        //2. Remove self from onSceneLoaded
        SceneManager.sceneLoaded -= DestroyCanvas;
    }
        
    


    
}
