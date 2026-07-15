public class QuizSession
{
    private readonly QuizGenerator generator;

    public QuizQuestion CurrentQuestion { get; private set; }

    public QuizSession(PlayerDatabase database)
    {
        generator = new QuizGenerator(database);
    }

    public QuizQuestion NextQuestion()
    {
        CurrentQuestion = generator.Generate();

        return CurrentQuestion;
    }

    public bool RevealHint()
    {
        if (!CurrentQuestion.CanRevealClub)
            return false;

        if (!GameManager.Instance.SpendCoins(50))
            return false;

        CurrentQuestion.RevealRandomClub();

        return true;
    }

    public bool SubmitAnswer(FootballPlayer player)
    {
        bool correct = player == CurrentQuestion.CorrectPlayer;

        if (correct)
        {
            GameManager.Instance.WinCoins(20);
        }
        else
        {
            GameManager.Instance.LoseLife();
        }

        return correct;
    }
}