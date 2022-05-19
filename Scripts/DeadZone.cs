using UnityEngine;

public class DeadZone : MonoBehaviour
{
    public GameManager gameManager;
    void Start()
    {
        // Connect to GameManager.
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            // Stop Camera Movement.
            Camera.main.GetComponent<CameraSmoothFollow>().enabled = false;
            
            // Restart current level.
            gameManager.RestartLevel();

        }
    }

}
