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
   

    public bool isSpawning = false;
    [SerializeField]
    protected Vector3 exitPt = Vector3.zero;
    protected float exitSpeed = 4f;
    protected float spawnTimer = 0;

    //Idx for thismap_exit
    public int exitID;
    //Name of scene to go to
    public string goToMap;
    //Idx of map exit in new map
    public int goToIdx = 0;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 1, 0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(exitPt), 0.2f);
        Gizmos.DrawLine(transform.position, transform.TransformPoint(exitPt));
    }


    private void Update()
    {
        //1. Increment spawn timer
        if (isSpawning)
        {
          
            //1b. Get player vel to spawn point
            Vector3 worldExit = transform.TransformPoint(exitPt);
            Vector3 moveTo = Vector3.MoveTowards(Player.Current.transform.position, worldExit, exitSpeed *  Time.deltaTime);
            Player.Current.move.moveDirOverride = (moveTo - Player.Current.transform.position).normalized;
            Player.Current.move.moveDirOverride.y = 0;
            //Player.Current.move.velOverride.y = Player.Current.move.vertVel;
            //2. Once player is at posiiton, end spawn
            if (Mathf.Abs(Player.Current.transform.position.x - worldExit.x) < 0.11f && 
                Mathf.Abs(Player.Current.transform.position.z - worldExit.z) < 0.11f)
            {
                isSpawning = false;
                Player.Current.move.isMoveDirOverride = false;
                Player.Current.move.moveDirOverride = Vector3.zero;
                Player.Current.move.LockDir(false);
                Player.Current.move.useCameraTransform = true;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        // 1. If player enters trigger...
        if (other.gameObject.layer == 11 && other.tag == "Player")
        {
            //1a. If map has started...
            if (!isSpawning)
            {
                Debug.Log($"Exiting map! isSpawning: {isSpawning}");
                //2. Prepare for scene transition
                //2a. Set refs to self vars in GameController
                GameController.lastExitID = exitID;
                GameController.lastMap = SceneController.GetCurrentScene().name;
                //2b. Set goToID
                GameController.goToID = goToIdx;
                //2c. Lock player movement
                Player.Current.move.LockDir(true);
                //3. If scene exists, go to scene
                if (Array.Exists(SceneController.scenes, s => s == goToMap))
                {
                    SceneController.GoToScene(goToMap, true);
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
        Debug.Log("Entering at spawn"); 
        //1. Set player to exit position
        player.transform.position = transform.position;
        //2. Set moveDir to local fwd
        player.transform.forward = transform.forward;
        player.move.isMoveDirOverride = true;
        player.move.overrideSpeed = exitSpeed;
        player.move.useCameraTransform = false;

        //3. move outwards for one second (see Update)

        //4. Re-enable player movement
    }


}
