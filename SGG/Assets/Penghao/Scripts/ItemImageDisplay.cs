using UnityEngine;
using UnityEngine.UI; // 引入UI命名空间

public class ItemImageDisplay : MonoBehaviour
{
    public Image itemImage; // 公开的Image, 方便在Inspector中拖入
    private bool isPlayerNearby = false; // 检测玩家是否靠近

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
        // 当Player进入触发区域时
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
        // 当Player离开触发区域时
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
}
