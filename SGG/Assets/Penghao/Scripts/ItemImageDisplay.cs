using UnityEngine;
using UnityEngine.UI; // ����UI�����ռ�

public class ItemImageDisplay : MonoBehaviour
{
    public Image itemImage; // ������Image, ������Inspector������
    private bool isPlayerNearby = false; // �������Ƿ񿿽�

    private void Start()
    {
        // ȷ��ͼƬ��һ��ʼ�����ص�
        if (itemImage != null)
        {
            itemImage.gameObject.SetActive(false); // ����ͼƬ
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // ��Player���봥������ʱ
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;

            // ��ʾͼƬ
            if (itemImage != null)
            {
                itemImage.gameObject.SetActive(true); // ��ʾͼƬ
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // ��Player�뿪��������ʱ
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;

            // ����ͼƬ
            if (itemImage != null)
            {
                itemImage.gameObject.SetActive(false); // ����ͼƬ
            }
        }
    }
}
