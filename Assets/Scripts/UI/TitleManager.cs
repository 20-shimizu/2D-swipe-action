using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    private AudioManager audioManager;
    private Image darkCover;
    private float fadeOutSpeed = 255.0f;
    private Color32 fadeOutColor = new Color32(0, 0, 0, 0);
    private float alpha = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.Find("AudioSource").GetComponent<AudioManager>();
        darkCover = transform.Find("DarkCover").gameObject.GetComponent<Image>();
        darkCover.color = fadeOutColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            audioManager.PlaySE("ButtonPositive");
            StartCoroutine("FadeOut");
        }
    }

    private IEnumerator FadeOut()
    {
        while (fadeOutColor.a < 250)
        {
            alpha += fadeOutSpeed * Time.deltaTime;
            fadeOutColor.a = (byte)alpha;
            darkCover.color = fadeOutColor;
            yield return null;
        }
        SceneManager.LoadScene("MapScene");
    }
}
