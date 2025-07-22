using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject InfoPanel;
    public GameObject SelectPanel;

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

    public void SelectGrid(int size)
    {
        GameManager.instance.SetupGrid(size);
        SelectPanel.SetActive(false);
    }
    
}
