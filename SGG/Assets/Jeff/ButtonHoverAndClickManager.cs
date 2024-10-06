using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonHoverAndClickFunctions : MonoBehaviour
{
    [Header("Object to Enlarge")]
    public GameObject objectToEnlarge;

    [Header("Hover Effect")]
    public float enlargeScale = 1.2f;
    public float hoverDuration = 0.2f;

    [Header("Objects to Show")]
    public List<GameObject> objectsToShow;

    [Header("Objects to Hide")]
    public List<GameObject> objectsToHide;

    private Vector3 originalScale;

    void Start()
    {
        if (objectToEnlarge != null)
        {
            originalScale = objectToEnlarge.transform.localScale;
        }
    }

    public void OnButtonHover()
    {
        StartCoroutine(ScaleObject(enlargeScale));
    }

    public void OnButtonExit()
    {
        StartCoroutine(ScaleObject(1f));
    }

    public void OnButtonClick()
    {
        ShowAndHideObjects();
    }

    IEnumerator ScaleObject(float targetScale)
    {
        if (objectToEnlarge == null) yield break;

        Vector3 startScale = objectToEnlarge.transform.localScale;
        Vector3 endScale = originalScale * targetScale;
        float elapsedTime = 0f;

        while (elapsedTime < hoverDuration)
        {
            objectToEnlarge.transform.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / hoverDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        objectToEnlarge.transform.localScale = endScale;
    }

    void ShowAndHideObjects()
    {
        foreach (GameObject obj in objectsToShow)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
            else
            {
                Debug.LogWarning("An object in the show list is null!");
            }
        }

        foreach (GameObject obj in objectsToHide)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
            else
            {
                Debug.LogWarning("An object in the hide list is null!");
            }
        }
    }
}