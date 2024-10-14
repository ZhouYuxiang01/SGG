using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class SimpleSceneLoader : MonoBehaviour
{
    public void LoadScene()
    {
        SceneManager.LoadScene(1);
    }
}