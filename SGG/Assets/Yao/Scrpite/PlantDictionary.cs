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

    private void Start()
    {
        UpdateAllPlants();
    }

    private void Update()
    {
        // ¼ì²â¿ì½Ý¼ü
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
        UpdateAllPlants();
    }

    public void UpdateAllPlants()
    {
        foreach (var plant in plants)
        {
            UpdatePlantDisplay(plant.plantName);
        }
    }

    public void UpdatePlantDisplay(string plantName)
    {
        PlantEntry plant = plants.Find(p => p.plantName == plantName);
        if (plant != null)
        {
            bool isUnlocked = PlayerPrefs.GetInt("Plant_" + plantName, 0) == 1;
            plant.plantImage.color = isUnlocked ? unlockedColor : lockedColor;
            plant.plantNameText.text = isUnlocked ? plant.plantName : lockedNameText;
        }
    }

    public void ResetAllPlants()
    {
        foreach (var plant in plants)
        {
            PlayerPrefs.SetInt("Plant_" + plant.plantName, 0);
        }
        PlayerPrefs.Save();
        UpdateAllPlants();
        Debug.Log("All plants have been reset to locked state.");
    }

    public void UnlockAllPlants()
    {
        foreach (var plant in plants)
        {
            PlayerPrefs.SetInt("Plant_" + plant.plantName, 1);
        }
        PlayerPrefs.Save();
        UpdateAllPlants();
        Debug.Log("All plants have been unlocked.");
    }
}