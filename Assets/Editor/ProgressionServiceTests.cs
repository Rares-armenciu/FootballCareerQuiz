using NUnit.Framework;

// Tests for ProgressionService replay isolation and normal progression.
public class ProgressionServiceTests
{
    [Test]
    public void ReplayDoesNotMutatePlayerProgress()
    {
        // Arrange
        var playerProgress = new PlayerProgress();
        playerProgress.CurrentQuestion = 2;
        playerProgress.CorrectAnswersThisLevel = 1;
        playerProgress.WrongAnswersThisLevel = 0;
        playerProgress.HintsUsedThisLevel = 0;

        var service = new ProgressionService(playerProgress);

        // Act - start replay
        service.StartReplay(1);

        // Replay should start at question 0
        Assert.IsTrue(service.IsReplay);
        Assert.AreEqual(0, service.CurrentQuestion);

        // Advance a question in replay
        bool levelCompleted = service.AdvanceQuestion();

        // Assert - session counter incremented, persistent not mutated
        Assert.AreEqual(1, service.CurrentQuestion);
        Assert.AreEqual(2, playerProgress.CurrentQuestion);
        Assert.IsFalse(levelCompleted); // depends on QuestionsInCurrentLevel but should not throw

        // Record answers in replay
        service.RecordCorrectAnswer();
        service.RecordWrongAnswer();
        service.RecordHintUsed();

        // Finish replay
        service.FinishReplay();

        // After finishing replay, ensure persistent progress remained unchanged
        Assert.IsFalse(service.IsReplay);
        Assert.AreEqual(2, playerProgress.CurrentQuestion);
        Assert.AreEqual(1, playerProgress.CorrectAnswersThisLevel);
        Assert.AreEqual(0, playerProgress.WrongAnswersThisLevel);
        Assert.AreEqual(0, playerProgress.HintsUsedThisLevel);
    }

    [Test]
    public void NormalProgressionMutatesPlayerProgress()
    {
        // Arrange
        var playerProgress = new PlayerProgress();
        playerProgress.CurrentQuestion = 0;
        playerProgress.CorrectAnswersThisLevel = 0;

        var service = new ProgressionService(playerProgress);

        // Act
        bool completed = service.AdvanceQuestion();

        // Assert - persistent progress changed
        Assert.IsFalse(service.IsReplay);
        Assert.AreEqual(1, playerProgress.CurrentQuestion);
        Assert.AreEqual(1, service.CurrentQuestion);

        service.RecordCorrectAnswer();
        Assert.AreEqual(1, playerProgress.CorrectAnswersThisLevel);
    }
}
