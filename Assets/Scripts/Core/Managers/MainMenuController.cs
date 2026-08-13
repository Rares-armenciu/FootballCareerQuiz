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

    [SerializeField]
    private DailyRewardView dailyRewardView;

    private void Start()
    {
        headerView.Show(GameManager.Instance.Progress);

        if (CanClaimDailyReward())
            OpenDailyReward();
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

    public bool CanClaimDailyReward()
    {
        return GameManager.Instance.DailyRewardService.CanClaim();
    }

    public int GetNextDailyReward()
    {
        int nextDay = Mathf.Clamp(GameManager.Instance.DailyRewardService.GetDisplayStreak() + 1, 1, 7);
        return GameManager.Instance.DailyRewardService.GetRewardForDay(nextDay);
    }

    public void OpenDailyReward()
    {
        dailyRewardView.Show();
    }

    public void CloseDailyReward()
    {
        dailyRewardView.Hide();
    }

    public DailyRewardClaim ClaimDailyReward()
    {
        DailyRewardClaim claim = GameManager.Instance.DailyRewardService.Claim();

        if (claim == null)
            return null;

        GameManager.Instance.SaveService.Save(
            GameManager.Instance.Progress,
            GameManager.Instance.Statistics,
            GameManager.Instance.Achievements,
            GameManager.Instance.DailyRewardService.Progress);

        headerView.Show(GameManager.Instance.Progress);
        return claim;
    }

    public bool CanClaimDailyRewardAdBonus()
    {
        return GameManager.Instance.DailyRewardService.CanClaimAdBonus();
    }

    public DailyRewardClaim ClaimDailyRewardAdBonus()
    {
        DailyRewardClaim claim = GameManager.Instance.DailyRewardService.ClaimAdBonus();

        if (claim == null)
            return null;

        GameManager.Instance.SaveService.Save(
            GameManager.Instance.Progress,
            GameManager.Instance.Statistics,
            GameManager.Instance.Achievements,
            GameManager.Instance.DailyRewardService.Progress);

        headerView.Show(GameManager.Instance.Progress);
        return claim;
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
