using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [Header("Quem sou eu?")]
    public string meuId; // Ex: "EntradaEsquerda"

    // Variável global que o Portal alterou antes de mudar de cena
    public static string proximoSpawnId;

    void Start()
    {
        // Se o portal mandou o jogador vir para este ponto específico
        if (proximoSpawnId == meuId)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                // Teletransporta o jogador instantaneamente
                player.transform.position = transform.position;
            }
        }
    }
}