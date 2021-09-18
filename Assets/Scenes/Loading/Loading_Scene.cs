using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Code © Bijan Pourmand
 * Authored 9.17.21
 * Script for Loading scene
 */


public class Loading_Scene : MonoBehaviour
{

    public bool loaded = false;
    private void Awake()
    {
        //1. On awake, start loading scene async
        StartCoroutine(LoadNextScene());

    }

    /// <summary>
    /// IEnumerator for loading next scene. </summary> <returns></returns>
    IEnumerator LoadNextScene()
    {

        yield return new WaitForSeconds(3f);
        //1. Start async load
        AsyncOperation async = SceneManager.LoadSceneAsync(SceneController.nextScene);
        async.allowSceneActivation = false;

        

        //2. Wait for async to complete loading
        while (!async.isDone)
        {
            if (async.progress >= 0.9f) break;
            yield return null;
        } 
        //3. Once done, if using transitions, wait for fade out
        TransitionManager.TransitionOut(0);
        yield return null;
        while (TransitionManager.isTrans) yield return null;
        //4. Activate new scene
        TransitionManager.TransitionIn(0);
        async.allowSceneActivation = true;
        
    }
}
