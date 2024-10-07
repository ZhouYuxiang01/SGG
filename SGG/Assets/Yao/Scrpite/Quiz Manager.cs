using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    [System.Serializable]
    public class PlantQuiz
    {
        public string plantName;
        public TextAsset quizTextAsset;
    }

    public List<PlantQuiz> plantQuizzes = new List<PlantQuiz>();
    private Answer answerScript;

    private void Awake()
    {
        // ȷ��ֻ��һ�� QuizManager ʵ��
        if (FindObjectsOfType<QuizManager>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        InitializeQuiz();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Question")
        {
            InitializeQuiz();
        }
    }

    private void InitializeQuiz()
    {
        FindAnswerScript();
        string selectedPlant = PlayerPrefs.GetString("SelectedPlant", "");
        if (!string.IsNullOrEmpty(selectedPlant))
        {
            LoadQuizForPlant(selectedPlant);
        }
    }

    private void FindAnswerScript()
    {
        answerScript = FindObjectOfType<Answer>();
        if (answerScript == null)
        {
            Debug.LogError("Answer script not found in the scene!");
        }
    }

    public void LoadQuizForPlant(string plantName)
    {
        if (answerScript == null)
        {
            FindAnswerScript();
        }

        PlantQuiz quiz = plantQuizzes.Find(q => q.plantName == plantName);
        if (quiz != null && quiz.quizTextAsset != null)
        {
            answerScript.InitializeQuiz(plantName, quiz.quizTextAsset.text);
        }
        else
        {
            Debug.LogError($"Quiz for plant {plantName} not found or text asset is missing!");
        }
    }

    public void SetPlantUnlockStatus(string plantName, bool isUnlocked)
    {
        PlayerPrefs.SetInt("Plant_" + plantName, isUnlocked ? 1 : 0);
        PlayerPrefs.Save();

        // ֪ͨ��һ�������е� PlantDictionary ������ʾ
        UpdatePlantDictionaryInMainScene(plantName);
    }

    private void UpdatePlantDictionaryInMainScene(string plantName)
    {
        // ����������
        Scene mainScene = SceneManager.GetSceneAt(0);
        if (mainScene.isLoaded)
        {
            // ���������в��� PlantDictionary
            GameObject[] rootObjects = mainScene.GetRootGameObjects();
            foreach (var rootObject in rootObjects)
            {
                PlantDictionary dictionary = rootObject.GetComponentInChildren<PlantDictionary>();
                if (dictionary != null)
                {
                    dictionary.UpdatePlantDisplay(plantName);
                    break;
                }
            }
        }
    }

    public bool IsPlantUnlocked(string plantName)
    {
        return PlayerPrefs.GetInt("Plant_" + plantName, 0) == 1;
    }
}