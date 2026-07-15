public class QuizSession
{
    private readonly QuizGenerator generator;
    private readonly HeaderView headerView;

    public QuizQuestion CurrentQuestion { get; private set; }

    public QuizSession(PlayerDatabase database, HeaderView headerView)
    {
        generator = new QuizGenerator(database);
        this.headerView = headerView;
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

        if (!GameManager.Instance.Progress.SpendCoins(50))
        {
            headerView.Show(GameManager.Instance.Progress);
            return false;
        }

        CurrentQuestion.RevealRandomClub();
        headerView.Show(GameManager.Instance.Progress);

        return true;
    }

    public bool SubmitAnswer(FootballPlayer player)
    {
        bool correct = player == CurrentQuestion.CorrectPlayer;

        if (correct)
        {
            GameManager.Instance.Progress.AddCoins(25);
            GameManager.Instance.Progress.LevelUp();
        }
        else
        {
            GameManager.Instance.Progress.LoseLife();
        }

        return correct;
    }
}