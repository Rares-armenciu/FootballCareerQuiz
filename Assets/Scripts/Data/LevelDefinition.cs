using System;
using UnityEngine;

[Serializable]
public class LevelDefinition
{
    public int Level;

    public int QuestionCount;

    public int BaseReward;

    public int WrongAnswerPenalty;

    public int HintPenalty;

    public int FlawlessBonus;

    public bool IsBossLevel;
}