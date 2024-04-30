using UnityEngine;

public class CameraMovementHandler : MonoBehaviour
{
    public GameObject Camera;
    public GameObject Player;
    Vector3 OffSet;

    void Start()
    {
        Camera = gameObject;
        OffSet = Camera.transform.position - Player.transform.position;
    }

    void Update()
    {
        Camera.transform.position = Player.transform.position + OffSet;
    }
}