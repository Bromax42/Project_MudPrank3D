using UnityEngine;

public class OpenUmbrella : MonoBehaviour
{
    public GameObject umbrella;
    private bool openedUmbrella;

    private void Start()
    {
        openedUmbrella = false;
    }
    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Splash") && !openedUmbrella)
        {
            openedUmbrella = false;
            umbrella.SetActive(true);
        }
    }
}
