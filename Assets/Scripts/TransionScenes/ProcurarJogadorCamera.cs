using UnityEngine;
using Unity.Cinemachine; // Use apenas "using Cinemachine;" se for uma versão mais antiga do pacote

public class ProcurarJogadorCamera : MonoBehaviour
{
    void Start()
    {
        // 1. Procura na cena o objeto que tem a Tag "Player"
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // 2. Pega o componente da câmera
            // Se der erro nesta linha, mude para: GetComponent<CinemachineVirtualCamera>()
            var cinemachineCam = GetComponent<CinemachineCamera>();

            if (cinemachineCam != null)
            {
                // 3. Define o jogador como o alvo para a câmera seguir
                cinemachineCam.Follow = player.transform;
            }
        }
        else
        {
            Debug.LogWarning("Nenhum objeto com a tag 'Player' foi encontrado na cena!");
        }
    }
}