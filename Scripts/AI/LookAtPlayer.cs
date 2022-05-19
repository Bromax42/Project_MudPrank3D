using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{

    [SerializeField]
    GameObject player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform.position);
    }
}
