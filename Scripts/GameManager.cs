using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ElephantSDK;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public GameObject TyreLeft, TyreRight;
    public GameObject player;

    // Declare UI Elements.
    public GameObject UI_Canvas, Pan_Transition, Img_LevelPanel;
    public GameObject But_Start, But_Continue, But_Restart;
    public GameObject Img_Win, Img_Lose;
    public GameObject Img_Stars;
    public Text Text_StarsCollected, Text_LevelCompleted, Text_CurrentLevel;

    public Slider Sli_ProgressBar;
    public Transform currentFinishLine;

    float totalDistance;
    float distancePassed;
    float progress;

    public CarRotation carRotation;


    // Declare Score elements.
    public int starsEarned = 0;
    public int totalStars = 0;

    public ActivateFXStars activateFXStars;

    // Player Car elements.
    public float currentHorizontalPosition;
    public int carHealth;
    public CarMovement carMovement;
    

    public int currentLevel = 0;
    public Transform startPosition;

    public Transform[] levels = new Transform[6];

    // NPC Reset along with current Level.
    private GameObject[] CurrentNPCs = new GameObject[16];
    public int NPC_count;

    private Transform[] NPC_positions = new Transform[16];
    private Vector3[] NPC_Rotation = new Vector3[16];

    [SerializeField] private GameObject finishCam;
    [SerializeField] private GameObject normalCam;
    [SerializeField] private GameObject cameraPosition;
 
    //Make Singleton and Connect with other elements.
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        // Get Inter-Level Constants.
        TyreLeft = GameObject.FindGameObjectWithTag("TyreLeft");
        TyreRight = GameObject.FindGameObjectWithTag("TyreRight");
        player = GameObject.FindGameObjectWithTag("Player");

        carRotation = player.GetComponentInChildren<CarRotation>();



        // Get UI Elements.
        UI_Canvas = GameObject.FindGameObjectWithTag("Canvas");
        Pan_Transition = UI_Canvas.transform.Find("Pan-Transition").gameObject;

        But_Continue = Pan_Transition.transform.Find("But-Continue").gameObject;
        But_Start = UI_Canvas.transform.Find("But-Start").gameObject;
        But_Restart = Pan_Transition.transform.Find("But-Restart").gameObject;

        Img_Stars = UI_Canvas.transform.Find("Img-StarPanel").Find("Img-Stars").gameObject;
        Text_StarsCollected = Img_Stars.GetComponentInChildren<Text>();

        Img_Win = Pan_Transition.transform.Find("Img-Win").gameObject;
        Img_Lose = Pan_Transition.transform.Find("Img-Lose").gameObject;
        Text_LevelCompleted = Img_Win.GetComponentInChildren<Text>();

        Sli_ProgressBar = UI_Canvas.GetComponentInChildren<Slider>();
        currentFinishLine = GameObject.FindGameObjectWithTag("FinishLine").transform;
        Img_LevelPanel = Sli_ProgressBar.transform.Find("Img-LevelPanel").gameObject;
        Text_CurrentLevel = Img_LevelPanel.transform.GetComponentInChildren<Text>();
        Text_CurrentLevel.text = ((currentLevel + 1).ToString());

        // Progressbar Calculations.
        totalDistance = currentFinishLine.position.z - startPosition.position.z;

        // Get Visual Effects.
        activateFXStars = GameObject.FindGameObjectWithTag("Stars").GetComponent<ActivateFXStars>();

        // Get/Set Player Elements.
        carHealth = 100;
        carMovement = player.GetComponent<CarMovement>();

        // Get a list of NPC's for the first time.
        NPC_count = levels[currentLevel].Find("NPC").childCount;
        for (int i = 0; i < NPC_count; i++)
        {
            CurrentNPCs[i] = levels[currentLevel].Find("NPC").GetChild(i).gameObject;
            NPC_positions[i] = CurrentNPCs[i].transform;
            Debug.Log(CurrentNPCs[i]);
        }


        // ELEPHANT SDK - Send App Initiation Level Message.
        Elephant.LevelStarted((currentLevel + 1));

        //
        // LOAD GAME.
        //
        if (PlayerPrefs.HasKey("playerX") && PlayerPrefs.HasKey("playerY") && PlayerPrefs.HasKey("playerZ"))
        {
            //Debug.Log("We have a Save File.");

            // Teleport Player to saved Position.
            player.transform.position = new Vector3(PlayerPrefs.GetFloat("playerX"), PlayerPrefs.GetFloat("playerY"), PlayerPrefs.GetFloat("playerZ"));
            currentLevel = PlayerPrefs.GetInt("savedLevel");

            // Close ALL Levels and Stages.
            for (int i = 0; i < levels.Length; i++ )
            {
                levels[i].gameObject.SetActive(false);
                levels[i].parent.gameObject.SetActive(false);
            }
            // Reopen the Current used Level and its Stage.
            levels[currentLevel].gameObject.SetActive(true);
            levels[currentLevel].parent.gameObject.SetActive(true);

            // Set the Level Displayer.
            Text_CurrentLevel.text = ((currentLevel + 1).ToString());

            // Restore the Stars.
            totalStars = PlayerPrefs.GetInt("savedStars");

        }

    }


    void Update()
    {
        // ProgressBar Calculation.
        distancePassed = player.transform.position.z - startPosition.position.z;
        progress = (distancePassed) / totalDistance;

        // Updating Progress Bar.
        Sli_ProgressBar.value = progress;                                                       // Debug.Log(progress);

    }


    public void LoadNextLevel() // via Button-Continue.
    {
        // ELEPHANT SDK - Send Level Completed Message.
        Elephant.LevelCompleted((currentLevel + 1));



        // Reset Player Health.
        carHealth = 100;

        // Reset Camera
        Camera.main.GetComponent<CameraSmoothFollow>().offset = new Vector3(0f, 0.175f, -0.4f);

        // Reset Stars Gained at the end of the level.
        starsEarned = 0;

        // Deactivate Finished Level and Activate the next.
        levels[currentLevel].gameObject.SetActive(false);

        // Deactivate current Stage if it has ended.
        if (currentLevel == 2 || currentLevel == 6)
        {
            levels[currentLevel].parent.gameObject.SetActive(false);
        }
        currentLevel++;

        // Update Current-Level Texts.
        Text_CurrentLevel.text = ((currentLevel + 1).ToString());

        levels[currentLevel].gameObject.SetActive(true);

        if (currentLevel == 3 || currentLevel == 7)
        {
            levels[currentLevel].parent.gameObject.SetActive(true);
        }

        // Update NPC list.
        NPC_count = levels[currentLevel].Find("NPC").childCount;
        for (int i = 0; i < NPC_count; i++)
        {
            CurrentNPCs[i] = levels[currentLevel].Find("NPC").GetChild(i).gameObject;
            NPC_positions[i] = CurrentNPCs[i].transform;
            CurrentNPCs[i].GetComponent<Animator>().Rebind();
            CurrentNPCs[i].GetComponent<Animator>().Update(0f);
            CurrentNPCs[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            CurrentNPCs[i].GetComponent<Rigidbody>().isKinematic = true;

            Debug.Log(CurrentNPCs[i]);
        }

        // Teleport player to Next Level
        // and set car's new horizontal position limit.
        startPosition = levels[currentLevel].Find("StartPosition");
        player.transform.SetPositionAndRotation(startPosition.position, startPosition.rotation);

        PlayerPrefs.SetFloat("playerX", player.transform.position.x);
        PlayerPrefs.SetFloat("playerY", player.transform.position.y);
        PlayerPrefs.SetFloat("playerZ", player.transform.position.z);
        PlayerPrefs.Save();    
        
        /*
        Debug.Log("Set the player position to: " + PlayerPrefs.GetFloat("playerX"));
        Debug.Log("Set the player position to: " + PlayerPrefs.GetFloat("playerY"));
        Debug.Log("Set the player position to: " + PlayerPrefs.GetFloat("playerZ"));
        */

        // Switch Panel Buttons.
        But_Continue.SetActive(false);
        Img_Win.SetActive(false);
        Pan_Transition.SetActive(false);
        But_Start.SetActive(true);

        // Focus Camera on the player
        CinemachineVirtualCamera vCam = normalCam.GetComponent<CinemachineVirtualCamera>();
        finishCam.SetActive(false);

        // Reset Level Progression Bar.
        currentFinishLine = GameObject.FindGameObjectWithTag("FinishLine").transform;
        totalDistance = currentFinishLine.position.z - startPosition.position.z;

        // SAVE GAME.
        PlayerPrefs.SetInt("savedLevel", currentLevel);
        PlayerPrefs.SetInt("savedStars", totalStars);
        PlayerPrefs.Save();
        //Debug.Log("Saved Level: " + (currentLevel+1));


        // ELEPHANT - Update Current Level.
        Elephant.LevelStarted((currentLevel + 1));


    }



    public void RestartLevel()
    {
        Elephant.LevelFailed((currentLevel + 1));

        // Reset Stars Gained.
        starsEarned = 0;

        // Stop the car.
        carMovement.rb.isKinematic = true;
        carMovement.enabled = false;

        // Activate Pan-Transition.
        Pan_Transition.SetActive(true);
        Img_Lose.SetActive(true);
        StartCoroutine(RestartTimer());

        finishCam.SetActive(false);

        carHealth = 100;
    }

    IEnumerator RestartTimer()
    {
        yield return new WaitForSeconds(1);
        But_Restart.SetActive(true);

    }

    public void RestartTheCar() // via Button-Restart.
    {
        
        Pan_Transition.SetActive(false);
        But_Restart.SetActive(false);
        Img_Lose.SetActive(false);
        But_Start.SetActive(true);
        player.transform.SetPositionAndRotation(startPosition.position, Quaternion.identity);
        carMovement.rb.isKinematic = false;
        Camera.main.GetComponent<CameraSmoothFollow>().enabled = true;



        // Activate Killed NPC's.
        for (int i = 0; i < NPC_count; i++)
        {
            CurrentNPCs[i].SetActive(true);
            CurrentNPCs[i].GetComponent<NPCActions>().CancelDeath();
            CurrentNPCs[i].transform.position = NPC_positions[i].position;
        }

        finishCam.SetActive(false);
        StartCoroutine(WaitBeforeRespawn());
    }

    IEnumerator WaitBeforeRespawn()
    {
        yield return new WaitForSeconds(0.75f);
        But_Start.SetActive(true);
    }

    public void StartFX_Stars()
    {
        activateFXStars.starsEarned = starsEarned;
        activateFXStars.StartCoroutine(activateFXStars.StartEmission());
    }
    public void GainStars()
    {
        totalStars += Random.Range(3, 5);
        Text_StarsCollected.text = totalStars.ToString();
    }

    public void EnableFinishCam()
    {

        finishCam.SetActive(true);
    }

    void OnApplicationQuit()
    {
        Elephant.LevelFailed((currentLevel + 1));
    }

}
