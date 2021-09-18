using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 * Code © Bijan Pourmand
 * Authored 4/6/21
 * Manager for Transitions. Contains ref's to all child transition objs and functions to use them.
 */

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance;
    public static string canvasPath = "Controllers/Transition_Canvas";

    //transition refs
    static Coroutine curRoutine;
    static Transition[] transitions;
    static public int curTransition;
    static protected bool isTransitioning;
    public static bool isTrans => transitions[curTransition].isTransitioning;

    static TransitionManager()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        
    }

    static void OnSceneLoaded(Scene name, LoadSceneMode mode)
    {
        //1. Spawn transition canvas
        if (instance == null)
        {
            instance = Instantiate(Resources.Load<GameObject>(canvasPath)).GetComponent<TransitionManager>();
            DontDestroyOnLoad(instance.gameObject);
            //2. Set transitions
            transitions = instance.GetComponentsInChildren<Transition>();
        }
        //2. Deattach from scene loaded
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Awake()
    {
        //1. If this isn't instance, destroy self
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;
        DontDestroyOnLoad(instance.gameObject);
        transitions = instance.GetComponentsInChildren<Transition>();

    }

    public static void LoadSceneWithTransition(string sceneName, bool useLoad)
    {
        // 1. If sceneIE is running, stop it
        if (curRoutine != null) instance.StopCoroutine(curRoutine);
        // 1a. Reset current transition
        TransitionStop();
        // 2. Start scene transition
        curRoutine = instance.StartCoroutine(SceneTransitionIE(sceneName, useLoad));
    }

    static IEnumerator SceneTransitionIE(string sceneName, bool useLoad)
    {
        // 1. Start transition out
        TransitionOut(0, 1);
        yield return null;
        //2. Wait for transition to finish
        while (isTrans) yield return null;
        Debug.Log("Transitioning out...");
        //3. LoadScene
        SceneController.LoadScene(sceneName, useLoad);
        //4. TransitionIn
        TransitionIn();
        //5. Wait for transIn before unlocking player moveDir
        while (isTrans) yield return null;
        Player.Current.move.LockDir(false);

    }
    

    // TransitionOut is called to transition out 
    public static void TransitionOut(int type, float speed = 0) {

        //0. SetActive
        transitions[curTransition].gameObject.SetActive(true);
        //1. fade out
        curTransition = type;
        if (speed > 0) transitions[type].Out(speed);
        else transitions[type].Out();
    }

    public static void TransitionOut(int type, Color c, float speed = 0)
    {
        //0. Set active
        transitions[curTransition].gameObject.SetActive(true);
        //1. fade out
        curTransition = type;
        if (speed > 0) transitions[type].Out(c,speed);
        else transitions[type].Out(c);
    }

    // TransitionIn is called to transition in
    public static void TransitionIn(int type = -1, float speed = 0)
    {
        
        //1. fade in
        int cur = curTransition;
        if (type > -1) cur = type;
        if (speed > 0) transitions[cur].In(speed);
        else transitions[cur].In();
    }

    //TransitionStop interrupts the current transition.
    public static void TransitionStop()
    {
        //1. Disable curTransition
        transitions[curTransition].gameObject.SetActive(false);
    }


}
