using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantDictionary : MonoBehaviour
{
 [System.Serializable]
    public class PlantEntry
    {
        public string plantName;
        public Image plantImage;
        public Text plantNameText;
        public Sprite unlockedSprite;
        public Sprite lockedSprite;
    }

    public List<PlantEntry> plants = new List<PlantEntry>();
    private const float UNLOCK_THRESHOLD = 0.6f; // 60% 正确率阈值

    void Start()
    {
        LoadDictionaryState();
        UpdateDictionaryDisplay();
    }

    public void UnlockPlant(string plantName, float correctRate)
    {
        if (correctRate >= UNLOCK_THRESHOLD)
        {
            PlayerPrefs.SetInt("Plant_" + plantName, 1);
            PlayerPrefs.Save();
            UpdateDictionaryDisplay();
        }
    }

    private void LoadDictionaryState()
    {
        foreach (var plant in plants)
        {
            int isUnlocked = PlayerPrefs.GetInt("Plant_" + plant.plantName, 0);
            plant.plantImage.sprite = isUnlocked == 1 ? plant.unlockedSprite : plant.lockedSprite;
            plant.plantNameText.text = isUnlocked == 1 ? plant.plantName : "???";
        }
    }

    private void UpdateDictionaryDisplay()
    {
        foreach (var plant in plants)
        {
            int isUnlocked = PlayerPrefs.GetInt("Plant_" + plant.plantName, 0);
            plant.plantImage.sprite = isUnlocked == 1 ? plant.unlockedSprite : plant.lockedSprite;
            plant.plantNameText.text = isUnlocked == 1 ? plant.plantName : "???";
        }
    }

    // 可以添加一个方法来重置所有植物的解锁状态（用于测试）
    public void ResetAllPlants()
    {
        foreach (var plant in plants)
        {
            PlayerPrefs.DeleteKey("Plant_" + plant.plantName);
        }
        PlayerPrefs.Save();
        UpdateDictionaryDisplay();
    }
}
