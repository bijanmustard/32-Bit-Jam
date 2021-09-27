using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Code © Bijan Pourmand
 * Authored 9/22/21
 * Script for Game_Canvas, UI for gameplay
 */

public class Game_Canvas : MonoBehaviour
{
    //Static canvas ref
    protected static Game_Canvas instance;
    public static Game_Canvas Instance;

    //Ref to UIs
    static HP_Bar playerHP, enemyHP;

    //Ref to tracked enemy (for HP bar)
    static Fighter trackedEnemy;

    bool dbg_tog;

    private void Awake()
    {
        //1. Set singleton
        if (instance != null && instance != this) Destroy(gameObject);
        else
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }

        //2. Set refs
        playerHP = transform.Find("Player_HP").GetComponent<HP_Bar>();
        enemyHP = transform.Find("Enemy_HP").GetComponent<HP_Bar>();
    }

    private void Start()
    {
        //0. Set render texture output
        GetComponent<Canvas>().worldCamera = RenderController.UICam;

        //1. Link HP Bar to player
        playerHP.AssignHPBar(Player.Current);
        //2. Set HP bar visibility
        playerHP.ToggleGameUI(true, false);
        enemyHP.ToggleGameUI(false, false);
    }

    //ShowPlayerHP shows player HP.
    public static void ShowEnemyHP(Fighter enemy)
    {
        //1. Set tracked enemy
        trackedEnemy = enemy;
        enemyHP.AssignHPBar(enemy);
        //2. Show bar
        enemyHP.ToggleGameUI((enemy != null ? true : false), false);
    }

    private void Update()
    {
       //1. If tracked enemy isn't null...
       if(trackedEnemy != null)
        {
            UnityEngine.Debug.Log(Vector3.Distance(Player.Current.transform.position, trackedEnemy.transform.position));
            //1a. If distance between tracked enemy and player is too far or is KO..
            if(trackedEnemy.IsKO || Vector3.Distance(Player.Current.transform.position, trackedEnemy.transform.position) > 20)
            {
                UnityEngine.Debug.Log("Disabling tracking");
                //1b. ..Disable tracking
                trackedEnemy = null;
                ShowEnemyHP(trackedEnemy);
            }
        }
    }

    void Debug()
    {
        //DEBUG
        if (Input.GetKeyDown(KeyCode.T))
        {
            dbg_tog = !dbg_tog;
            foreach (GameUI gui in GetComponentsInChildren<GameUI>())
            {
                gui.ToggleGameUI(dbg_tog, (Input.GetKey(KeyCode.LeftShift) ? false : true));
            }
        }
    }

}
