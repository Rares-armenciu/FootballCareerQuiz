using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private ProfileView _profileView;
    [SerializeField] private AchievementsView _achievementsView;

    public void PlayGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void OpenProfile()
    {
        _profileView.Show(
            GameManager.Instance.Progress,
            GameManager.Instance.Statistics);
    }

    public void OpenAchievements()
    {
        _achievementsView.Show(
            GameManager.Instance.AchievementService,
            GameManager.Instance.Achievements);
    }
}
