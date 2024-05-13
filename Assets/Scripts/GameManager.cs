using System;
using UnityEngine;

[DefaultExecutionOrder(-10)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else
        {
            Instance = this;
            Player = player;
            Canvas = canvas;
        }
    }

    [SerializeField] private Transform player;
    public Transform Player { get; private set; }

    [SerializeField] private Transform canvas;
    public Transform Canvas { get; private set; }
}
