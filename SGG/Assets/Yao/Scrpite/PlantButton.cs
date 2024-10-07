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
            // ����Ѽ��أ�ֱ�Ӽ���
            SceneManager.SetActiveScene(quizScene);
        }
        else
        {
            // ���δ���أ�ʹ�õ���ģʽ����
            SceneManager.LoadScene(quizSceneName, LoadSceneMode.Additive);
        }

        // ��ѡ��ж�ص�ǰ�����������������ⳡ����
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name != quizSceneName)
        {
            SceneManager.UnloadSceneAsync(currentScene);
        }
    }
}