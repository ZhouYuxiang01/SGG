using UnityEngine;
using UnityEngine.UI; // 引入UI命名空间

public class ItemPickupAndDisplay : MonoBehaviour
{
    private bool isPickedUp = false; // 防止重复拾取
    public AudioSource audioSource; // 连接到外部的 AudioSource
    public AudioClip pickUpSound; // 可自定义的拾取音效
    public string itemName = "Item"; // 自定义物品名称（如萝卜、西兰花）

    public Image itemImage; // 公开的Image，用于显示
    private bool isPlayerNearby = false; // 用于检测玩家是否靠近

    private void Start()
    {
        // 确保图片在一开始是隐藏的
        if (itemImage != null)
        {
            itemImage.gameObject.SetActive(false); // 隐藏图片
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 检查玩家是否进入触发区域
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;

            // 显示图片
            if (itemImage != null)
            {
                itemImage.gameObject.SetActive(true); // 显示图片
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 检查玩家是否离开触发区域
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;

            // 隐藏图片
            if (itemImage != null)
            {
                itemImage.gameObject.SetActive(false); // 隐藏图片
            }
        }
    }

    // 添加一个方法来实现拾取，可能是在玩家按下某个键时
    public void TryPickup()
    {
        if (isPlayerNearby && !isPickedUp)
        {
            isPickedUp = true;

            // 播放拾取音效
            if (audioSource != null && pickUpSound != null)
            {
                audioSource.PlayOneShot(pickUpSound); // 使用 AudioSource 播放音效
            }

            // 显示拾取信息
            Debug.Log("Picked up: " + itemName);

            // 隐藏图片，销毁物体（你可以选择立即销毁，或者延时销毁）
            itemImage.gameObject.SetActive(false);
            Destroy(gameObject); // 这里销毁物品
        }
    }
}
