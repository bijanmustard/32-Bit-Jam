using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyNPC : MonoBehaviour
{
    public Dialogue dia;
    //public DiaSprites myDIASPR;

    DialogueCanvas dCanvas;

    private void Awake()
    {
        dia = GetComponent<Dialogue>();
        dCanvas = FindObjectOfType<DialogueCanvas>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !TextboxManager.IsRunning)
        {
            StartDialogue();
        }
    }

    //StartDialogue is called to start the dialogue event.
    public void StartDialogue()
    {
        //if (myDIASPR != null) dCanvas.EnableCanvas(ref myDIASPR);
        TextboxManager.StartDialogue(dia);
    }

}
