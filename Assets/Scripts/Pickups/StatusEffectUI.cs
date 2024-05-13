using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using TMPro;

public class StatusEffectUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Image timerImage;
    [SerializeField] private TextMeshProUGUI valueText;

    private float timerEndTime;
    public float timeLeft => timerEndTime - Time.time;
    private float duration;
    private float value;

    public event Action<StatusEffectUI> OnRemove = delegate { };

    public void Init(float duration, Color color, float value, StatModifier.OperatorType operatorType)
    {
        timerEndTime = Time.time + duration;
        this.duration = duration;
        icon.color = color;
        this.value = value;
        if (operatorType == StatModifier.OperatorType.Multiply) valueText.text = value.ToString() + "X";
        else valueText.text = value.ToString();
    }

    private void Update()
    {
        if (duration == 0) return;

        timerImage.fillAmount = Helpers.Map(timeLeft, 0, duration, 0, 1, true);
        if (timeLeft <= 0)
        {
            Remove();
        }
    }

    private void Remove()
    {
        OnRemove.Invoke(this);
        Destroy(gameObject);
    }

    public void Modify(bool refresh, float addedTime, bool stackable, StatModifier.OperatorType operatorType, float stackValue)
    {
        if (refresh)
        {
            timerEndTime = Time.time + duration;
        }

        if (addedTime > 0f)
        {
            timerEndTime += addedTime;
            duration += addedTime;
        }

        if (stackable)
        {
            value += stackValue;
            if (operatorType == StatModifier.OperatorType.Multiply) valueText.text = value.ToString() + "X";
            else valueText.text = value.ToString();
        }
    }

}
