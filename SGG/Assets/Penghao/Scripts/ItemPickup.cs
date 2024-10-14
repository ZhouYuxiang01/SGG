using UnityEngine;
using UnityEngine.UI;

public class ItemInteractionAndCameraSwitch : MonoBehaviour
{
    public string itemName = "Item"; // 自定义物品名称（如萝卜、西兰花）
    public Image itemImage; // 公开的Image，用于显示
    public Camera playerCamera; // 玩家的第一人称相机
    public Camera displayCamera; // 展台相机
    public PlantDisplaySystem plantDisplaySystem; // 植物展示系统的引用

    private bool isPlayerNearby = false;
    private bool isInDisplayMode = false;

    private void Start()
    {
        // 确保图片在一开始是隐藏的
        if (itemImage != null)
        {
            itemImage.gameObject.SetActive(false);
        }

        // 确保玩家相机是激活的，展台相机是禁用的
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

        // 使用植物展示系统显示当前植物
        if (plantDisplaySystem != null)
        {
            plantDisplaySystem.DisplayPlant(itemName);
        }

        // 禁用玩家输入（您可能需要根据您的输入系统调整这部分）
        // 例如：playerInputSystem.enabled = false;
    }

    private void SwitchToPlayerMode()
    {
        isInDisplayMode = false;
        if (playerCamera != null) playerCamera.gameObject.SetActive(true);
        if (displayCamera != null) displayCamera.gameObject.SetActive(false);

        // 重新启用玩家输入
        // 例如：playerInputSystem.enabled = true;
    }
}