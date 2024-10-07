using UnityEngine;
using System.Collections.Generic;

public class RandomModelSpawner : MonoBehaviour
{
    public GameObject[] models; // 你要放置的5个模型
    public GameObject[] cubes;  // 你放置的cube参照物

    private void Start()
    {
        //// 检查模型和cube的数量是否符合要求
        //if (models.Length > cubes.Length)
        //{
        //    Debug.LogError("模型数量不能超过参照物的数量！");
        //    return;
        //}

        // 创建一个cube的列表并随机分配模型

        List<int> availableCubes = new List<int>();
        for (int i = 0; i < cubes.Length; i++)
        {
            availableCubes.Add(i);  // 把所有cube的索引加入列表
        }

        // 随机分配每个模型到cube的位置
        foreach (GameObject model in models)
        {
            int randomIndex = Random.Range(0, availableCubes.Count);
            int cubeIndex = availableCubes[randomIndex];

            // 设置模型的位置与cube一致
            model.transform.position = cubes[cubeIndex].transform.position;

            // 如果模型还没有 ItemPickup 组件，就添加它
            if (model.GetComponent<ItemInteractionAndCameraSwitch>() == null)
            {
                model.AddComponent<ItemInteractionAndCameraSwitch>();

                // 检查模型是否有 Collider 和 Rigidbody，确保碰撞检测正常
                if (model.GetComponent<Collider>() == null)
                {
                    BoxCollider boxCollider = model.AddComponent<BoxCollider>();
                    boxCollider.isTrigger = true; // 确保是触发器
                }

                if (model.GetComponent<Rigidbody>() == null)
                {
                    Rigidbody rb = model.AddComponent<Rigidbody>();
                    rb.isKinematic = true; // 设置为静态防止物理干扰
                }
            }

            // 从availableCubes中移除已使用的cube
            availableCubes.RemoveAt(randomIndex);
        }
    }
}
