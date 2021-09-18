using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authored 9/16/21
 * Script for MainController, handles applicaiton-related funcs
 */

public class MainController : MonoBehaviour
{
    public static GameMode curMode;
    
}

public enum GameMode
{
    Menu,
    Game
}
