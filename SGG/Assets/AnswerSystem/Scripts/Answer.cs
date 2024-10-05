using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Answer : MonoBehaviour
{
    // 题目数据
    private string[][] ArrayX;
    private string[] lineArray;
    private int topicMax = 0;
    private List<bool> isAnserList = new List<bool>();

    // UI 元素
    public GameObject tipsbtn;
    public TextMeshProUGUI tipsText;
    public List<Toggle> toggleList;
    public TextMeshProUGUI indexText;
    public TextMeshProUGUI TM_Text;
    public List<TextMeshProUGUI> DA_TextList;

    // 按钮
    public Button BtnBack;
    public Button BtnNext;
    public Button BtnTip;
    private QuizManager quizManager;
    private string currentPlantName;

    // 统计信息
    public TextMeshProUGUI TextAccuracy;
    private int topicIndex = 0;
    private int anserint = 0;
    private int isRightNum = 0;

    private void Start()
    {
        SetupListeners();
        quizManager = FindObjectOfType<QuizManager>();
        if (quizManager == null)
        {
            Debug.LogError("QuizManager not found in the scene!");
        }
    }

    private void SetupListeners()
    {
        for (int i = 0; i < toggleList.Count; i++)
        {
            int index = i;
            toggleList[i].onValueChanged.AddListener((isOn) => AnswerRightWrongJudgment(isOn, index));
        }

        BtnTip.onClick.AddListener(() => Select_Answer(0));
        BtnBack.onClick.AddListener(() => Select_Answer(1));
        BtnNext.onClick.AddListener(() => Select_Answer(2));
    }

    public void InitializeQuiz(string plantName, string quizText)
    {
        if (string.IsNullOrEmpty(plantName) || string.IsNullOrEmpty(quizText))
        {
            Debug.LogError("Plant name or quiz text is null or empty!");
            return;
        }

        currentPlantName = plantName;
        ParseQuizText(quizText);
        ResetQuiz();
        LoadAnswer();
    }

    private void ParseQuizText(string quizText)
    {
        lineArray = quizText.Split('\n');
        ArrayX = new string[lineArray.Length][];
        for (int i = 0; i < lineArray.Length; i++)
        {
            ArrayX[i] = lineArray[i].Split(':');
        }
        topicMax = lineArray.Length;
        isAnserList.Clear();
        for (int x = 0; x < topicMax; x++)
        {
            isAnserList.Add(false);
        }
    }

    private void ResetQuiz()
    {
        topicIndex = 0;
        anserint = 0;
        isRightNum = 0;
        TextAccuracy.text = "Accuracy 0%";
    }

    private void LoadAnswer()
    {
        ResetToggles();
        ResetTips();
        UpdateQuestionDisplay();
    }

    private void ResetToggles()
    {
        foreach (var toggle in toggleList)
        {
            toggle.isOn = false;
            toggle.interactable = true;
        }
    }

    private void ResetTips()
    {
        tipsbtn.SetActive(false);
        tipsText.text = "";
    }

    private void UpdateQuestionDisplay()
    {
        indexText.text = "Question " + (topicIndex + 1);
        TM_Text.text = ArrayX[topicIndex][1];
        int optionCount = ArrayX[topicIndex].Length - 3;
        
        for (int x = 0; x < DA_TextList.Count; x++)
        {
            if (x < optionCount)
            {
                DA_TextList[x].text = ArrayX[topicIndex][x + 2];
                DA_TextList[x].transform.parent.gameObject.SetActive(true);
            }
            else
            {
                DA_TextList[x].transform.parent.gameObject.SetActive(false);
            }
        }
    }

    private void Select_Answer(int index)
    {
        switch (index)
        {
            case 0: // 提示
                ShowTip();
                break;
            case 1: // 上一题
                NavigateToPreviousQuestion();
                break;
            case 2: // 下一题
                NavigateToNextQuestion();
                break;
        }
    }

    private void ShowTip()
    {
        int correctAnswerIndex = int.Parse(ArrayX[topicIndex][ArrayX[topicIndex].Length - 1]) - 1;
        string correctAnswerLetter = "ABCD"[correctAnswerIndex].ToString();
        tipsText.text = $"<color=#FFAB08FF>Correct Answer is {correctAnswerLetter}</color>";
    }

    private void NavigateToPreviousQuestion()
    {
        if (topicIndex > 0)
        {
            topicIndex--;
            LoadAnswer();
        }
        else
        {
            tipsText.text = "<color=#27FF02FF>Not more question before</color>";
        }
    }

    private void NavigateToNextQuestion()
    {
        if (topicIndex < topicMax - 1)
        {
            topicIndex++;
            LoadAnswer();
        }
        else
        {
            tipsText.text = "<color=#27FF02FF>Already is last one</color>";
        }
    }

    private void CheckQuizCompletion()
    {
        if (anserint == topicMax)
        {
            float correctRate = (float)isRightNum / topicMax;
            if (quizManager != null)
            {
                quizManager.OnQuizComplete(currentPlantName, correctRate);
            }
            else
            {
                Debug.LogError("QuizManager is null, can't report quiz completion!");
            }
        }
    }

    private void AnswerRightWrongJudgment(bool isOn, int index)
    {
        if (!isOn) return;

        int correctAnswerIndex = int.Parse(ArrayX[topicIndex][ArrayX[topicIndex].Length - 1]) - 1;
        bool isCorrect = (index == correctAnswerIndex);

        UpdateTipsForAnswer(isCorrect);
        UpdateAccuracy(isCorrect);
        DisableToggles();

        CheckQuizCompletion();
    }

    private void UpdateTipsForAnswer(bool isCorrect)
    {
        tipsText.text = isCorrect ? "<color=#27FF02FF>Correct!</color>" : "<color=#FF0020FF>Sorry, wrong answer</color>";
        tipsbtn.SetActive(true);
    }

    private void UpdateAccuracy(bool isCorrect)
    {
        if (!isAnserList[topicIndex])
        {
            anserint++;
            if (isCorrect) isRightNum++;
            isAnserList[topicIndex] = true;
            TextAccuracy.text = $"Accuracy {((float)isRightNum / anserint * 100):F2}%";
        }
        else
        {
            tipsText.text = "<color=#FF0020FF>Already answered</color>";
        }
    }

    private void DisableToggles()
    {
        foreach (var toggle in toggleList)
        {
            toggle.interactable = false;
        }
    }
}