using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TalkingNPC : NPC
{
    public string[] beginningDialogue;
    public TextMeshProUGUI dialogueText;
    private bool talkable = true;
    private int dialogueIndex = 0;

    protected override void Awake()
    {
        base.Awake();
    }


    // Start is called before the first frame update
    protected override void Start()
    {
        
    }

    public bool Talk()
    {
        Debug.Log("talk!");
        dialogueText.text = beginningDialogue[dialogueIndex];
        dialogueIndex++;
        if (dialogueIndex >= beginningDialogue.Length)
        {
            dialogueText.text = "";
            StartCoroutine(Roaming());
            talkable = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsTalkable()
    {
        return talkable;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
