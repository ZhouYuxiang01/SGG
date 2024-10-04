using UnityEngine;
using System.Collections.Generic;

public class QuizManager : MonoBehaviour
{
    [System.Serializable]
    public class PlantQuiz
    {
        public string plantName;
        public string quizFileName;
    }

    public List<PlantQuiz> plantQuizzes = new List<PlantQuiz>();
    public Answer answerScript;

    private void Start()
    {
        // 确保 Answer 脚本被正确引用
        if (answerScript == null)
        {
            answerScript = FindObjectOfType<Answer>();
            if (answerScript == null)
            {
                Debug.LogError("Answer script not found!");
                return;
            }
        }
    }

    public void LoadQuizForPlant(string plantName)
    {
        PlantQuiz quiz = plantQuizzes.Find(q => q.plantName == plantName);
        if (quiz != null)
        {
            answerScript.LoadQuizFromFile(quiz.quizFileName);
        }
        else
        {
            Debug.LogError($"Quiz for plant {plantName} not found!");
        }
    }

    // 可以添加更多方法来管理题库，如添加新的植物题库、移除题库等
}