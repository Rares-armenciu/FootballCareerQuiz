using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] 
    private ProfileView _profileView;
    
    [SerializeField] 
    private AchievementsView _achievementsView;

    [SerializeField]
    private LevelsPopupView levelsPopup;

    [SerializeField]
    private HeaderView headerView;

    private void Start()
    {
        headerView.Show(GameManager.Instance.Progress);
    }

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

    public void OpenLevels()
    {
        levelsPopup.Show(GameManager.Instance.ProgressionService.GetLevels());
    }

    private void Awake()
    {
        levelsPopup.LevelSelected += OnLevelSelected;
    }

    private void OnDestroy()
    {
        levelsPopup.LevelSelected -= OnLevelSelected;
    }

    private void OnLevelSelected(int level)
    {
        GameManager.Instance.ProgressionService.StartReplay(level);

        SceneManager.LoadScene("Gameplay");
    }
}
