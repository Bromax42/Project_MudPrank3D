using UnityEngine;

public class CarObstacles : MonoBehaviour
{
    public GameManager gameManager;

    public float speedReduction = 5000f;
    public int damage = 40;

    // Boolean for one-time-event check.
    private bool hasHitTheCar;


    private void Start()
    {
        hasHitTheCar = false;

        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    //When hit, slow player down.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("PlayerCollider") && !hasHitTheCar)
        {
            hasHitTheCar = true;
            gameManager.carHealth -= damage;
            collision.collider.GetComponentInParent<CarMovement>().speed -= speedReduction;

            // Check Car Health.
            if (gameManager.carHealth <= 0)
            {
                gameManager.RestartLevel();
            }
        }
    }

}
