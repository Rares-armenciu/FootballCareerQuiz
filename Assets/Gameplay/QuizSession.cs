using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class QuizSession
{
    private readonly QuizGenerator _quizGenerator;
    private readonly LifeService _lifeService;
    private readonly CoinsService _coinsService;
    private readonly ProgressionService _progressionService;
    private readonly StatisticsService _statisticsService;

    public QuizQuestion CurrentQuestion { get; private set; }

    public QuizSession(QuizGenerator generator, LifeService lifeService, CoinsService coinsService, ProgressionService progressionService, StatisticsService statisticsService)
    {
        _quizGenerator = generator;
        _lifeService = lifeService;
        _coinsService = coinsService;
        _progressionService = progressionService;
        _statisticsService = statisticsService;
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

        CurrentQuestion.RevealRandomClub();
        _statisticsService.UseHint();
        Debug.Log("Hints revealed:  " + GameManager.Instance.Statistics.HintsUsed);

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
            Debug.Log("Correct Answers:  " + GameManager.Instance.Statistics.CorrectAnswers);
        }
        else
        {
            _lifeService.SpendLife();
            _statisticsService.RecordWrongAnswer();
            Debug.Log("Wrong Answers:  " + GameManager.Instance.Statistics.WrongAnswers);
        }

        return correct;
    }
}