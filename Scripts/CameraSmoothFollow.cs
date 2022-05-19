using UnityEngine;

public class CameraSmoothFollow : MonoBehaviour
{
    public GameManager gameManager;
    public Transform player;
    public Transform playerFocusPoint;
    public Transform finish;
    public Transform target;

    [SerializeField]
    private float smoothing = 0.125f;

    
    public Vector3 offset;
    public Vector3 externalOffset;

    Vector3 velocity = Vector3.zero;

    private void Start()
    {
        // Connect to GameManager.
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        // Get Focusable Targets.
        player = gameManager.player.transform;
        playerFocusPoint = player.Find("Camera Focus Points").Find("FocusPoint");
        finish = player.Find("Camera Focus Points").Find("FinishFocusPoint").transform;
        target = playerFocusPoint;
    }

    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset + externalOffset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothing);
        transform.position = smoothedPosition;

        transform.LookAt(playerFocusPoint);
    }

    public void FocusOnFinish()
    {
        target = finish; 
    }

    public void FocusOnPlayer()
    {
        target = playerFocusPoint;
    }
}
