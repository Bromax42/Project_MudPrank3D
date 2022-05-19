using UnityEngine;

public class SplashEffect : MonoBehaviour
{


    public GameObject gameManager;

    public float slowMotionAmount = 0.8f;

    float burstIntervalReduction = 0.001f;

    // Declare Effect Modules.
    public ParticleSystem FX_Splash;

    ParticleSystem.EmissionModule emitterModule;
    ParticleSystem.MainModule mainModule;


    //Accesses FX Modules.
    private void Start()
    {
        // Connect GameManager.
        gameManager = GameObject.FindGameObjectWithTag("GameManager");

        // Get Splash Effect.
        FX_Splash = GetComponentInChildren<ParticleSystem>();

        // Get Effect Modules.
        mainModule = FX_Splash.main;
        emitterModule = FX_Splash.emission;
    }

    public void OnTriggerEnter(Collider other)
    {
        // Start Splash.
        if (other.CompareTag("SplashArea"))
        {
            Time.timeScale = slowMotionAmount;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;


            emitterModule.enabled = true;
        }
   
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("SplashArea"))
        {
            //burstIntervalReduction += 0.01f * Time.deltaTime;
            emitterModule.SetBursts(
            new ParticleSystem.Burst[]
            {
                new ParticleSystem.Burst(0.0f, 10, 0, 0.025f - burstIntervalReduction * Time.deltaTime)
            });
        }
    }

    // Stop Splash.
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SplashArea"))
        {
            // End Slow Motion.
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02F;



            // StopSplash
            emitterModule.enabled = false;

            // Reset Splash Rate.
            emitterModule.SetBursts(
            new ParticleSystem.Burst[]
            {
                new ParticleSystem.Burst(0.0f, 3, 0, 0.025f)
            });
        }
    }
}
