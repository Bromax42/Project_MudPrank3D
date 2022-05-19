using ElephantSDK;
using UnityEngine;
using UnityEngine.Events;

public class LevelFinish : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject UI_Canvas, Pan_Transition, But_Continue;

    public UnityEvent FinishLineEvents;

    private void Start()
    {
        // Connect to GameManager.
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        // Get UI Elements from GameManager.
        UI_Canvas = gameManager.UI_Canvas;
        Pan_Transition = gameManager.Pan_Transition;
        But_Continue = gameManager.But_Continue;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerCollider"))
        {

            // ELEPHANT - Level Completed Message.
            Elephant.LevelCompleted((gameManager.currentLevel + 1));

            // Stop Player Movement, Activate Transition Panel.
            other.GetComponentInParent<CarMovement>().enabled = false;
            Pan_Transition.SetActive(true);
            gameManager.Img_Win.SetActive(true);

            // More Like Activate Camera Function "Focus On Finish Line".
            FinishLineEvents.Invoke();

            // Start FX_Stars.
            gameManager.StartFX_Stars();
        }
    }
}
