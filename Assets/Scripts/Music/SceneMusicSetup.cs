using UnityEngine;

public class SceneMusicSetup : MonoBehaviour
{
    [Header("Trilha Sonora desta Fase")]
    [Tooltip("Arraste a música que deve tocar neste mapa")]
    public AudioClip musicaDaFase;

    void Start()
    {
        
        if (AudioManager.instance != null && musicaDaFase != null)
        {
           
            AudioManager.instance.PlayMusic(musicaDaFase);
        }
        else if (AudioManager.instance == null)
        {
            Debug.LogWarning("AudioManager não foi encontrado! Lembre-se de começar o jogo pelo Menu.");
        }
    }
}