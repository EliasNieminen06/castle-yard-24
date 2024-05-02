using UnityEngine;
using System.Collections.Generic;

public class LevelUpScreen : MonoBehaviour
{
    private static LevelUpScreen instance;

    [SerializeField] private List<BaseStatModifier> statModifiers;

    [SerializeField] private StatUpgrade upgrade1;
    [SerializeField] private StatUpgrade upgrade2;
    [SerializeField] private StatUpgrade upgrade3;

    private void Awake()
    {
        instance = this;
        Hide();
    }

    private void Show(Hero hero)
    {
        gameObject.SetActive(true);

        if (statModifiers.Count == 0)
        {
            Debug.LogWarning("0 Modifiers in Levelupscreen... Hiding");
            Hide();
            return;
        }

        SetUpgrades(hero);

        Time.timeScale = 0f;
    }

    private void SetUpgrades(Hero hero)
    {
        int randNum1 = Random.Range(0, statModifiers.Count);
        upgrade1.SetUpgrade(hero, statModifiers[randNum1]);

        if (statModifiers.Count == 1) return;

        int randNum2 = Random.Range(0, statModifiers.Count);
        while (randNum2 == randNum1)
        {
            randNum2 = Random.Range(0, statModifiers.Count);
        }
        upgrade2.SetUpgrade(hero, statModifiers[randNum2]);

        if (statModifiers.Count == 2) return;

        int randNum3 = Random.Range(0, statModifiers.Count);
        while (randNum3 == randNum2 || randNum3 == randNum1)
        {
            randNum3 = Random.Range(0, statModifiers.Count);
        }
        upgrade3.SetUpgrade(hero, statModifiers[randNum3]);
    }

    private void Hide()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    public static void Show_Static(Hero hero)
    {
        instance.Show(hero);
    }

    public static void Hide_Static()
    {
        instance.Hide();
    }
}
