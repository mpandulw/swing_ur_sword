using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void buttonStart()
    {
        SceneManager.LoadScene("Level1");
    }

    public void buttonExit()
    {
        Application.Quit();
    }
}
