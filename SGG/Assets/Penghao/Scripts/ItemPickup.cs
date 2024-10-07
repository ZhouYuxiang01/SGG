using UnityEngine;
using UnityEngine.UI;

public class ItemInteractionAndCameraSwitch : MonoBehaviour
{
    public string itemName = "Item"; // �Զ�����Ʒ���ƣ����ܲ�����������
    public Image itemImage; // ������Image��������ʾ
    public Camera playerCamera; // ��ҵĵ�һ�˳����
    public Camera displayCamera; // չ̨���
    public PlantDisplaySystem plantDisplaySystem; // ֲ��չʾϵͳ������

    private bool isPlayerNearby = false;
    private bool isInDisplayMode = false;

    private void Start()
    {
        // ȷ��ͼƬ��һ��ʼ�����ص�
        if (itemImage != null)
        {
            itemImage.gameObject.SetActive(false);
        }

        // ȷ���������Ǽ���ģ�չ̨����ǽ��õ�
        if (playerCamera != null) playerCamera.gameObject.SetActive(true);
        if (displayCamera != null) displayCamera.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            SwitchToDisplayMode();
        }
        else if (isInDisplayMode && Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchToPlayerMode();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isPlayerNearby = true;
            if (itemImage != null)
            {
                itemImage.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isPlayerNearby = false;
            if (itemImage != null)
            {
                itemImage.gameObject.SetActive(false);
            }
        }
    }

    private void SwitchToDisplayMode()
    {
        isInDisplayMode = true;
        if (playerCamera != null) playerCamera.gameObject.SetActive(false);
        if (displayCamera != null) displayCamera.gameObject.SetActive(true);

        // ʹ��ֲ��չʾϵͳ��ʾ��ǰֲ��
        if (plantDisplaySystem != null)
        {
            plantDisplaySystem.DisplayPlant(itemName);
        }

        // ����������루��������Ҫ������������ϵͳ�����ⲿ�֣�
        // ���磺playerInputSystem.enabled = false;
    }

    private void SwitchToPlayerMode()
    {
        isInDisplayMode = false;
        if (playerCamera != null) playerCamera.gameObject.SetActive(true);
        if (displayCamera != null) displayCamera.gameObject.SetActive(false);

        // ���������������
        // ���磺playerInputSystem.enabled = true;
    }
}