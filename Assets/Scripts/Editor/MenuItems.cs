using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/*
 * Code © Bijan Pourmand
 * Authored 9/26/21
 * Editor Script for Creation Menu, adds objects for easier level building.
 */
#if UNITY_EDITOR
public static class MenuItems
{


    //Prefab file paths
    static string fp_camZone = "Assets/Resources/Prefabs/CamZone.prefab";
    static string fp_exit = "Assets/Resources/Prefabs/Exit.prefab";
    static string fp_enemy = "Assets/Resources/Prefabs/Enemy.prefab";
    

    static void CreateObj(MenuCommand command, string fp)
    {
        GameObject zone = GameObject.Instantiate((GameObject)AssetDatabase.LoadAssetAtPath(fp, typeof(GameObject)));
        //ensure it gets reparented
        GameObjectUtility.SetParentAndAlign(zone, command.context as GameObject);
        //register creation in undo system
        Undo.RegisterCreatedObjectUndo(zone, "Create " + zone.name);
        Selection.activeObject = zone;
    }

    #region SpawnFunctions

    [MenuItem("GameObject/Level/Camera Zone", false, 10)]
    public static void CreateCamZone(MenuCommand command)
    {
        CreateObj(command, fp_camZone);
    }

    [MenuItem("GameObject/Level/Exit", false, 9)]
    public static void CreateExit(MenuCommand command)
    {
        CreateObj(command, fp_exit);
    }

    [MenuItem("GameObject/Level/Enemy", false, 8)]
    public static void CreateEnemy(MenuCommand command)
    {
        CreateObj(command, fp_enemy);
    }

    #endregion
}
#endif
