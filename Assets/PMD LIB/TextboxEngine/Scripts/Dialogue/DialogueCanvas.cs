using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Code © Bijan Pourmand
 * AUthored 6/26/21
 * Script controller for Dialogue Canvas
 */

public class DialogueCanvas : MonoBehaviour
{
    private static DialogueCanvas instance;
    public Image BG, character;
    Animator anim;
    //DiaSprites curSprites;
    public bool isCanvas;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        anim = GetComponent<Animator>();
        BG = transform.Find("BG").GetComponent<Image>();
        character = transform.Find("NPC").GetComponent<Image>();
        BG.gameObject.SetActive(false);
        character.gameObject.SetActive(false);

    }

    //EnableCanvas fades in the canvas.
    public void EnableCanvas()
    {
        isCanvas = true;
        //1. Set initial sprite
        //curSprites = sprites;
        //character.sprite = curSprites.expressions[0];
        //2. Fade in 
        anim.SetTrigger("BG_FI");
        anim.SetTrigger("NPC_FI");
    }

    //DisableCanvas fades out the canvas.
    public void DisableCanvas()
    {
        //fade out vars
        anim.SetTrigger("BG_FO");
        anim.SetTrigger("NPC_FO");
        //curSprites = null;
        isCanvas = false;
    }

    //ChangeExpression is called by TextboxManager to change the expression.
    //public void ChangeExpression(int idx)
    //{
    //    if(idx < curSprites.expressions.Length)
    //    {
    //        character.sprite = curSprites.expressions[idx];
    //    }
    //}

}
