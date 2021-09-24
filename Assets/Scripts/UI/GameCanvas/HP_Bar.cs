using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Code © Bijan Pourmand
 * Authored 9/22/21
 * Script for HP Bar UI
 */

public class HP_Bar : GameUI
{

    //Fighter assigned to HP Bar
    Fighter myFighter;

    //UI Refs
    RectTransform rect;
    Text title;
    RectTransform bar;
    RectTransform barInner;
    RectTransform mask;

    [SerializeField]
    Vector2 showPos;
    [SerializeField]
    Vector2 hidePos;
    Coroutine moveIE;

    float hpPerc;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        bar = transform.Find("Bar").GetComponent<RectTransform>() ;
        mask = transform.GetComponentInChildren<RectMask2D>().GetComponent<RectTransform>();
        barInner = mask.GetChild(0).GetComponent<RectTransform>();
        title = transform.GetComponentInChildren<Text>();
    }


    private void Update()
    {
        //1. If fighter is set, display HP
        if(myFighter != null)
        {
            //2. Get percentage of HP
            hpPerc = myFighter.hp / myFighter.maxHP;
            //3. Set barInner pos
            barInner.anchoredPosition = new Vector2((mask.rect.width/2) * hpPerc, 0);
            barInner.localScale = new Vector3(hpPerc, 1, 1);

        }
    }

    //AssignHPBar is called to assign a fighter to an HP bar
    public void AssignHPBar(Fighter f)
    {
        //1. Set Fighter
        myFighter = f;
        //2. Set UI refs
        title.text = (f == null ? "" : f.name);
    }

    protected override void OnShow(bool useAnimation)
    {
        if (moveIE != null) StopCoroutine(moveIE);
        if (useAnimation) moveIE = StartCoroutine(MoveIE(true));
        
        else
        {
            title.enabled = true;
            bar.gameObject.SetActive(true);
            rect.anchoredPosition = showPos;
            
        }
    }

    protected override void OnHide(bool useAnimation)
    {
        if (moveIE != null) StopCoroutine(moveIE);
        if (useAnimation) moveIE = StartCoroutine(MoveIE(false));
        
        else {
            title.enabled = false;
            bar.gameObject.SetActive(false);
            rect.anchoredPosition = hidePos;
        }
    }

    IEnumerator MoveIE(bool show)
    {
        //0. Set up vars
        Vector2 dest = (show ? showPos : hidePos);
        //1. If enabling, enable text and bar
        if (show)
        {
            //1. Disable text and HPBar
            title.enabled = true;
            bar.gameObject.SetActive(true);
            isVisible = true;
        }
        //2. While rect isn't at destination pos..
        while(rect.anchoredPosition != dest)
        {
            //2a. Move towards pos
            rect.anchoredPosition = Vector2.MoveTowards(rect.anchoredPosition, dest, 5f);
            yield return null;
        }
        //3. If hiding, disable bar
        if (!show)
        {
            //1. Disable text and HPBar
            title.enabled = false;
            bar.gameObject.SetActive(false);
            isVisible = false;
        }
        //4. End IE
        moveIE = null;
    }
}
