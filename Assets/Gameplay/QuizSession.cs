using System;

public class QuizSession
{
    private readonly QuizGenerator _quizGenerator;
    private readonly LifeService _lifeService;
    private readonly CoinsService _coinsService;
    private readonly ProgressionService _progressionService;

    public QuizQuestion CurrentQuestion { get; private set; }

    public QuizSession(QuizGenerator generator, LifeService lifeService, CoinsService coinsService, ProgressionService progressionService)
    {
        _quizGenerator = generator;
        _lifeService = lifeService;
        _coinsService = coinsService;
        _progressionService = progressionService;
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

        return true;
    }

    public bool SubmitAnswer(FootballPlayer player)
    {
        bool correct = player == CurrentQuestion.CorrectPlayer;

        if (correct)
        {
            _coinsService.AddCoins(25);
            _progressionService.LevelUp();
        }
        else
        {
            _lifeService.SpendLife();
        }

        return correct;
    }
}