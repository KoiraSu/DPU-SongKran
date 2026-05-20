using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneName);
    }
}