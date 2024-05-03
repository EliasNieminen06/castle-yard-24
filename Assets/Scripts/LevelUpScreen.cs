using System;
using UnityEngine;
using System.Collections.Generic;

public class LevelUpScreen : MonoBehaviour
{
    private static LevelUpScreen instance;

    [SerializeField] private List<BaseStatModifier> statModifiers;

    [SerializeField] private StatUpgrade upgrade1;
    [SerializeField] private StatUpgrade upgrade2;
    [SerializeField] private StatUpgrade upgrade3;

    private event Action onHide = delegate { };

    private void Awake()
    {
        instance = this;
        Hide();
    }

    private void Show(Hero hero, Action onHide)
    {
        gameObject.SetActive(true);

        if (statModifiers.Count == 0)
        {
            Debug.LogWarning("0 Modifiers in Levelupscreen... Hiding");
            Hide();
            return;
        }

        SetUpgrades(hero);
        this.onHide = onHide;

        Time.timeScale = 0f;
    }

    private void Hide()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
        onHide.Invoke();
    }

    private void SetUpgrades(Hero hero)
    {
        int randNum1 = UnityEngine.Random.Range(0, statModifiers.Count);
        upgrade1.SetUpgrade(hero, statModifiers[randNum1]);

        if (statModifiers.Count == 1) return;

        int randNum2 = UnityEngine.Random.Range(0, statModifiers.Count);
        while (randNum2 == randNum1)
        {
            randNum2 = UnityEngine.Random.Range(0, statModifiers.Count);
        }
        upgrade2.SetUpgrade(hero, statModifiers[randNum2]);

        if (statModifiers.Count == 2) return;

        int randNum3 = UnityEngine.Random.Range(0, statModifiers.Count);
        while (randNum3 == randNum2 || randNum3 == randNum1)
        {
            randNum3 = UnityEngine.Random.Range(0, statModifiers.Count);
        }
        upgrade3.SetUpgrade(hero, statModifiers[randNum3]);
    }

    public static void Show_Static(Hero hero, Action onHide)
    {
        instance.Show(hero, onHide);
    }

    public static void Hide_Static()
    {
        instance.Hide();
    }
}
