using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loadsecnce : MonoBehaviour
{
    // Start is called before the first frame update
    public string targetSceneName;

    // �˷���Ӧ���ӵ���� UI Button �� OnClick �¼�
    public void SwitchLevel()
    {
        // �л���Ŀ�곡��
        SceneManager.LoadScene(targetSceneName);
    }
}
