using UnityEngine;

public class CarRotation : MonoBehaviour
{
    private Rigidbody rbParent;

    public float rotationTarget;
    public float turnModifier;
    public float smooth = 5.0f;

    float rotationSource;

    public bool forward, left, right;

    void Awake()
    {
        turnModifier = 0;
        // Set initial direction (forward).
        forward = true;
        left = false;
        right = false;

        rbParent = GetComponentInParent<Rigidbody>();
    }



    void Update()
    {
        if (forward)
        {
            left = false;
            right = false;

            rotationSource = rbParent.velocity.x;

            Quaternion target = Quaternion.Euler(0, rotationSource, 0);

            transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * smooth);
        }
        if (left)
        {
            forward = false;
            right = false;
            rotationSource = rbParent.velocity.z;

            Quaternion target = Quaternion.Euler(0, rotationSource, 0);

            transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * smooth);
        }

        if (right)
        {
            forward = false;
            left = false;

            rotationSource = -(rbParent.velocity.z);
        }
 



    }


}
