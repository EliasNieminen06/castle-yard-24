using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour
{
    public float playerMovementSpeed;
    private Vector3 playerMovementVector;
    private Rigidbody PlayerRB;
    public Hero hero;

    void Start()
    {
        playerMovementSpeed = 4;
        PlayerRB = this.GetComponent<Rigidbody>();
        hero = this.GetComponent<Hero>();

        playerMovementSpeed = hero.Stats.Speed;
        
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



     public void ModifySpeed()
    {
        playerMovementSpeed = hero.Stats.Speed;
    }
}