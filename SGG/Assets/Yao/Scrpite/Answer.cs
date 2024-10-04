using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // 添加 TextMeshPro 命名空间
using System;

public interface IQuizCompleteHandler
{
    void OnQuizComplete(int totalQuestions, int correctAnswers);
}

public class Answer : MonoBehaviour
{
    //读取文档
    string[][] ArrayX;//题目数据
    string[] lineArray;//读取到题目数据
    private int topicMax = 0;//最大题数
    private List<bool> isAnserList = new List<bool>();//存放是否答过题的状态

    //加载题目
    public GameObject tipsbtn;//提示按钮
    public TextMeshProUGUI tipsText;//提示信息
    public List<Toggle> toggleList;//答题Toggle
    public TextMeshProUGUI indexText;//当前第几题
    public TextMeshProUGUI TM_Text;//当前题目
    public List<TextMeshProUGUI> DA_TextList;//选项
    private int topicIndex = 0;//第几题
    private string currentQuizFileName;

    //按钮功能及提示信息
    public Button BtnBack;//上一题
    public Button BtnNext;//下一题
    public Button BtnTip;//消息提醒
    public TextMeshProUGUI TextAccuracy;//正确率
    private int anserint = 0;//已经答过几题
    private int isRightNum = 0;//正确题数

    // 新增接口引用
    public IQuizCompleteHandler quizCompleteHandler;

    void Awake()
    {
        TextCsv();
        LoadAnswer();
    }

    void Start()
    {
        toggleList[0].onValueChanged.AddListener((isOn) => AnswerRightWrongJudgment(isOn,0));
        toggleList[1].onValueChanged.AddListener((isOn) => AnswerRightWrongJudgment(isOn,1));
        toggleList[2].onValueChanged.AddListener((isOn) => AnswerRightWrongJudgment(isOn,2));
        toggleList[3].onValueChanged.AddListener((isOn) => AnswerRightWrongJudgment(isOn,3));

        BtnTip.onClick.AddListener(() => Select_Answer(0));
        BtnBack.onClick.AddListener(() => Select_Answer(1));
        BtnNext.onClick.AddListener(() => Select_Answer(2));
    }

    public void LoadQuizFromFile(string fileName)
    {
        currentQuizFileName = fileName;
        TextCsv();
        ResetQuiz();
        LoadAnswer();
    }

    /*****************读取txt数据******************/
    void TextCsv()
    {
        TextAsset binAsset = Resources.Load(currentQuizFileName, typeof(TextAsset)) as TextAsset;
        if (binAsset == null)
        {
            Debug.LogError($"Quiz file {currentQuizFileName} not found!");
            return;
        }

        lineArray = binAsset.text.Split('\r');
        ArrayX = new string[lineArray.Length][];
        for (int i = 0; i < lineArray.Length; i++)
        {
            ArrayX[i] = lineArray[i].Split(':');
        }
        topicMax = lineArray.Length;
    }


    /*****************加载题目******************/
    void LoadAnswer()
    {
        for (int i = 0; i < toggleList.Count; i++)
        {
            toggleList[i].isOn = false;
        }
        for (int i = 0; i < toggleList.Count; i++)
        {
            toggleList[i].interactable = true;
        }
        
        tipsbtn.SetActive(false);
        tipsText.text = "";

        indexText.text = "Question " + (topicIndex + 1);//第几题
        TM_Text.text = ArrayX[topicIndex][1];//题目
        int idx = ArrayX[topicIndex].Length - 3;//有几个选项
        for (int x = 0; x < idx; x++)
        {
            DA_TextList[x].text = ArrayX[topicIndex][x + 2];//选项
        }
    }

    void ResetQuiz()
    {
        topicIndex = 0;
        anserint = 0;
        isRightNum = 0;
        isAnserList.Clear();
        for (int x = 0; x < topicMax; x++)
        {
            isAnserList.Add(false);
        }
        TextAccuracy.text = "Accuracy 0%";
    }

    /*****************按钮功能******************/
    void Select_Answer(int index)
    {
        switch (index)
        {
            case 0://提示
                int idx = ArrayX[topicIndex].Length - 1;
                int n = int.Parse(ArrayX[topicIndex][idx]);
                string nM = "";
                switch (n)
                {
                    case 1:
                        nM = "A";
                        break;
                    case 2:
                        nM = "B";
                        break;
                    case 3:
                        nM = "C";
                        break;
                    case 4:
                        nM = "D";
                        break;
                }
                tipsText.text = "<color=#FFAB08FF>" +"Correct Answer is "+ nM + "</color>";
                break;
            case 1://上一题
                if (topicIndex > 0)
                {
                    topicIndex--;
                    LoadAnswer();
                }
                else
                {
                    tipsText.text = "<color=#27FF02FF>" + "Not more question before" + "</color>";
                }
                break;
            case 2://下一题
                if (topicIndex < topicMax-1)
                {
                    topicIndex++;
                    LoadAnswer();
                }
                else
                {
                    tipsText.text = "<color=#27FF02FF>" + "Already is last one" + "</color>";
                    // 检查是否所有题目都已回答
                    if (IsQuizComplete())
                    {
                        OnQuizComplete();
                    }
                }
                break;
        }
    }

    /*****************题目对错判断******************/
    void AnswerRightWrongJudgment(bool check, int index)
    {
        if (check)
        {
            //判断题目对错
            bool isRight;
            int idx = ArrayX[topicIndex].Length - 1;
            int n = int.Parse(ArrayX[topicIndex][idx]) - 1;
            if (n == index)
            {
                tipsText.text = "<color=#27FF02FF>" + "Correct!" + "</color>";
                isRight = true;
                tipsbtn.SetActive(true);
            }
            else
            {
                tipsText.text = "<color=#FF0020FF>" + "Sorry, wrong answer" + "</color>";
                isRight = false;
                tipsbtn.SetActive(true);
            }

            //正确率计算
            if (isAnserList[topicIndex])
            {
                tipsText.text = "<color=#FF0020FF>" + "Already answered" + "</color>";
            }
            else
            {
                anserint++;
                if (isRight)
                {
                    isRightNum++;
                }
                isAnserList[topicIndex] = true;
                TextAccuracy.text = "Accuracy " + ((float)isRightNum / anserint * 100).ToString("f2") + "%";
            }

            //禁用掉选项
            for (int i = 0; i < toggleList.Count; i++)
            {
                toggleList[i].interactable = false;
            }

            // 在回答完最后一题后检查是否完成测验
            if (topicIndex == topicMax - 1 && IsQuizComplete())
            {
                OnQuizComplete();
            }
        }
    }

    bool IsQuizComplete()
    {
        return isAnserList.TrueForAll(answered => answered);
    }

    void OnQuizComplete()
    {
        if (quizCompleteHandler != null)
        {
            quizCompleteHandler.OnQuizComplete(topicMax, isRightNum);
        }
        Debug.Log($"Quiz completed! Total questions: {topicMax}, Correct answers: {isRightNum}");
    }

    // 可以添加一个方法来设置 quizCompleteHandler
    public void SetQuizCompleteHandler(IQuizCompleteHandler handler)
    {
        quizCompleteHandler = handler;
    }
}