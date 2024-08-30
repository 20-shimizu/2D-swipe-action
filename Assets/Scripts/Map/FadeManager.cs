using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance { get; private set; }

    private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private string fadeCanvasName = "FadeCanvas";
    [SerializeField] private string fadeImageName = "FadeImage";

    private bool isFading = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            Debug.Log("FadeManager initialized");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(SetupFadeImageAndFadeIn());
    }

    private IEnumerator SetupFadeImageAndFadeIn()
    {
        yield return null; // Wait for one frame to ensure all objects are initialized
        SetupFadeImage();
        if (fadeImage != null)
        {
            StartCoroutine(FadeCoroutine(1f, 0f));
        }
    }

    private void SetupFadeImage()
    {
        // Find FadeCanvas in the root of the scene
        Canvas fadeCanvas = null;
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in canvases)
        {
            if (canvas.name == fadeCanvasName && canvas.transform.parent == null)
            {
                fadeCanvas = canvas;
                break;
            }
        }

        if (fadeCanvas == null)
        {
            Debug.LogError($"FadeCanvas named '{fadeCanvasName}' not found in the scene root!");
            return;
        }

        // Find FadeImage as a child of FadeCanvas
        fadeImage = fadeCanvas.transform.Find(fadeImageName)?.GetComponent<Image>();

        if (fadeImage == null)
        {
            Debug.LogError($"FadeImage named '{fadeImageName}' not found as a child of FadeCanvas!");
            return;
        }

        // Ensure the fadeImage is set to cover the entire screen and is on top
        fadeCanvas.sortingOrder = 9999; // Ensure it's on top of other UI elements
        RectTransform fadeImageRect = fadeImage.rectTransform;
        fadeImageRect.anchorMin = Vector2.zero;
        fadeImageRect.anchorMax = Vector2.one;
        fadeImageRect.offsetMin = Vector2.zero;
        fadeImageRect.offsetMax = Vector2.zero;
        fadeImage.color = Color.black;
    }

    public void FadeAndLoadScene(string sceneName)
    {
        if (!isFading)
        {
            StartCoroutine(FadeAndLoadSceneCoroutine(sceneName));
        }
    }

    private IEnumerator FadeAndLoadSceneCoroutine(string sceneName)
    {
        isFading = true;

        // Fade out
        yield return StartCoroutine(FadeCoroutine(0f, 1f));

        // Load new scene
        SceneManager.LoadScene(sceneName);

        isFading = false;
    }

    private IEnumerator FadeCoroutine(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;

        if (fadeImage == null)
        {
            yield break;
        }

        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, endAlpha);
    }
}