using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    private Transform unitTransform;
    [SerializeField] private Image hpBar;

    public void Init(Transform unitTransform)
    {
        this.unitTransform = unitTransform;
        transform.SetAsFirstSibling();
    }

    private void LateUpdate()
    {
        transform.position = Camera.main.WorldToScreenPoint(unitTransform.position);
    }

    public void UpdateVisual(float currentHp, int maxHp)
    {
        hpBar.fillAmount = Helpers.Map(currentHp, 0, maxHp, 0, 1, true);
    }
}
