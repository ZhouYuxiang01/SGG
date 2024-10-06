using UnityEngine;
using System.Collections.Generic;

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

    private void Start()
    {
        answerScript = FindObjectOfType<Answer>();
        string selectedPlant = PlayerPrefs.GetString("SelectedPlant", "");
        if (!string.IsNullOrEmpty(selectedPlant))
        {
            LoadQuizForPlant(selectedPlant);
        }
    }

    private void LoadQuizForPlant(string plantName)
    {
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
    }
}