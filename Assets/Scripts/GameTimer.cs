using System.Collections;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameTimer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timerTMP;
        private float time;
        private int seconds;
        private int minutes;

        private int previousSecond;
        private void Update()
        {
            time += Time.deltaTime;

            if (Mathf.FloorToInt(time) > previousSecond)
            {
                previousSecond = Mathf.FloorToInt(time);
                IncrementSeconds();
            }
        }

        private void IncrementSeconds()
        {
            seconds++;

            if (seconds == 60)
            {
                seconds = 0;
                IncrementMinutes();
            }

            UpdateText();
        }

        private void IncrementMinutes()
        {
            minutes++;
        }

        private void UpdateText()
        {
            timerTMP.text = $"{minutes}.{seconds}";
        }
    }
}