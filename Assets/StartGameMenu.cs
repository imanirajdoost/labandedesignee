using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameMenu : MonoBehaviour
{
    public void StartGame()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index + 1);
    }
}
