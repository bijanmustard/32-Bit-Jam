using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authored 9/15/21
 * Script for player inventory
 */

public class Inventory : MonoBehaviour
{
    //Key Items
    public static Dictionary<string, bool> KeyItems = new Dictionary<string, bool>
    {
        {"Test", false },
        {"Test2", false },
        {"Test3", true }
    };

    //Consumable Items
    public Dictionary<string, int> Items = new Dictionary<string, int>();


    ///<summary>
    ///Sets whether or not a key item is aquired.
    ///</summary>
    public static void SetKeyItem(string item, bool tog)
    {
        //1. If key item is valid, set value
        if (KeyItems.ContainsKey(item)) KeyItems[item] = tog;
        Debug.LogFormat("{0} {1} from inventory.", (tog ? "Adding" : "Removing"), item);
    }

    /// <summary>
    /// Returns whether or not the given key item is in inventory.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public static bool HasKeyItem(string item)
    {
        bool val = false;
        //1. If key item is in inventory, get value
        if (KeyItems.ContainsKey(item)) KeyItems.TryGetValue(item, out val);
        //2. return value
        return val;
        
    }
}
