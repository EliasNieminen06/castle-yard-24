using System;
using UnityEngine;
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
        }
    }

    [SerializeField] private Transform player;
    public Transform Player { get; private set; }
}
