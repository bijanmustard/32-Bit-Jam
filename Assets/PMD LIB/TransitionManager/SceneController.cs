using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Code © Bijan Pourmand
 * Authored 4/9/21
 * Static controller for scene management. Works with transition manager.
 * REVISED 9/13/21 - Converted to static class
 */

public static class SceneController
{

    public static string[] scenes;
    public static string nextScene;

    public static bool useTransitions = true;


    static SceneController()
    {
        // 2. Set scene array
        scenes = new string[SceneManager.sceneCountInBuildSettings];
        for (int i = 0; i < scenes.Length; i++)
        {        
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            scenes[i] = System.IO.Path.GetFileNameWithoutExtension(path);
        }
    }


    public static void GoToScene(string str, bool useLoad = true)
    {
        // 1. Double check to see if string is valid
        if(System.Array.Exists(scenes, s => s == str)){

            //1a. Set goToScene
            nextScene = str;

            //2. If using transitions, use transition scene load
            if (TransitionManager.instance != null && useTransitions)
                TransitionManager.LoadSceneWithTransition(nextScene, useLoad);
            //3. Else, manual scene load
            else LoadScene(nextScene, useLoad);
        }
    }

    /// <summary> Manual scene load that skips transitions. </summary>
    /// <param name="sceneName"></param>
    /// <param name="useLoad"></param>
    public static void LoadScene(string sceneName, bool useLoad)
    {
        //3a. If load, go to loading screen; else go straight to scene
        Debug.Log((useLoad ? "Going to loading scene" : "Skipping load"));
        if (useLoad) SceneManager.LoadScene("Loading");
        else SceneManager.LoadScene(sceneName);
    }


    //GetCurScene returns the current scene
    public static Scene GetCurrentScene()
    {
        return SceneManager.GetActiveScene();
    }


}
