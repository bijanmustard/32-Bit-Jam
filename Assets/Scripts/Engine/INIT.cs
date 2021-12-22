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

    static bool initialized = false;
    private void Awake()
    {
        if (!initialized)
        {
            //0. Disable cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            

            //1. Initialize MainController to init static controllers
            MainController.Initialize();
            //1a. RenderController
            RenderController.Initialize();
            //1b. SceneController
            SceneController.Initialize();
            //1c. GameController
            GameController.SetIsActive(false);
            //1d. TextboxManager
            TextboxManager.SetActive(true);
            //2. mark init bool
            initialized = true;
        }

    }


}
