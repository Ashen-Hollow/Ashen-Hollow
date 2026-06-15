using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseScene : MonoBehaviour
{
    public string nomeDaCenaUI = "Scenes/UI"; 

    public void FecharMinhaCena()
    {
        Time.timeScale = 1f; 
        SceneManager.UnloadSceneAsync(nomeDaCenaUI);
    }
}