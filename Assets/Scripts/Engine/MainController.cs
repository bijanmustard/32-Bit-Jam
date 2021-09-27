using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authored 9/16/21
 * Script for MainController, handles applicaiton-related funcs
 */

public static class MainController
{
    public static GameMode curMode;

    //Intialize is called to Initialize the main controller.
    public static void Initialize()
    {
    

    }

    //StartGame is called to start the game.
    public static void StartGame()
    {
        //1. Set GameMode
        curMode = GameMode.Game;
        //2. Activate GameController
        GameController.SetIsActive(true);
    }
}

public enum GameMode
{
    Menu,
    Game
}
