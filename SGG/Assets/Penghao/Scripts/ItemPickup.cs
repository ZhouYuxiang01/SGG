using UnityEngine;
using UnityEngine.UI; // ����UI�����ռ�

public class ItemPickupAndDisplay : MonoBehaviour
{
    private bool isPickedUp = false; // ��ֹ�ظ�ʰȡ
    public AudioSource audioSource; // ���ӵ��ⲿ�� AudioSource
    public AudioClip pickUpSound; // ���Զ����ʰȡ��Ч
    public string itemName = "Item"; // �Զ�����Ʒ���ƣ����ܲ�����������

    public Image itemImage; // ������Image��������ʾ
    private bool isPlayerNearby = false; // ���ڼ������Ƿ񿿽�

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
        // �������Ƿ���봥������
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
        // �������Ƿ��뿪��������
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

    // ���һ��������ʵ��ʰȡ������������Ұ���ĳ����ʱ
    public void TryPickup()
    {
        if (isPlayerNearby && !isPickedUp)
        {
            isPickedUp = true;

            // ����ʰȡ��Ч
            if (audioSource != null && pickUpSound != null)
            {
                audioSource.PlayOneShot(pickUpSound); // ʹ�� AudioSource ������Ч
            }

            // ��ʾʰȡ��Ϣ
            Debug.Log("Picked up: " + itemName);

            // ����ͼƬ���������壨�����ѡ���������٣�������ʱ���٣�
            itemImage.gameObject.SetActive(false);
            Destroy(gameObject); // ����������Ʒ
        }
    }
}
