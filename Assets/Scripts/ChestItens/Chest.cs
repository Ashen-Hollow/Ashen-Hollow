using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Chest : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private CollectibleSO collectibleSO;
    [SerializeField] private GameObject lootPrefab;
    [SerializeField] private float spawnDelay = .2f; 

    private PlayerInput playerInput;
    private bool isOpened;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInput = collision.GetComponent<PlayerInput>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInput = null;
        }
    }

    private void Update()
    {
        if (isOpened || playerInput == null)
            return;

        Vector2 moveInput = playerInput.actions["Move"].ReadValue<Vector2>();

        if (moveInput.y > .1f || playerInput.actions["Interact"].WasPressedThisFrame())
        {
            StartCoroutine(OpenChestRoutine());
        }
    }

   
    private IEnumerator OpenChestRoutine()
    {
        isOpened = true; 
        anim.Play("ChestOpen");

        yield return new WaitForSeconds(spawnDelay);

        Loot newLoot = Instantiate(lootPrefab, transform.position, Quaternion.identity).GetComponent<Loot>();
        newLoot.Initialize(collectibleSO);
    }
}