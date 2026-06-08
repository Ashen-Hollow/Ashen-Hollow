using System.Collections;
using UnityEngine;
using TMPro;

public class Loot : MonoBehaviour
{
    // Mantive o CollectibleSO para não dar erro no seu script do Baú, 
    // que provavelmente ainda usa isso no Instantiate.
    [SerializeField] private CollectibleSO collectibleSO;

    [SerializeField] private SpriteRenderer sr;
    public Animator anim;
    public TMP_Text itemMessage;

    // --- ADICIONADO: As variáveis que o seu Inventário precisa ---
    [Header("Configurações do Inventário")]
    [SerializeField] private string itemName;
    [SerializeField] private int quantity = 1;
    [SerializeField] private Sprite itemSprite;
    [TextArea][SerializeField] private string itemDescription;

    [Header("Configurações de Coleta")]
    [SerializeField] private float tempoParaColetar = 1f;
    private bool podeColetar = false;

    [Header("Configurações do Drop (Física)")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float dropForce = 5f;

    // O seu gerenciador
    private InventoryManager inventoryManager;

    void Start()
    {
        // O mesmo comando mágico do seu Item.cs
        inventoryManager = FindFirstObjectByType<InventoryManager>(FindObjectsInactive.Include);
    }

    public void Initialize(CollectibleSO collectibleSO)
    {
        this.collectibleSO = collectibleSO;

        // Puxa a imagem do SO caso você esqueça de colocar no Inspector
        if (itemSprite == null && collectibleSO != null)
        {
            itemSprite = collectibleSO.itemSprite;
        }

        if (sr != null) sr.sprite = itemSprite;

        StartCoroutine(LiberarColetaRoutine());
        PopItemOut();
    }

    private void PopItemOut()
    {
        if (rb != null)
        {
            float randomX = Random.Range(-0.5f, 0.5f);
            Vector2 jumpDirection = new Vector2(randomX, 1f).normalized;
            rb.AddForce(jumpDirection * dropForce, ForceMode2D.Impulse);
        }
    }

    private IEnumerator LiberarColetaRoutine()
    {
        yield return new WaitForSeconds(tempoParaColetar);
        podeColetar = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TentarColetar(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        TentarColetar(collision);
    }

    private void TentarColetar(Collider2D collision)
    {
        if (!podeColetar) return;

        // Trocado para usar o mesmo sistema de Tag do seu Item.cs (muito mais leve!)
        if (collision.CompareTag("Player"))
        {
            CollectItem();
        }
    }

    private void CollectItem()
    {
        if (inventoryManager != null)
        {
            // --- A NOVA LÓGICA DE INVENTÁRIO AQUI ---
            int leftOverItems = inventoryManager.AddItem(itemName, quantity, itemSprite, itemDescription);

            if (leftOverItems <= 0)
            {
                Destroy(gameObject); // Inventário sugou tudo, deleta o loot do chão
            }
            else
            {
                quantity = leftOverItems; // Inventário encheu, sobra o resto no chão
            }
        }
        else
        {
            Debug.LogWarning("InventoryManager não encontrado na cena!");
        }
    }
}