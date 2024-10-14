using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    // 需要激活/关闭的对象
    public GameObject targetObject;

    private void Update()
    {
        // 检测是否按下K键
        if (Input.GetKeyDown(KeyCode.K))
        {
            // 切换对象的激活状态
            targetObject.SetActive(!targetObject.activeSelf);
        }
    }
}
