using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomModelSpawner : MonoBehaviour
{
    public GameObject[] models; // ��Ҫ���õ�5��ģ��
    public GameObject[] cubes;  // ����õ�cube������

    private void Start()
    {
        // ���ģ�ͺ�cube�������Ƿ����Ҫ��
        if (models.Length > cubes.Length)
        {
            Debug.LogError("ģ���������ܳ����������������");
            return;
        }

        // ����һ��cube���б��������ģ��
        List<int> availableCubes = new List<int>();
        for (int i = 0; i < cubes.Length; i++)
        {
            availableCubes.Add(i);  // ������cube�����������б�
        }

        // �������ÿ��ģ�͵�cube��λ��
        foreach (GameObject model in models)
        {
            // �����availableCubes�б���ѡ��һ��λ��
            int randomIndex = Random.Range(0, availableCubes.Count);
            int cubeIndex = availableCubes[randomIndex];

            // ����ģ�͵�λ����cubeһ��
            model.transform.position = cubes[cubeIndex].transform.position;

            // ��availableCubes���Ƴ���ʹ�õ�cube
            availableCubes.RemoveAt(randomIndex);
        }
    }
}

