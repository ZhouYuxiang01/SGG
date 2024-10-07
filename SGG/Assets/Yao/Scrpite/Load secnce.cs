using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string mainSceneName;  // 主场景的名称

    private void Start()
    {
        // 确保主场景名称已设置
        if (string.IsNullOrEmpty(mainSceneName))
        {
            Debug.LogError("Main scene name is not set in LoadScene script!");
        }
    }

    public void ReturnToMainScene()
    {
        Debug.Log("Attempting to return to main scene: " + mainSceneName);

        // 获取当前加载的所有场景
        int sceneCount = SceneManager.sceneCount;
        for (int i = 0; i < sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            Debug.Log("Loaded scene: " + scene.name + ", Build Index: " + scene.buildIndex);

            if (scene.name != mainSceneName)
            {
                Debug.Log("Unloading scene: " + scene.name);
                SceneManager.UnloadSceneAsync(scene);
            }
        }

        // 查找主场景并激活它
        Scene mainScene = SceneManager.GetSceneByName(mainSceneName);
        if (mainScene.IsValid())
        {
            Debug.Log("Activating main scene: " + mainSceneName);
            SceneManager.SetActiveScene(mainScene);
        }
        else
        {
            Debug.LogError("Main scene not found: " + mainSceneName);
        }
    }
}