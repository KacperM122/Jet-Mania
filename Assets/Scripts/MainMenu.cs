using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject titleScreen;
    public GameObject descriptionScreen;

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("SampleScene");
    }

    public void ShowLevelDescription()
    {
        titleScreen.SetActive(false);
        descriptionScreen.SetActive(true);
    }
}
