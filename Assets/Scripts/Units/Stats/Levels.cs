using System;
using UnityEngine.UI;

public class Levels
{
    readonly int baseExperienceRequiredToLevel;
    readonly int levelScalingStartingLevel;
    readonly int experienceRequirementAddedPerLevel;

    public int level { get; private set; } = 1;
    private int currentExperience;
    private int experienceToNextLevel;

    private Image expBar;

    public event Action<int> OnLevelUp = delegate { };

    /// <returns>True if leveled up</returns>
    public bool AddExperience(int experienceToAdd)
    {
        currentExperience += experienceToAdd;

        if (currentExperience >= experienceToNextLevel)
        {
            LevelUp();
            return true;
        }

        RefreshExpBar();
        return false;
    }

    private void LevelUp()
    {
        level = level + 1;
        currentExperience = currentExperience - experienceToNextLevel;
        if (level >= levelScalingStartingLevel) experienceToNextLevel = baseExperienceRequiredToLevel + experienceRequirementAddedPerLevel * (level - levelScalingStartingLevel + 1);
        OnLevelUp.Invoke(level);
        RefreshExpBar();
    }

    public void CheckExperience()
    {
        if (currentExperience >= experienceToNextLevel) LevelUp();
    }

    private void RefreshExpBar()
    {
        expBar.fillAmount = Helpers.Map(currentExperience, 0, experienceToNextLevel, 0, 1, true);
    }

    public Levels(Image expBar, int baseExperienceRequiredToLevel, int levelScalingStartingLevel, int experienceRequirementAddedPerLevel)
    {
        this.expBar = expBar;
        this.baseExperienceRequiredToLevel = baseExperienceRequiredToLevel;
        this.levelScalingStartingLevel = levelScalingStartingLevel;
        this.experienceRequirementAddedPerLevel = experienceRequirementAddedPerLevel;

        currentExperience = 0;
        experienceToNextLevel = baseExperienceRequiredToLevel;

        RefreshExpBar();
    }

    public override string ToString() => $"Level: {level} | Exp to next level: {experienceToNextLevel} | Current exp: {currentExperience}";
}
