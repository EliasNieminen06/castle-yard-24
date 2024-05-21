using System.Collections;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class ObjectFader : MonoBehaviour
{
    [SerializeField] private Material transparentMaterial;
    private Material solidMaterial;

    Renderer rend;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        solidMaterial = rend.material;
    }

    public void Fade()
    {
        rend.material = transparentMaterial;
    }

    public void UnFade()
    {
        rend.material = solidMaterial;
    }
}
