using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPC : MonoBehaviour
{
    [Header("Diálogo")]
    public DialogueData dialogueData;  // arraste o ScriptableObject do NPC aqui
    public float interactionRange = 2.0f;
    public Transform player;

    [Header("Indicador Visual")]
    [SerializeField] private SpriteRenderer warningIndicator;

    private DialogueSystem dialogueSystem;
    private bool playerInRange = false;

    void Awake()
    {
        dialogueSystem = FindObjectOfType<DialogueSystem>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (player == null) return;

        float dist = Mathf.Abs(transform.position.x - player.position.x);
        playerInRange = dist < interactionRange;

        if (warningIndicator != null)
        {
            warningIndicator.enabled = playerInRange;
        }
        
        if (playerInRange && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            dialogueSystem.StartDialogue(dialogueData);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}