using UnityEngine;
using UnityEngine.UI; // ���� UI �����ռ�

public class DisplayTextOnTrigger : MonoBehaviour
{
    public Text pressEText; // ������ Text ����������ʾ "Press E"
    // ���ʹ�� TextMeshPro ����Ը�Ϊ TMP_Text ������ TMPro �����ռ�
    // public TMP_Text pressEText;

    private void Start()
    {
        // ȷ���ı�����Ϸ��ʼʱ����
        if (pressEText != null)
        {
            pressEText.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // �� Player ���봥������ʱ��ʾ�ı�
        if (other.CompareTag("Player"))
        {
            if (pressEText != null)
            {
                pressEText.gameObject.SetActive(true); // ��ʾ�ı�
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // �� Player �뿪��������ʱ�����ı�
        if (other.CompareTag("Player"))
        {
            if (pressEText != null)
            {
                pressEText.gameObject.SetActive(false); // �����ı�
            }
        }
    }
}
