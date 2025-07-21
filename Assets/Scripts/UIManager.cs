using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    GameObject InfoPanel;

    private void Awake()
    {
        if(instance = null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void MainScene()
    {
        SceneManager.LoadScene("Main Scene");
    }

    public void StartScene()
    {
        SceneManager.LoadScene("Start Scene");
    }

    public void OpenInfo()
    {
        InfoPanel.SetActive(true);
    }

    public void CloseInfo()
    {
        InfoPanel.SetActive(false);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InfoPanel = GameObject.Find("Info Panel");
    }
    
}
