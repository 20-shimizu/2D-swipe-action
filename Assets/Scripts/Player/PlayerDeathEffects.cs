using UnityEngine;
using System.Collections;

public class PlayerDeathEffects : MonoBehaviour
{
    public float fadeOutDuration = 1.0f;
    public float shakeIntensity = 0.1f;
    public float shakeDuration = 0.5f;

    private SpriteRenderer playerSprite;
    public Vector3 originalPosition;

    void Start()
    {
        playerSprite = GetComponent<SpriteRenderer>();
    }

    public IEnumerator PlayDeathEffect()
    {
        originalPosition = transform.position;
        // Screen shake effect
        StartCoroutine(ShakeEffect());

        // Fade out effect
        float elapsedTime = 0;
        Color originalColor = playerSprite.color;

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
            playerSprite.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // Disable the player object
        gameObject.SetActive(false);
    }

    private IEnumerator ShakeEffect()
    {
        float elapsedTime = 0;

        while (elapsedTime < shakeDuration)
        {
            elapsedTime += Time.deltaTime;
            float x = originalPosition.x + Random.Range(-1f, 1f) * shakeIntensity;
            float y = originalPosition.y + Random.Range(-1f, 1f) * shakeIntensity;
            transform.position = new Vector3(x, y, originalPosition.z);
            yield return null;
        }

        transform.position = originalPosition;
    }
}