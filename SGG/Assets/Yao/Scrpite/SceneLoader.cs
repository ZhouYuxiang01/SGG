
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlantButton : MonoBehaviour
{
    public string plantName;
    public string quizSceneName = "QuizScene";

    private void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
        else
        {
            Debug.LogError("Button component not found on this GameObject!");
        }
    }

    private void OnButtonClick()
    {
        PlayerPrefs.SetString("SelectedPlant", plantName);
        SceneManager.LoadScene(quizSceneName);
    }
}