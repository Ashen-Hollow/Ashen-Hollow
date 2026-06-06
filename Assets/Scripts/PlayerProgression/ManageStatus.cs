using UnityEngine;
using UnityEngine.SceneManagement; 

public class ManageStatus : MonoBehaviour
{
    public string nomeDaCenaUI = "Scenes/UI"; 

    public void AlternarCenaUI()
    {
        Scene cena = SceneManager.GetSceneByName(nomeDaCenaUI);

        if (!cena.isLoaded)
        {
            SceneManager.LoadScene(nomeDaCenaUI, LoadSceneMode.Additive);
            
            Time.timeScale = 0f; 

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            FecharCenaUI();
        }
    }

    public void FecharCenaUI()
    {
        SceneManager.UnloadSceneAsync(nomeDaCenaUI);
        Time.timeScale = 1f; 
    }
}