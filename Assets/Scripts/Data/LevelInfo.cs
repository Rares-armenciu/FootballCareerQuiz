using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Scripts.Data
{
    public class LevelInfo
    {
        public int Level;

        public bool IsUnlocked;

        public bool IsCurrent;

        public bool IsBossLevel;

        public int QuestionCount;

        public int BestStars;

        public int BestReward;

        public int BestCorrectAnswers;

        public bool IsCompleted => BestStars > 0;
    }
}
