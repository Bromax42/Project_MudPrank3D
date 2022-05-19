using UnityEngine;


public class UIButtonFunctions : MonoBehaviour
{
    private GameManager gameManager;
    private CarMovement carMovement;

    private GameObject Pan_Transition;
    private GameObject But_Start, But_Continue;

    void Start()
    {
        // Get GameManager Script.
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        // Get Player and its PlayerMovement Script.
        carMovement = gameManager.player.GetComponent<CarMovement>();

        // Get Pan-Transition from GameManager.
        Pan_Transition = gameManager.Pan_Transition;

        // Get Buttons from GameManager.
        But_Start = gameManager.But_Start;
        But_Continue = gameManager.But_Continue;
    }
    public void ButtonStart()
    {
        carMovement.enabled = true;
        But_Start.SetActive(false);
      //Pan_Transition.SetActive(false);

    }

    public void ButtonContinue()
    {


    }

    public void PauseGame()
    {
        if (Time.timeScale == 1f)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
