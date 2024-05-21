using System.Collections;
using UnityEngine;
using TMPro;

namespace Assets.Scripts
{
    public class KillCounter : MonoBehaviour
    {
        private static KillCounter instance;

        [SerializeField] private TextMeshProUGUI killCountTMP;
        private int killCount;

        private void Awake()
        {
            instance = this;
            killCountTMP.text = "0";
        }

        private void AddKill()
        {
            killCount++;
            killCountTMP.text = killCount.ToString();
        }

        public static void AddKill_Static()
        {
            instance.AddKill();
        }
    }
}