using UnityEngine;
using UnityEngine.SceneManagement; // Necessário para carregar cenas

#if UNITY_EDITOR
using UnityEditor; // Necessário para fechar o jogo no Editor
#endif

public class GameManager : MonoBehaviour
{
    // Implementação simples de Singleton para facilitar o acesso
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Opcional: mantém o GameManager vivo entre as cenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Carrega uma cena baseada no nome fornecido.
    /// </summary>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Fecha o jogo tanto no executável (Build) quanto no Editor da Unity.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Saindo do jogo...");

        // Fecha o executável
        Application.Quit();

        // Fecha o modo de Play no Editor da Unity
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #endif
    }
}