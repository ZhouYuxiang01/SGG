using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loadsecnce : MonoBehaviour
{
    // Start is called before the first frame update
    public string targetSceneName;

    // 此方法应连接到你的 UI Button 的 OnClick 事件
    public void SwitchLevel()
    {
        // 切换到目标场景
        SceneManager.LoadScene(targetSceneName);
    }
}
