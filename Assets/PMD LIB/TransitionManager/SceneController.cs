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

public class SceneController : MonoBehaviour
{

    private static SceneController instance;
    static Coroutine curRoutine;

    public static string[] scenes;


    private void Awake()
    {
        if (instance != null && instance != this) Destroy(this);
        else { instance = this; }

        // 2. Set scene array
        scenes = new string[SceneManager.sceneCountInBuildSettings];
        Debug.Log($"Scenes: {scenes.Length}");
        for (int i = 0; i < scenes.Length; i++)
        {
            
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            scenes[i] = System.IO.Path.GetFileNameWithoutExtension(path);
            Debug.Log(scenes[i]);
        }
    }

    // GoToScene() is called to go to a specified scene.
    public static void GoToScene(int idx)
    {
        //1. Stop current coroutine
        if (curRoutine != null)
        {
            instance.StopCoroutine(curRoutine);

        }
        //2.Reset transitioning bool
        curRoutine = instance.StartCoroutine(GoToIE(idx));
    }

    public static void GoToScene(string str)
    {
        //1. Stop current coroutine
        if (curRoutine != null)
        {
            instance.StopCoroutine(curRoutine);

        }
        //2.Reset transitioning bool
        curRoutine = instance.StartCoroutine(GoToIE(str));
    }

    //GetCurScene returns the current scene
    public static Scene GetCurrentScene()
    {
        return SceneManager.GetActiveScene();
    }

    //GoToIE() is the IE for GoToScene.
    static IEnumerator GoToIE(int idx)
    {
        //Player_Movement move = FindObjectOfType<Player_Movement>();
        //if (move != null) move.LockMovement(true);

        //1. start out transition
        TransitionManager.TransitionOut(0, 1);
        yield return null;
        //2. Wait for transition to finish
        while (TransitionManager.isTrans) yield return null;
        Debug.Log("Loading scene....");
        //3. LoadScene
        SceneManager.LoadScene(idx);
        //4. TransitionIn
        TransitionManager.TransitionIn();
        //5. Wait for transIn before unlocking player moveDir
        while (TransitionManager.isTrans) yield return null;
        Player.Current.move.LockDir(false);
    }

    //GoToIE() is the IE for GoToScene.
    static IEnumerator GoToIE(string str)
    {

        //1. start out transition
        TransitionManager.TransitionOut(0, 1);
        yield return null;
        //2. Wait for transition to finish
        while (TransitionManager.isTrans) yield return null;
        Debug.Log("Loading scene....");
        //3. LoadScene
        SceneManager.LoadScene(str);
        //4. TransitionIn
        TransitionManager.TransitionIn();
        //5. Wait for transIn before unlocking player moveDir
        while (TransitionManager.isTrans) yield return null;
        Player.Current.move.LockDir(false);


    }

}
