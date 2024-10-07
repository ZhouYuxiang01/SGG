using UnityEngine;
using UnityEngine.UI; // 引入 UI 命名空间

public class DisplayTextOnTrigger : MonoBehaviour
{
    public Text pressEText; // 公开的 Text 对象，用于显示 "Press E"
    // 如果使用 TextMeshPro 则可以改为 TMP_Text 并引入 TMPro 命名空间
    // public TMP_Text pressEText;

    private void Start()
    {
        // 确保文本在游戏开始时隐藏
        if (pressEText != null)
        {
            pressEText.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 当 Player 进入触发区域时显示文本
        if (other.CompareTag("Player"))
        {
            if (pressEText != null)
            {
                pressEText.gameObject.SetActive(true); // 显示文本
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 当 Player 离开触发区域时隐藏文本
        if (other.CompareTag("Player"))
        {
            if (pressEText != null)
            {
                pressEText.gameObject.SetActive(false); // 隐藏文本
            }
        }
    }
}
