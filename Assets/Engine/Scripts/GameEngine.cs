using Jint;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEngine : MonoBehaviour
{
    Engine jint = new Engine();

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        SceneManager.LoadScene("Authentication");
    }
}
