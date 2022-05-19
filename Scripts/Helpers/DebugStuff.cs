using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugStuff : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadScene("LEVEL");
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            PlayerPrefs.DeleteAll();
        }
    }

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }


}
