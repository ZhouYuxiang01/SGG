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

    private void Start()
    {
        if (victoryScreen != null)
        {
            victoryScreen.SetActive(false);
            HideCursor();
        }

        // 如果是新游戏，重置所有解锁状态
        if (PlayerPrefs.GetInt("IsNewGame", 1) == 1)
        {
            ResetAllPlantsForNewGame();
            PlayerPrefs.SetInt("IsNewGame", 0); // 标记不再是新游戏
            PlayerPrefs.Save();
        }

        UpdateAllPlants();
    }

    public void ResetAllPlantsForNewGame()
    {
        Debug.Log("[PlantDictionary] Resetting all plants for new game");
        foreach (var plant in plants)
        {
            PlayerPrefs.SetInt("Plant_" + plant.plantName, 0);
        }
        PlayerPrefs.Save();

        if (victoryScreen != null)
        {
            victoryScreen.SetActive(false);
            HideCursor();
        }

        UpdateAllPlants();
    }

    public void StartNewGame()
    {
        PlayerPrefs.SetInt("IsNewGame", 1);
        PlayerPrefs.Save();
        ResetAllPlantsForNewGame();
    }

    private void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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

    public void UnlockPlant(string plantName)
    {
        Debug.Log($"[PlantDictionary] Attempting to unlock plant: {plantName}");
        PlantEntry plant = plants.Find(p => p.plantName == plantName);
        if (plant != null)
        {
            Debug.Log($"[PlantDictionary] Found plant {plantName}, updating display");
            UpdatePlantDisplay(plantName);
            CheckVictoryCondition();
        }
        else
        {
            Debug.LogError($"[PlantDictionary] Plant not found: {plantName}");
            foreach (var p in plants)
            {
                Debug.Log($"Available plant: {p.plantName}");
            }
        }
    }

    public void UpdatePlantDisplay(string plantName)
    {
        PlantEntry plant = plants.Find(p => p.plantName == plantName);
        if (plant != null && plant.plantImage != null && plant.plantNameText != null)
        {
            bool isUnlocked = PlayerPrefs.GetInt("Plant_" + plantName, 0) == 1;
            plant.plantImage.color = isUnlocked ? unlockedColor : lockedColor;
            plant.plantNameText.text = isUnlocked ? plant.plantName : lockedNameText;
            Debug.Log($"[PlantDictionary] Updated {plantName} - Unlock status: {isUnlocked}");
        }
    }

    public void ResetAllPlants()
    {
        foreach (var plant in plants)
        {
            PlayerPrefs.SetInt("Plant_" + plant.plantName, 0);
        }
        PlayerPrefs.Save();

        if (victoryScreen != null)
        {
            victoryScreen.SetActive(false);
            HideCursor();
        }

        UpdateAllPlants();
        Debug.Log("[PlantDictionary] All plants have been reset to locked state.");
    }

    public void UnlockAllPlants()
    {
        foreach (var plant in plants)
        {
            PlayerPrefs.SetInt("Plant_" + plant.plantName, 1);
        }
        PlayerPrefs.Save();
        UpdateAllPlants();
        Debug.Log("[PlantDictionary] All plants have been unlocked.");
    }

    private void CheckVictoryCondition()
    {
        if (victoryScreen == null)
        {
            Debug.LogWarning("[PlantDictionary] Victory Screen is not assigned!");
            return;
        }

        bool allUnlocked = true;
        foreach (var plant in plants)
        {
            bool isUnlocked = PlayerPrefs.GetInt("Plant_" + plant.plantName, 0) == 1;
            if (!isUnlocked)
            {
                allUnlocked = false;
                break;
            }
        }

        victoryScreen.SetActive(allUnlocked);
        if (allUnlocked)
        {
            ShowCursor();
            Debug.Log("[PlantDictionary] Congratulations! All plants have been discovered!");
        }
        else
        {
            HideCursor();
        }
    }
}