using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authroed 9/24/21
 * Script for RenderCanvas, used to house render texture layers
 */

public class Render_Canvas : MonoBehaviour
{
    private Render_Canvas instance;
    private void Awake()
    {
        //1. Set singleton
        if (instance != null && instance != this) Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
