using System.Collections.Generic;

public static class AchievementDatabase
{
    public static readonly List<AchievementDefinition> All = new()
    {
        new AchievementDefinition(
            "first_correct",
            "First Goal",
            "Answer your first question correctly.",
            AchievementType.CorrectAnswers,
            1,
            50),

        new AchievementDefinition(
            "quiz_master",
            "Quiz Master",
            "Answer 100 questions correctly.",
            AchievementType.CorrectAnswers,
            100,
            500),

        new AchievementDefinition(
            "student",
            "Student",
            "Answer 50 questions.",
            AchievementType.QuestionsAnswered,
            50,
            100),

        new AchievementDefinition(
            "on_fire",
            "On Fire",
            "Reach a streak of 10.",
            AchievementType.LongestStreak,
            10,
            250),
    };
}