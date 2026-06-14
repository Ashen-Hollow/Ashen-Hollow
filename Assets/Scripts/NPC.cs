using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPC : MonoBehaviour
{
    [Header("Diálogo")]
    public DialogueData dialogueData;  // arraste o ScriptableObject do NPC aqui
    public float interactionRange = 2.0f;
    public Transform player;

    private DialogueSystem dialogueSystem;
    private bool playerInRange = false;

    void Awake()
    {
        dialogueSystem = FindObjectOfType<DialogueSystem>();
    }

    void Start()
    {
        Console.WriteLine("debug");
    }

    void Update()
    {
        if (player == null) return;

        float dist = Mathf.Abs(transform.position.x - player.position.x);
        playerInRange = dist < interactionRange;

        Console.WriteLine(playerInRange);
        if (playerInRange && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            // Troca o dialogueData do sistema para o deste NPC e inicia
            dialogueSystem.StartDialogue(dialogueData);
            Console.WriteLine("debug");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}