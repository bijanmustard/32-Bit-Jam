using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authored 6/25/21
 * Script for Dialogue NPC events. Contains text asset for dialogue, events
 * Supports up to 10 Dialogue Events.
 */

public abstract class Dialogue : MonoBehaviour
{

    public TextAsset dialogueScript;

    [SerializeField]
    protected abstract string filename { get; }

    private void Awake()
    {
        //1. Load .txt file from filename
        dialogueScript = Resources.Load<TextAsset>(string.Format("Texts/{0}", filename));
    }

    //Event() calls the specified event and ref to reader that called it. Supports up to 21 events.
    public void Event(int i, TextReader reader)
    {
        // 1. Call event
        switch (i)
        {
            case 0:
                DEvent0(reader);
                break;
            case 1:
                DEvent1(reader);
                break;
            case 2:
                DEvent2(reader);
                break;
            case 3:
                DEvent3(reader);
                break;
            case 4:
                DEvent4(reader);
                break;
            case 5:
                DEvent5(reader);
                break;
            case 6:
                DEvent6(reader);
                break;
            case 7:
                DEvent7(reader);
                break;
            case 8:
                DEvent8(reader);
                break;
            case 9:
                DEvent9(reader);
                break;
            case 10:
                DEvent10(reader);
                break;
            case 11:
                DEvent11(reader);
                break;
            case 12:
                DEvent12(reader);
                break;
            case 13:
                DEvent13(reader);
                break;
            case 14:
                DEvent14(reader);
                break;
            case 15:
                DEvent15(reader);
                break;
            case 16:
                DEvent16(reader);
                break;
            case 17:
                DEvent17(reader);
                break;
            case 18:
                DEvent18(reader);
                break;
            case 19:
                DEvent19(reader);
                break;
            case 20:
                DEvent20(reader);
                break;
            default:
                Debug.Log("Event not found.");
                break;
        }
        
        reader.EndOptionWait();
    }

    //Event functions called by Event()
    protected virtual void DEvent0(TextReader reader) { Debug.Log("DEvent0"); }
    protected virtual void DEvent1(TextReader reader) { Debug.Log("DEvent1"); }
    protected virtual void DEvent2(TextReader reader) { Debug.Log("DEvent2"); }
    protected virtual void DEvent3(TextReader reader) { Debug.Log("DEvent3"); }
    protected virtual void DEvent4(TextReader reader) { Debug.Log("DEvent4"); }
    protected virtual void DEvent5(TextReader reader) { Debug.Log("DEvent5"); }
    protected virtual void DEvent6(TextReader reader) { Debug.Log("DEvent6"); }
    protected virtual void DEvent7(TextReader reader) { Debug.Log("DEvent7"); }
    protected virtual void DEvent8(TextReader reader) { Debug.Log("DEvent8"); }
    protected virtual void DEvent9(TextReader reader) { Debug.Log("DEvent9"); }
    protected virtual void DEvent10(TextReader reader) { Debug.Log("DEvent10"); }
    protected virtual void DEvent11(TextReader reader) { Debug.Log("DEvent11"); }
    protected virtual void DEvent12(TextReader reader) { Debug.Log("DEvent12"); }
    protected virtual void DEvent13(TextReader reader) { Debug.Log("DEvent13"); }
    protected virtual void DEvent14(TextReader reader) { Debug.Log("DEvent14"); }
    protected virtual void DEvent15(TextReader reader) { Debug.Log("DEvent15"); }
    protected virtual void DEvent16(TextReader reader) { Debug.Log("Devent16"); }
    protected virtual void DEvent17( TextReader reader) { Debug.Log("DEvent17"); }
    protected virtual void DEvent18(TextReader reader) { Debug.Log("DEvent18"); }
    protected virtual void DEvent19(TextReader reader) { Debug.Log("DEvent19"); }
    protected virtual void DEvent20(TextReader reader) { Debug.Log("DEvent20"); }
}
