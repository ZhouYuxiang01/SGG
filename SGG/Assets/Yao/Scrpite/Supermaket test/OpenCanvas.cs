using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    // ��Ҫ����/�رյĶ���
    public GameObject targetObject;

    private void Update()
    {
        // ����Ƿ���K��
        if (Input.GetKeyDown(KeyCode.K))
        {
            // �л�����ļ���״̬
            targetObject.SetActive(!targetObject.activeSelf);
        }
    }
}
