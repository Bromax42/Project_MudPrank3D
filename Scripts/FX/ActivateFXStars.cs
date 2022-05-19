using System.Collections;
using UnityEngine;

public class ActivateFXStars : MonoBehaviour
{


    // Connect to Game Manager.
    public GameManager gameManager;

    // Get Stars Earned in a level.
    public float starsEarned;

    // Define FX_UI_Star.
    ParticleSystem FX_UI_Star;
    ParticleSystem.EmissionModule emission;
    
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        FX_UI_Star = GetComponent<ParticleSystem>();
        emission = FX_UI_Star.emission;
    }


    public IEnumerator StartEmission()
    {
        // Give time for effect to build up.
        yield return new WaitForSeconds(0.75f);

        for (int i = 0; i < starsEarned; i++)
        {
            // Increase Star Emission rate relative to stars earned.
            emission.rateOverTimeMultiplier += starsEarned;
            emission.enabled = true;
            gameManager.GainStars();

            // End Effect after time.
            yield return new WaitForSeconds(0.4f);
            emission.enabled = false;
            
        }
        // Activate But_Continue after collecting Stars.
        gameManager.But_Continue.SetActive(true);
    }





    //gameManager.GainStars();

}
