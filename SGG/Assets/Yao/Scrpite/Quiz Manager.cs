using UnityEngine;
using System.Collections.Generic;

public class QuizManager : MonoBehaviour
{
    public static QuizManager Instance { get; private set; }

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
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        string selectedPlant = PlayerPrefs.GetString("SelectedPlant", "");
        if (!string.IsNullOrEmpty(selectedPlant))
        {
            LoadQuizForPlant(selectedPlant);
        }
    }

    public void LoadQuizForPlant(string plantName)
    {
        if (answerScript == null)
        {
            answerScript = FindObjectOfType<Answer>();
        }

        if (answerScript == null)
        {
            Debug.LogError("Answer script not found in the scene!");
            return;
        }

        PlantQuiz quiz = plantQuizzes.Find(q => q.plantName == plantName);
        if (quiz != null && quiz.quizTextAsset != null)
        {
            answerScript.InitializeQuiz(quiz.quizTextAsset.text);
        }
        else
        {
            Debug.LogError($"Quiz for plant {plantName} not found or text asset is missing!");
        }
    }

    public TextAsset GetQuizTextAsset(string plantName)
    {
        PlantQuiz quiz = plantQuizzes.Find(q => q.plantName == plantName);
        return quiz?.quizTextAsset;
    }
}