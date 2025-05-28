using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject panelMenu;
    [SerializeField] RectTransform pausePanelRect;
    [SerializeField] float topPosY, middleTopY;
    [SerializeField] float tweenDuration;

    public void Pause()
    {
        PausePanelIntro();
        Time.timeScale = 0;
    }

    public void Resume()
    {
        PausePanelOutro();
        Time.timeScale = 1;
    }

    private void PausePanelIntro()
    {
        pausePanelRect.DOAnchorPosY(middleTopY, tweenDuration).SetUpdate(true);
    }

    private void PausePanelOutro()
    {
        pausePanelRect.DOAnchorPosY(topPosY, tweenDuration).SetUpdate(true);
    }
}
