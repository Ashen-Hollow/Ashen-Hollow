using System.Collections;
using UnityEngine;
using TMPro;

public class Loot : MonoBehaviour
{
    private Player player;
    [SerializeField] private CollectibleSO collectibleSO;
    [SerializeField] private SpriteRenderer sr;
    public Animator anim;
    public TMP_Text itemMessage;

    [Header("Configurações de Coleta")]
    [SerializeField] private float tempoParaColetar = 1f;
    private bool podeColetar = false;

    [Header("Configurações do Drop (Física)")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float dropForce = 5f; // Força do pulo

    public void Initialize(CollectibleSO collectibleSO)
    {
        this.collectibleSO = collectibleSO;
        sr.sprite = collectibleSO.itemSprite;

        // Começa a contar o tempo para poder pegar
        StartCoroutine(LiberarColetaRoutine());

        // Faz o item pular da caixa
        PopItemOut();
    }

    private void PopItemOut()
    {
        if (rb != null)
        {
            // Sorteia um valor para ele cair um pouco para a esquerda ou para a direita
            float randomX = Random.Range(-0.5f, 0.5f);

            // Define a direção: o X é aleatório, e o Y (para cima) é 1.
            Vector2 jumpDirection = new Vector2(randomX, 1f).normalized;

            // Empurra o item usando a física de Impulso
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

        player = collision.GetComponent<Player>();

        if (player == null)
            return;

        CollectItem();
    }

    private void CollectItem()
    {
        collectibleSO.Collect(player);
        Destroy(gameObject);
    }
}