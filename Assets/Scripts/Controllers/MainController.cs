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
}

public enum GameMode
{
    Menu,
    Game
}
