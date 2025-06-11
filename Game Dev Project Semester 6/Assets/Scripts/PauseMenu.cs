using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuPanel; // Pause menu panel object  
    [SerializeField] RectTransform pausePanelRect;
    [SerializeField] float topPosY, middlePosY; // Location of the panel
    [SerializeField] float tweenDuration; // Duration of the animation

    // Pause the game
    public void Pause()
    {
        pausePanelRect.anchoredPosition = new Vector2(pausePanelRect.anchoredPosition.x, topPosY); // Ensure start position
        pauseMenuPanel.SetActive(true);

        PausePanelIntro();
        Time.timeScale = 0;
    }


    // Resume the game
    public async void Resume()
    {
        Time.timeScale = 1; // Restore time first so tween can animate
        await PausePanelOutro();
        pauseMenuPanel.SetActive(false);
    }


    // Back to main menu
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }


    private void PausePanelIntro()
    {
        pausePanelRect.DOAnchorPosY(middlePosY, tweenDuration).SetUpdate(true);
    }

    async Task PausePanelOutro()
    {
        await pausePanelRect.DOAnchorPosY(topPosY, tweenDuration).SetUpdate(true).AsyncWaitForCompletion();
    }
}
