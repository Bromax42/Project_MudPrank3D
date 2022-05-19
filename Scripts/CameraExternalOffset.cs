using UnityEngine;

public class CameraExternalOffset : MonoBehaviour
{
    public Vector3 extertalOffset;
    public float steerModifier = 2.5f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            Camera.main.GetComponent<CameraSmoothFollow>().externalOffset = extertalOffset;
            other.transform.GetComponentInParent<CarMovement>().steerSpeed *= steerModifier;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            Camera.main.GetComponent<CameraSmoothFollow>().externalOffset = new Vector3(0, 0, 0);
            other.transform.GetComponentInParent<CarMovement>().steerSpeed /= steerModifier;
        }
    }

}
