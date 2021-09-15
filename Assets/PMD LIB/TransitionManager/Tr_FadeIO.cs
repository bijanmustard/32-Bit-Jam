using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Code © Bijan Pourmand
 * Authored 4/6/21
 * Script for FadeIO transition obj. 
 */

public class Tr_FadeIO : Transition
{
    Image fadeImg;
    Color fadeColor = Color.black;
    float fadeSpeed;


    protected override void Awake()
    {
        base.Awake();
        if (fadeImg == null) fadeImg = GetComponent<Image>();
        if (anim == null) anim = GetComponent<Animator>();
    }


    public override void In()
    {
        //Fade in
        anim.SetTrigger("In");
    }
    public override void In(float speed)
    {
        //1. set parameter values
        fadeSpeed = speed;
        anim.speed = fadeSpeed;
        //2. fade in
        anim.SetTrigger("In");
    }

    public override void Out()
    {
        //1. set up vars
        fadeSpeed = 1;
        anim.speed = fadeSpeed;
        fadeColor = Color.black;
        //2. if fadeColor != cur color, change it
        if (!fadeImg.color.Equals(fadeColor)) fadeImg.color = fadeColor;
        //3. fade out
        anim.SetTrigger("Out");
    }


    public override void Out(Color c, float speed = 1) 
    {
        //1. Set up parameter values
        fadeSpeed = speed;
        anim.speed = fadeSpeed;
        fadeColor = c;
        //2. if fadeColor != cur color, change it
        if (!fadeImg.color.Equals(fadeColor)) fadeImg.color = fadeColor;
        //3. fade out
        anim.SetTrigger("Out");
    }

    public override void Out(float speed)
    {
        //1. Set up parameter values
        fadeSpeed = speed;
        anim.speed = fadeSpeed;
        fadeColor = Color.black;
        //2. if fadeColor != cur color, change it
        if (!fadeImg.color.Equals(fadeColor)) fadeImg.color = fadeColor;
        //3. fade out
        anim.SetTrigger("Out");
    }
}
