using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour
{
    public float playerMovementSpeed;
    private Vector3 playerMovementVector;
    private Rigidbody PlayerRB;

    void Start()
    {
        playerMovementSpeed = 4;
        PlayerRB = this.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        playerMovementVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        playerMovementVector.Normalize();
    }

    private void FixedUpdate()
    {
        PlayerRB.velocity = playerMovementVector * playerMovementSpeed;
    }
}