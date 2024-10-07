using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlantButton : MonoBehaviour
{
    public string quizSceneName = "Question";

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        PlantDisplaySystem plantDisplaySystem = FindObjectOfType<PlantDisplaySystem>();

        if (plantDisplaySystem != null)
        {
            string currentPlantName = plantDisplaySystem.GetCurrentPlantName();
            if (!string.IsNullOrEmpty(currentPlantName))
            {
                PlayerPrefs.SetString("SelectedPlant", currentPlantName);
                LoadQuizScene();
            }
            else
            {
                Debug.LogWarning("No plant is currently selected.");
            }
        }
        else
        {
            Debug.LogError("PlantDisplaySystem not found in the scene!");
        }
    }

    private void LoadQuizScene()
    {
        Scene quizScene = SceneManager.GetSceneByName(quizSceneName);
        if (quizScene.isLoaded)
        {
            // 如果已加载，直接激活
            SceneManager.SetActiveScene(quizScene);
        }
        else
        {
            // 如果未加载，使用叠加模式加载
            SceneManager.LoadScene(quizSceneName, LoadSceneMode.Additive);
        }

        // 可选：卸载当前活动场景（如果不是问题场景）
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name != quizSceneName)
        {
            SceneManager.UnloadSceneAsync(currentScene);
        }
    }
}