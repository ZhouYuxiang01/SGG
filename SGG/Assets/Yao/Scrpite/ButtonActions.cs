using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonActions : MonoBehaviour
{
    public Button loadSceneButton;
    public Button quitGameButton;
    public string sceneName;

    void Start()
    {
        loadSceneButton.onClick.AddListener(() => LoadScene(sceneName));
        quitGameButton.onClick.AddListener(QuitGame);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
