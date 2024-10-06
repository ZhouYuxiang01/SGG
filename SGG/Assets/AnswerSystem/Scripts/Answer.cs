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

    // 统计信息
    public TextMeshProUGUI TextAccuracy;
    private int topicIndex = 0;
    private int anserint = 0;
    private int isRightNum = 0;
    private float displayedAccuracy = 0f;

    private QuizManager quizManager;
    private string currentPlantName;
    private const float PASS_THRESHOLD = 0.6f; // 60% 通过阈值

    void Start()
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
        if (topicIndex >= topicMax)
        {
            OnQuizComplete();
            return;
        }

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
        if (topicIndex >= topicMax)
        {
            return;
        }

        indexText.text = "Question " + (topicIndex + 1);
        TM_Text.text = ArrayX[topicIndex][1];
        int optionCount = Mathf.Min(ArrayX[topicIndex].Length - 3, DA_TextList.Count);

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
        if (topicIndex >= topicMax) return;

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
            OnQuizComplete();
        }
    }

    private void AnswerRightWrongJudgment(bool isOn, int index)
    {
        if (!isOn || topicIndex >= topicMax) return;

        int correctAnswerIndex = int.Parse(ArrayX[topicIndex][ArrayX[topicIndex].Length - 1]) - 1;
        bool isCorrect = (index == correctAnswerIndex);

        UpdateTipsForAnswer(isCorrect);
        UpdateAccuracy(isCorrect);
        DisableToggles();

        if (topicIndex == topicMax - 1)
        {
            OnQuizComplete();
        }
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
            displayedAccuracy = (float)isRightNum / anserint * 100;
            TextAccuracy.text = $"Accuracy {displayedAccuracy:F2}%";
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

    private void OnQuizComplete()
    {
        Debug.Log($"Quiz completed for {currentPlantName}. Displayed accuracy: {displayedAccuracy:F2}%");

        string resultMessage;
        if (displayedAccuracy >= PASS_THRESHOLD * 100)
        {
            resultMessage = "\n<color=#27FF02FF>Congratulations! You passed the quiz and unlocked the plant!</color>";
            if (quizManager != null)
            {
                quizManager.SetPlantUnlockStatus(currentPlantName, true);
                Debug.Log($"{currentPlantName} unlocked!");
            }
            else
            {
                Debug.LogError("QuizManager is null, can't update plant unlock status!");
            }
        }
        else
        {
            resultMessage = "\n<color=#FF0020FF>Quiz failed. Try again to unlock the plant.</color>";
            Debug.Log($"Quiz failed. Required accuracy: {PASS_THRESHOLD * 100:F2}%");
        }

        // 更新UI显示最终结果
        TextAccuracy.text += " " + resultMessage;

        // 禁用所有答题按钮和导航按钮
        DisableToggles();
        BtnBack.interactable = false;
        BtnNext.interactable = false;
        BtnTip.interactable = false;

        // 这里可以添加返回选择植物场景的逻辑
        // 例如：SceneManager.LoadScene("SelectPlantScene");
    }
}