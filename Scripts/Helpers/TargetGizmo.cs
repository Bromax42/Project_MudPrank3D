using UnityEngine;

public class TargetGizmo : MonoBehaviour
{
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "downArrow.png", true, Color.green);

    }
}
