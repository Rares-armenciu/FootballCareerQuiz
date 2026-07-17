using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class QuizSession
{
    private readonly QuizGenerator _quizGenerator;
    private readonly LifeService _lifeService;
    private readonly CoinsService _coinsService;
    private readonly ProgressionService _progressionService;
    private readonly StatisticsService _statisticsService;
    private readonly AchievementService _achievementsService;

    public QuizQuestion CurrentQuestion { get; private set; }

    public QuizSession(QuizGenerator generator, LifeService lifeService, CoinsService coinsService, ProgressionService progressionService, StatisticsService statisticsService, AchievementService achievementsService)
    {
        _quizGenerator = generator;
        _lifeService = lifeService;
        _coinsService = coinsService;
        _progressionService = progressionService;
        _statisticsService = statisticsService;
        _achievementsService = achievementsService;
    }

    public QuizQuestion NextQuestion()
    {
        CurrentQuestion = _quizGenerator.Generate();

        return CurrentQuestion;
    }

    public bool RevealHint()
    {
        if (!CurrentQuestion.CanRevealClub)
            return false;

        if (!_coinsService.SpendCoins(50))
        {
            return false;
        }

        _achievementsService.CheckAchievements(AchievementType.HintsUsed);

        CurrentQuestion.RevealRandomClub();
        _statisticsService.UseHint();
        Debug.Log("Hint revealed");

        return true;
    }

    public bool SubmitAnswer(FootballPlayer player)
    {
        bool correct = player == CurrentQuestion.CorrectPlayer;

        if (correct)
        {
            _coinsService.AddCoins(25);
            _progressionService.LevelUp();
            _statisticsService.RecordCorrectAnswer();
            Debug.Log("Correct Answer recorded");
        }
        else
        {
            _lifeService.SpendLife();
            _statisticsService.RecordWrongAnswer();
            Debug.Log("Wrong Answer recorded");
        }

        _achievementsService.CheckAchievements(AchievementType.QuestionsAnswered);

        return correct;
    }
}