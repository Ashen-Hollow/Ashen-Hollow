using UnityEngine;
using UnityEngine.InputSystem;

public enum STATE
{
    DISABLE,
    WAITING,
    TYPING
}

public class DialogueSystem : MonoBehaviour
{
    public DialogueData dialogueData;

    int currentText = 0;
    bool finished = false;

    TypeTextAnimation typeText;
    DialogueUI dialogueUI;
    STATE state;

    
    void Awake()
    {
        typeText = FindObjectOfType<TypeTextAnimation>();
        dialogueUI = FindAnyObjectByType<DialogueUI>();
        typeText.TypeFinished = OnTypeFinishe;
    }
    void Start()
    {
        state = STATE.DISABLE;
    }

    // Update is called once per frame
    void Update()
    {
        if(state == STATE.DISABLE) return;

        switch (state)
        {
            case STATE.WAITING:
                Waiting();
                break;
            case STATE.TYPING:
                Typing();
                break;
        }
    }

    public void Next()
    {
        if(currentText == 0)
        {
            dialogueUI.Enable();
        }
        dialogueUI.SetName(dialogueData.talkScript[currentText].name);
        typeText.fullText = dialogueData.talkScript[currentText++].text;

        if(currentText == dialogueData.talkScript.Count) finished = true;

        typeText.StartTyping();
        state = STATE.TYPING;
    }

    void OnTypeFinishe()
    {
        state = STATE.WAITING;
    }

    void Waiting()
    {
        if (Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame)
        {
            if (!finished){
                Next();
            } 
            else{
                dialogueUI.Disable();
                state = STATE.DISABLE;
                currentText = 0;
                finished = false;
            }
        }
    }

    void Typing()
    {
        if (Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame)
        {
            typeText.Skip();
            state = STATE.WAITING;
        }
    }

    public void StartDialogue(DialogueData data)
    {
        if (state != STATE.DISABLE)
        {
            return;
        } 
        dialogueData = data;
        currentText = 0;
        finished = false;
        
        Next();
    }
}
