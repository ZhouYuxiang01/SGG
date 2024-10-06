using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ObjectVisibilityManager : MonoBehaviour
{
    public Button playButton;

    [Header("Objects to Disable")]
    public List<GameObject> objectsToDisable;

    [Header("Objects to Enable")]
    public List<GameObject> objectsToEnable;

    [Header("Fade Settings")]
    public float fadeDuration = 1f;

    void Start()
    {
        if (playButton == null)
        {
            Debug.LogError("PlayButton is not assigned!");
            return;
        }

        playButton.onClick.AddListener(ManageObjectVisibility);
    }

    void ManageObjectVisibility()
    {
        // Fade out objects
        foreach (GameObject obj in objectsToDisable)
        {
            if (obj != null)
            {
                StartCoroutine(FadeObject(obj, false));
            }
            else
            {
                Debug.LogWarning("An object in the disable list is null!");
            }
        }

        // Fade in objects
        foreach (GameObject obj in objectsToEnable)
        {
            if (obj != null)
            {
                obj.SetActive(true); // Ensure the object is active before fading in
                StartCoroutine(FadeObject(obj, true));
            }
            else
            {
                Debug.LogWarning("An object in the enable list is null!");
            }
        }
    }

    IEnumerator FadeObject(GameObject obj, bool fadeIn)
    {
        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();

        if (canvasGroup != null)
        {
            // Fade UI elements using CanvasGroup
            float startAlpha = fadeIn ? 0f : 1f;
            float endAlpha = fadeIn ? 1f : 0f;

            canvasGroup.alpha = startAlpha;

            float elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
                yield return null;
            }

            canvasGroup.alpha = endAlpha;
        }
        else
        {
            // Fade non-UI elements by scaling
            Vector3 startScale = fadeIn ? Vector3.zero : obj.transform.localScale;
            Vector3 endScale = fadeIn ? obj.transform.localScale : Vector3.zero;

            float elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                obj.transform.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / fadeDuration);
                yield return null;
            }

            obj.transform.localScale = endScale;
        }

        if (!fadeIn)
        {
            obj.SetActive(false);
        }
    }
}