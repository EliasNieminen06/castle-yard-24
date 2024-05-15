using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[DefaultExecutionOrder(-1)]
public class StatsAndHealthText : MonoBehaviour
{
    private static StatsAndHealthText instance;

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI statsText;
    private Hero hero;

    private void Awake()
    {
        instance = this;
        hero = GameManager.Instance.Player.GetComponent<Hero>();
        Hide(0f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Show();
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            Hide(0f);
        }
    }

    private void UpdateTexts()
    {
        healthText.text = hero.Health.ToString();
        statsText.text = hero.Stats.ToString();

    }

    private void Show()
    {
        UpdateTexts();

        healthText.enabled = true;
        statsText.enabled = true;
    }

    private void Hide(float delay)
    {
        if (delay > 0)
        {
            StopAllCoroutines();
            StartCoroutine(HideWithDelay(delay));
            return;
        }

        healthText.enabled = false;
        statsText.enabled = false;

        IEnumerator HideWithDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            healthText.enabled = false;
            statsText.enabled = false;
        }
    }

    public static void Show_Static()
    {
        instance.Show();
    }

    public static void Hide_Static(float delay)
    {
        instance.Hide(delay);
    }

    public static void UpdateTexts_Static()
    {
        instance.UpdateTexts();
    }
}
