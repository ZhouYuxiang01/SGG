using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomModelSpawner : MonoBehaviour
{
    public GameObject[] models; // 你要放置的5个模型
    public GameObject[] cubes;  // 你放置的cube参照物

    private void Start()
    {
        // 检查模型和cube的数量是否符合要求
        if (models.Length > cubes.Length)
        {
            Debug.LogError("模型数量不能超过参照物的数量！");
            return;
        }

        // 创建一个cube的列表并随机分配模型
        List<int> availableCubes = new List<int>();
        for (int i = 0; i < cubes.Length; i++)
        {
            availableCubes.Add(i);  // 把所有cube的索引加入列表
        }

        // 随机分配每个模型到cube的位置
        foreach (GameObject model in models)
        {
            // 随机从availableCubes列表中选择一个位置
            int randomIndex = Random.Range(0, availableCubes.Count);
            int cubeIndex = availableCubes[randomIndex];

            // 设置模型的位置与cube一致
            model.transform.position = cubes[cubeIndex].transform.position;

            // 从availableCubes中移除已使用的cube
            availableCubes.RemoveAt(randomIndex);
        }
    }
}

