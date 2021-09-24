using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authored 9/23/21
 * INIT script to be called when booting game. Initializes controllers.
 */

public class INIT : MonoBehaviour
{
    private void Awake()
    {
        //1. Initialize MainController to init static controllers
        MainController.Initialize();
    }


}
