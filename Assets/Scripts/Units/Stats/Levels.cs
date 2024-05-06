using System;

public class Levels
{
    readonly int baseExperienceRequiredToLevel;
    readonly int levelScalingStartingLevel;
    readonly int experienceRequirementAddedPerLevel;

    public int level { get; private set; } = 1;
    private int currentExperience;
    private int experienceToNextLevel;

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

        return false;
    }

    private void LevelUp()
    {
        level = level + 1;
        currentExperience = currentExperience - experienceToNextLevel;
        if (level >= levelScalingStartingLevel) experienceToNextLevel = baseExperienceRequiredToLevel + experienceRequirementAddedPerLevel * (level - levelScalingStartingLevel + 1);
        OnLevelUp.Invoke(level);
    }

    public void CheckExperience()
    {
        if (currentExperience >= experienceToNextLevel) LevelUp();
    }

    public Levels(int baseExperienceRequiredToLevel, int levelScalingStartingLevel, int experienceRequirementAddedPerLevel)
    {
        this.baseExperienceRequiredToLevel = baseExperienceRequiredToLevel;
        this.levelScalingStartingLevel = levelScalingStartingLevel;
        this.experienceRequirementAddedPerLevel = experienceRequirementAddedPerLevel;

        currentExperience = 0;
        experienceToNextLevel = baseExperienceRequiredToLevel;
    }

    public override string ToString() => $"Level: {level} | Exp to next level: {experienceToNextLevel} | Current exp: {currentExperience}";
}
