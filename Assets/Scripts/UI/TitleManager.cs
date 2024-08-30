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
    private Collider2D touchCollider;
    private Button creditButton;
    private GameObject creditDialog;
    private Button creditCloseButton;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.Find("AudioSource").GetComponent<AudioManager>();
        darkCover = transform.Find("DarkCover").gameObject.GetComponent<Image>();
        darkCover.color = fadeOutColor;
        touchCollider = transform.Find("TouchCollider").gameObject.GetComponent<Collider2D>();
        creditButton = transform.Find("CreditButton").gameObject.GetComponent<Button>();
        creditDialog = transform.Find("CreditDialog").gameObject;
        creditCloseButton = creditDialog.transform.Find("CloseButton").gameObject.GetComponent<Button>();
        creditDialog.SetActive(false);
        creditButton.onClick.AddListener(OnClickCreditButton);
        creditCloseButton.onClick.AddListener(OnClickCreditCloseButton);
        darkCover.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && touchCollider.OverlapPoint(Input.mousePosition))
        {
            audioManager.PlaySE("ButtonPositive");
            StartCoroutine("FadeOut");
        }
    }

    private IEnumerator FadeOut()
    {
        darkCover.gameObject.SetActive(true);
        while (fadeOutColor.a < 250)
        {
            alpha += fadeOutSpeed * Time.deltaTime;
            fadeOutColor.a = (byte)alpha;
            darkCover.color = fadeOutColor;
            yield return null;
        }
        SceneManager.LoadScene("MapScene");
    }

    private void OnClickCreditButton()
    {
        audioManager.PlaySE("ButtonPositive");
        creditDialog.SetActive(true);
        touchCollider.gameObject.SetActive(false);
    }
    private void OnClickCreditCloseButton()
    {
        audioManager.PlaySE("ButtonNegative");
        creditDialog.SetActive(false);
        touchCollider.gameObject.SetActive(true);
    }
}
