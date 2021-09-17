using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Code © Bijan Pourmand
 * Authored 9/16/21
 * Script for Map_Exit triggers
 */

public class Map_Exit : MonoBehaviour
{
    //Ref to player
    Player player;

    protected bool isSpawning = false;
    //Idx for thismap_exit
    public int exitID;
    //Name of scene to go to
    public string goToMap;
    //Idx of map exit in new map
    public int goToIdx = 0;

    void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 1. If player enters trigger...
        if(other.gameObject.layer == 11 && other.tag == "Player")
        {
            //1a. If map has started...
            if (!isSpawning)
            {
                //2. Prepare for scene transition
                //2a. Set refs to self vars in GameController
                GameController.lastExitID = exitID;
                GameController.lastMap = SceneController.GetCurrentScene().name;
                //2b. Set goToID
                GameController.goToID = goToIdx;
                //2c. Lock player movement
                player.move.LockDir(true);
                //3. If scene exists, go to scene
                if (Array.Exists(SceneController.scenes, s => s == goToMap))
                {
                    SceneController.GoToScene(goToMap);
                }
                //4. Else if scene doesn't exist, reload map
                else
                {
                    Debug.Log($"Map {goToMap} doesn't exist!");
                    GameController.goToID = exitID;
                    SceneController.GoToScene(SceneController.GetCurrentScene().name);
                }
            }
                
        }
    }

    //PlayerExit is called by GameController to have the player walk out from a given exit.
    public void EnterAtSpawn(Player player)
    {
        //1. Start spawn instructions
        isSpawning = true;
        //1. Set player to exit position
        player.transform.position = transform.position;
        //2. Set moveDir to local fwd
        //<-- TODO: Implement manual velocity set in Character_Move?
        //3. move outwards for one second
        //4. Re-enable player movement
    }


}
