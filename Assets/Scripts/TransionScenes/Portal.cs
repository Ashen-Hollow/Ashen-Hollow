using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [Header("Para onde este portal leva?")]
    public string nomeDaCenaDestino; // Ex: "Graveyard"

    [Tooltip("O ID do ponto onde o jogador vai nascer lá. Ex: 'EntradaEsquerda'")]
    public string idDestino;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // O teu Player usa a tag "Player", logo isto vai funcionar diretamente
        if (collision.CompareTag("Player"))
        {
            // Avisa o jogo sobre qual é a porta de chegada
            SpawnPoint.proximoSpawnId = idDestino;

            // Carrega a cena nova
            SceneManager.LoadScene(nomeDaCenaDestino);
        }
    }
}