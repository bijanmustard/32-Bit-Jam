using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*
 * Code © Bijan Pourmand
 * Authored 9/25/21
 * Script for TitleScreen
 */

public class TitleScreen : MonoBehaviour
{
    //Main canvas
    Canvas myCanvas;
    //Main Menu
    GameObject[] menus = new GameObject[4];
    SelectableOption[] menuItems;

    //Ref to current menu
    //0 == main, 1 == controls, 2 == settings
    int curMenu = 0;

    private void Awake()
    {
        //1. Set refs
        myCanvas = GetComponent<Canvas>();
        menus[0] = myCanvas.transform.Find("MainMenu").gameObject;
        menuItems = menus[0].GetComponentsInChildren<SelectableOption>();
        menus[1] = myCanvas.transform.Find("ControlsMenu").gameObject;
        menus[1].SetActive(false);
        menus[2] = myCanvas.transform.Find("CreditsMenu").gameObject;
        menus[2].SetActive(false);
        menus[3] = myCanvas.transform.Find("CharacterMenu").gameObject;
        menus[3].SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        //1. Set canvas output to UI
        myCanvas.worldCamera = RenderController.UICam;
    }

 

    private void Update()
    {
        Debug.Log(EventSystem.current.currentSelectedGameObject);
    }

    #region MenuItem Functions
    //Start Game loads the game.
    public void StartGame(int player)
    {
        //1. Set mode to game
        MainController.curMode = GameMode.Game;
        //2. Enable GameController
        GameController.SetIsActive(true);
        //2a. Set player
        GameController.SetCharacter(player);
        //3. Load game scene
        SceneController.GoToScene("Sewers_1", true);
    }

    //Controls shows the controls panel
    public void Controls()
    {
        GoToMenu(1);
    }

    //Settings shows the settings menu.
    public void Settings()
    {
        GoToMenu(2);
    }

    //GoToMenu closes the current menu and opens the destination menu.
    public void GoToMenu(int menu)
    {
        //1. If not switching to current menu..
        if (menu != curMenu)
        {
            //2. Set curMenu to false
            menus[curMenu].SetActive(false);
            //3. Enable new menu
            curMenu = menu;
            menus[curMenu].SetActive(true);
            //4. Set first selected to first selectable in new menu
            Selectable newSelected = menus[curMenu].GetComponentInChildren<Selectable>();
            if(newSelected != null) EventSystem.current.SetSelectedGameObject(newSelected.gameObject);
            
            
        }
    }
    #endregion
}
