using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlantButton : MonoBehaviour
{
    public string plantName;
    public string quizSceneName = "Question";

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
       PlayerPrefs.SetString("SelectedPlant", plantName);
       SceneManager.LoadScene(quizSceneName);
    }
}