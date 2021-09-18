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

    public bool isSpawning = false;
    [SerializeField]
    protected Vector3 exitPt = Vector3.zero;
    protected float exitSpeed = 2f;
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

    void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        //1. Increment spawn timer
        if (isSpawning)
        {
          
            //1b. Get player vel to spawn point
            Vector3 worldExit = transform.TransformPoint(exitPt);
            Vector3 moveTo = Vector3.MoveTowards(player.transform.position, worldExit, exitSpeed *  Time.deltaTime);
            player.move.velOverride = (moveTo - player.transform.position);
            player.move.velOverride.y = player.move.vertVel;
            //2. Once player is at posiiton, end spawn
            if (Mathf.Abs(player.transform.position.x - worldExit.x) < 0.11f && 
                Mathf.Abs(player.transform.position.z - worldExit.z) < 0.11f)
            {
                isSpawning = false;
                player.move.isVelOverride = false;
                player.move.velOverride = Vector3.zero;
                player.move.LockDir(false);
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
        //1. Set player to exit position
        player.transform.position = transform.position;
        //2. Set moveDir to local fwd
        player.transform.forward = transform.forward;
        player.move.isVelOverride = true;

        //3. move outwards for one second (see Update)

        //4. Re-enable player movement
    }


}
