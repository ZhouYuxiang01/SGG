using UnityEngine;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[Serializable]
public class PlantInfo
{
    public string plantName;
    public List<GameObject> growthStages;
}

public class PlantDisplaySystem : MonoBehaviour
{
    [SerializeField] private List<PlantInfo> plants = new List<PlantInfo>();
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private TextMeshProUGUI plantNameText;
    [SerializeField] private TextMeshProUGUI scientificNameText;
    [SerializeField] private TextMeshProUGUI ediblePartsText;
    [SerializeField] private TextMeshProUGUI originText;
    [SerializeField] private TextMeshProUGUI plantingSeasonText;
    [SerializeField] private TextMeshProUGUI commonDishesText;
    [SerializeField] private float autoRotationSpeed = 30f;
    [SerializeField] private float growthStageInterval = 5f;
    [SerializeField] private GameObject displayPosition;
    [SerializeField] private float dragRotationSpeed = 5f;
    [SerializeField] private float zoomSpeed = 0.1f;
    [SerializeField] private float minZoom = 0.5f;
    [SerializeField] private float maxZoom = 2f;
    [SerializeField] private float autoRotationResumeDelay = 3f;
    [SerializeField] private Camera displayCamera;
    [SerializeField] private Camera firstPersonCamera;

    private Dictionary<string, Dictionary<string, string>> plantData = new Dictionary<string, Dictionary<string, string>>();
    private GameObject currentPlant;
    private int currentGrowthStage = 0;
    private Coroutine growthCoroutine;
    private bool isDragging = false;
    private Vector3 previousMousePosition;
    private float currentZoom = 1f;
    private float lastInteractionTime;
    private bool isAutoRotating = true;
    private string currentPlantName;
    //private string dataFilePath = Path.Combine(Application.streamingAssetsPath, "PlantData.txt");
    //private string dataFilePath = "Assets/PlantData.txt";
    private string dataFilePath = "/PlantGame/PlantData.txt";

    private void Start()
    {
        LoadPlantData();
        HideUI();
    }

