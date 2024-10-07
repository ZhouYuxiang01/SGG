using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string mainSceneName;  // ������������

    private void Start()
    {
        // ȷ������������������
        if (string.IsNullOrEmpty(mainSceneName))
        {
            Debug.LogError("Main scene name is not set in LoadScene script!");
        }
    }

    public void ReturnToMainScene()
    {
        Debug.Log("Attempting to return to main scene: " + mainSceneName);

        // ��ȡ��ǰ���ص����г���
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

        // ������������������
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