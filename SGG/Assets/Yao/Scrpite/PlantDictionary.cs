using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlantDictionary : MonoBehaviour
{
    [System.Serializable]
    public class PlantEntry
    {
        public string plantName;
        public Image plantImage;
        public TextMeshProUGUI plantNameText;
    }

    public List<PlantEntry> plants = new List<PlantEntry>();
    public Color lockedColor = Color.black;
    public Color unlockedColor = Color.white;
    public string lockedNameText = "???";
    public GameObject victoryScreen;

    private Dictionary<string, bool> unlockedPlants = new Dictionary<string, bool>();

    private void Start()
    {
        // 确保胜利页面和鼠标一开始是隐藏的
        if (victoryScreen != null)
        {
            victoryScreen.SetActive(false);
            HideCursor();
        }

        InitializePlants();
        UpdateAllPlants();
    }

    // 显示鼠标
    private void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // 隐藏鼠标
    private void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void InitializePlants()
    {
        unlockedPlants.Clear();
        foreach (var plant in plants)
        {
            unlockedPlants[plant.plantName] = false;
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                ResetAllPlants();
            }
            else if (Input.GetKeyDown(KeyCode.O))
            {
                UnlockAllPlants();
            }
        }
    }

    public void UpdateAllPlants()
    {
        foreach (var plant in plants)
        {
            UpdatePlantDisplay(plant.plantName);
        }
        CheckVictoryCondition();
    }

    public void UpdatePlantDisplay(string plantName)
    {
        PlantEntry plant = plants.Find(p => p.plantName == plantName);
        if (plant != null)
        {
            bool isUnlocked = unlockedPlants.ContainsKey(plantName) && unlockedPlants[plantName];
            plant.plantImage.color = isUnlocked ? unlockedColor : lockedColor;
            plant.plantNameText.text = isUnlocked ? plant.plantName : lockedNameText;
        }
    }

    public void ResetAllPlants()
    {
        foreach (var plant in plants)
        {
            unlockedPlants[plant.plantName] = false;
        }
        UpdateAllPlants();

        // 重置时隐藏胜利页面和鼠标
        if (victoryScreen != null)
        {
            victoryScreen.SetActive(false);
            HideCursor();
        }
        Debug.Log("All plants have been reset to locked state.");
    }

    public void UnlockAllPlants()
    {
        foreach (var plant in plants)
        {
            unlockedPlants[plant.plantName] = true;
        }
        UpdateAllPlants();
        Debug.Log("All plants have been unlocked.");
    }

    public void UnlockPlant(string plantName)
    {
        if (unlockedPlants.ContainsKey(plantName))
        {
            unlockedPlants[plantName] = true;
            UpdatePlantDisplay(plantName);
            CheckVictoryCondition();
        }
    }

    public bool IsPlantUnlocked(string plantName)
    {
        return unlockedPlants.ContainsKey(plantName) && unlockedPlants[plantName];
    }

    private void CheckVictoryCondition()
    {
        if (victoryScreen == null)
        {
            Debug.LogWarning("Victory Screen is not assigned!");
            return;
        }

        bool allUnlocked = true;
        foreach (var plant in plants)
        {
            if (!unlockedPlants[plant.plantName])
            {
                allUnlocked = false;
                break;
            }
        }

        // 根据解锁状态显示或隐藏胜利页面和鼠标
        victoryScreen.SetActive(allUnlocked);
        if (allUnlocked)
        {
            ShowCursor();
            Debug.Log("Congratulations! All plants have been discovered!");
        }
        else
        {
            HideCursor();
        }
    }
}