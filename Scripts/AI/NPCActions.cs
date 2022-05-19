using System.Collections;
using UnityEngine;

public class NPCActions : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject player;

    // Declare Wet Effect Emission Module
    private ParticleSystem.EmissionModule FX_Wet_Emission;

    // Health and Damage variables.
    public float health = 100;
    public float splashDamage = 3.0f;
    private bool lowDamage, midDamage, highDamage;


    public bool kid, old, corpo;

    bool canYieldStars = true;

    Animator animator;


    void Start()
    {
        lowDamage = true; midDamage = true; highDamage = true;

        //Connect to Game Manager and get Player.
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        player = gameManager.player;

        // Get Animator.
        animator = GetComponent<Animator>();

        // Get Dripping Wet Effect.
        FX_Wet_Emission = transform.GetChild(0).GetComponent<ParticleSystem>().emission;
    }


    //Splash Results:
    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Splash"))
        {
            // Get Splash Damage (1-2-3) and Player Speed.
            float carSpeed = player.GetComponent<Rigidbody>().velocity.z;
            float totalDamage = splashDamage + (carSpeed / 10);                     //  Debug.Log("total damage on " + transform.name+ " is " + totalDamage);

            // Take damage for each particle hit.
            health -= totalDamage;                                                  //  Debug.Log(health);

            // Initiate Self Destruction.
            StartCoroutine(DisableNPC());

            // Gain coins.
            if (canYieldStars)
            {
                canYieldStars = false;
                gameManager.starsEarned++; 
            }

            // Activate FX_Wet:
            FX_Wet_Emission.enabled = true;




            // Animate damage reaction according to Health.


            if (health >= 66.6)
            {
                LowDamage();

                // Activate Emoji Effect.                
            }
            
            else if (health > 33.3 && health < 66.6)
            {
                MidDamage();
                // Activate Harder Emoji Effect.
                
            }
            
            else
            {
                HighDamage();
                // Activate Hardest Emoji Effect.
               
            }
            
        }
    }

    //animator.applyRootMotion = true;
    void LowDamage()
    {
        if (lowDamage)
        {
   //         Debug.Log("LOW DAMAGE ACTIVATED.");
            lowDamage = false;

            if (kid)
            {
                // Activate a random reaction of Level 1.
                animator.SetFloat("ReactionNumber1", Random.Range(1,3));
            }
            else if (old)
            {
                animator.SetFloat("ReactionNumber1", Random.Range(1, 2));
            }
            // Corpo has a single Reaction 1. All good.

            // Trigger Animation.
            animator.SetTrigger("React1");
        }
    }

    void MidDamage()
    {
        if (midDamage)
        {
            midDamage = false;
   //         Debug.Log("MID DAMAGE ACTIVATED.");

            if (kid || old)
            {
                // Activate random reaction of Level 2.
                animator.SetFloat("ReactionNumber2", Random.Range(1, 3));
            }

            // Trigger Level 2 Animation.
            animator.SetTrigger("React2");
        }
    }

    void HighDamage()
    { 
        if (highDamage)
        {
    //        Debug.Log("HIGH DAMAGE ACTIVATED.");
            highDamage = false;

            if (corpo)
            {
                animator.SetFloat("ReactionNumber3", Random.Range(1, 3));
            }

            animator.SetTrigger("React3");
        }
    }

    // Deactivates Root-Motion for Non-Compatible Animation Clips.
    void RemoveRM()
    {
        animator.applyRootMotion = false;
    }

    IEnumerator DisableNPC()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

    public void CancelDeath()
    {
        StopAllCoroutines();
        animator.Rebind();
        animator.Update(0f);
    }
}
