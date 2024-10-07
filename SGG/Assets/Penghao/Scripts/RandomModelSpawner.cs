using UnityEngine;
using System.Collections.Generic;

public class RandomModelSpawner : MonoBehaviour
{
    public GameObject[] models; // ��Ҫ���õ�5��ģ��
    public GameObject[] cubes;  // ����õ�cube������

    private void Start()
    {
        //// ���ģ�ͺ�cube�������Ƿ����Ҫ��
        //if (models.Length > cubes.Length)
        //{
        //    Debug.LogError("ģ���������ܳ����������������");
        //    return;
        //}

        // ����һ��cube���б��������ģ��

        List<int> availableCubes = new List<int>();
        for (int i = 0; i < cubes.Length; i++)
        {
            availableCubes.Add(i);  // ������cube�����������б�
        }

        // �������ÿ��ģ�͵�cube��λ��
        foreach (GameObject model in models)
        {
            int randomIndex = Random.Range(0, availableCubes.Count);
            int cubeIndex = availableCubes[randomIndex];

            // ����ģ�͵�λ����cubeһ��
            model.transform.position = cubes[cubeIndex].transform.position;

            // ���ģ�ͻ�û�� ItemPickup ������������
            if (model.GetComponent<ItemInteractionAndCameraSwitch>() == null)
            {
                model.AddComponent<ItemInteractionAndCameraSwitch>();

                // ���ģ���Ƿ��� Collider �� Rigidbody��ȷ����ײ�������
                if (model.GetComponent<Collider>() == null)
                {
                    BoxCollider boxCollider = model.AddComponent<BoxCollider>();
                    boxCollider.isTrigger = true; // ȷ���Ǵ�����
                }

                if (model.GetComponent<Rigidbody>() == null)
                {
                    Rigidbody rb = model.AddComponent<Rigidbody>();
                    rb.isKinematic = true; // ����Ϊ��̬��ֹ�������
                }
            }

            // ��availableCubes���Ƴ���ʹ�õ�cube
            availableCubes.RemoveAt(randomIndex);
        }
    }
}
