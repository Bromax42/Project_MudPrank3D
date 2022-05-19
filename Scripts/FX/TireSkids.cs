using UnityEngine;

public class TireSkids : MonoBehaviour
{
    public TrailRenderer trailLeft, trailRight, skids; 
    [SerializeField] float minimumAngle = 15.0f;

    private void Awake()
    {
        skids = GetComponentInChildren<TrailRenderer>();
    }
    void FixedUpdate()
    {
        // Set skitting effect according to Y angle.
        float absoluteRotation = (transform.rotation.eulerAngles.y);            //Debug.Log("Absolute Y rotation is: " + absoluteRotation);
        if (absoluteRotation > minimumAngle && absoluteRotation < 360-minimumAngle)
        {
            skids.emitting = true;
            // Stop Effect in seconds.
            // do stuff.
        }
        else
        {
            skids.emitting = false;
        }
    }

}