    private void Update()
    {
        if (currentPlant == null) return;

        HandleMouseInteraction();
        HandleZoom();
        HandleAutoRotation();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToFirstPersonView();
        }
    }

    private void LoadPlantData()
    {
        if (!File.Exists(dataFilePath))
        {
            Debug.LogError("Plant data file not found: " + dataFilePath);
            return;
        }

        string[] lines = File.ReadAllLines(dataFilePath);
        string currentPlant = "";
        Dictionary<string, string> currentPlantData = new Dictionary<string, string>();

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            if (line.StartsWith("Plant:"))
            {
                if (!string.IsNullOrEmpty(currentPlant))
                {
                    plantData[currentPlant] = new Dictionary<string, string>(currentPlantData);
                    currentPlantData.Clear();
                }
                currentPlant = line.Substring(6).Trim();
            }
            else
            {
                string[] parts = line.Split(':');
                if (parts.Length == 2)
                {
                    currentPlantData[parts[0].Trim()] = parts[1].Trim();
                }
            }
        }

        if (!string.IsNullOrEmpty(currentPlant))
        {
            plantData[currentPlant] = new Dictionary<string, string>(currentPlantData);
        }

        Debug.Log($"Loaded data for {plantData.Count} plants.");
    }

    public void DisplayPlant(string plantName)
    {
        if (!plantData.ContainsKey(plantName))
        {
            Debug.LogError($"Plant data not found for: {plantName}");
            return;
        }

        currentPlantName = plantName;
        PlantInfo plant = plants.Find(p => p.plantName == plantName);
        if (plant == null || plant.growthStages.Count == 0)
        {
            Debug.LogError($"Plant model not found or no growth stages for: {plantName}");
            return;
        }

        if (currentPlant != null)
        {
            Destroy(currentPlant);
        }

        if (growthCoroutine != null)
        {
            StopCoroutine(growthCoroutine);
        }

        currentGrowthStage = 0;
        Vector3 spawnPosition = displayPosition != null ? displayPosition.transform.position : Vector3.zero;
        currentPlant = Instantiate(plant.growthStages[currentGrowthStage], spawnPosition, Quaternion.identity);
        ShowUI();
        UpdateUI(plantName);

        growthCoroutine = StartCoroutine(CycleGrowthStages(plant));

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (displayCamera != null) displayCamera.gameObject.SetActive(true);
        if (firstPersonCamera != null) firstPersonCamera.gameObject.SetActive(false);
    }

    private IEnumerator CycleGrowthStages(PlantInfo plant)
    {
        while (true)
        {
            yield return new WaitForSeconds(growthStageInterval);

            currentGrowthStage = (currentGrowthStage + 1) % plant.growthStages.Count;
            Vector3 spawnPosition = displayPosition != null ? displayPosition.transform.position : Vector3.zero;
            Quaternion currentRotation = currentPlant.transform.rotation;
            Vector3 currentScale = currentPlant.transform.localScale;
            Destroy(currentPlant);
            currentPlant = Instantiate(plant.growthStages[currentGrowthStage], spawnPosition, currentRotation);
            currentPlant.transform.localScale = currentScale;
        }
    }

    private void HandleMouseInteraction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            previousMousePosition = Input.mousePosition;
            isAutoRotating = false;
            lastInteractionTime = Time.time;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 delta = Input.mousePosition - previousMousePosition;
            currentPlant.transform.Rotate(Vector3.up, -delta.x * dragRotationSpeed * Time.deltaTime, Space.World);
            currentPlant.transform.Rotate(Vector3.right, delta.y * dragRotationSpeed * Time.deltaTime, Space.World);
            previousMousePosition = Input.mousePosition;
            lastInteractionTime = Time.time;
        }
    }

    private void HandleZoom()
    {
        float scrollDelta = Input.mouseScrollDelta.y;
        if (scrollDelta != 0)
        {
            currentZoom = Mathf.Clamp(currentZoom - scrollDelta * zoomSpeed, minZoom, maxZoom);
            Vector3 zoomCenter = displayCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, displayCamera.nearClipPlane));
            currentPlant.transform.localScale = Vector3.one * currentZoom;

            Vector3 newZoomCenter = displayCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, displayCamera.nearClipPlane));
            currentPlant.transform.position += zoomCenter - newZoomCenter;

            lastInteractionTime = Time.time;
        }
    }

    private void HandleAutoRotation()
    {
        if (Time.time - lastInteractionTime > autoRotationResumeDelay)
        {
            isAutoRotating = true;
        }

        if (isAutoRotating)
        {
            currentPlant.transform.Rotate(Vector3.up, autoRotationSpeed * Time.deltaTime);
        }
    }

    private void ReturnToFirstPersonView()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (firstPersonCamera != null) firstPersonCamera.gameObject.SetActive(true);
        if (displayCamera != null) displayCamera.gameObject.SetActive(false);

        HideUI();

        if (growthCoroutine != null)
        {
            StopCoroutine(growthCoroutine);
        }
        if (currentPlant != null)
        {
            Destroy(currentPlant);
        }

        currentPlantName = null;
    }

    private void ShowUI()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("UI Panel is not assigned in the inspector.");
        }
    }

    private void HideUI()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(false);
        }
    }

    private void UpdateUI(string plantName)
    {
        if (!plantData.ContainsKey(plantName))
        {
            Debug.LogError($"Plant data not found for: {plantName}");
            return;
        }

        var data = plantData[plantName];

        SafeSetText(plantNameText, plantName);
        SafeSetText(scientificNameText, GetDataValue(data, "Scientific Name"));
        SafeSetText(ediblePartsText, GetDataValue(data, "Edible Parts"));
        SafeSetText(originText, GetDataValue(data, "Origin"));
        SafeSetText(plantingSeasonText, GetDataValue(data, "Planting Season"));
        SafeSetText(commonDishesText, GetDataValue(data, "Common Dishes"));
    }

    private void SafeSetText(TextMeshProUGUI textComponent, string value)
    {
        if (textComponent != null)
        {
            textComponent.text = value;
        }
        else
        {
            Debug.LogWarning($"Text component not assigned for: {value}");
        }
    }

    private string GetDataValue(Dictionary<string, string> data, string key)
    {
        if (data.ContainsKey(key))
        {
            return data[key];
        }
        else
        {
            Debug.LogWarning($"Data not found for key: {key}");
            return "N/A";
        }
    }

    public string GetCurrentPlantName()
    {
        return currentPlantName;
    }
}