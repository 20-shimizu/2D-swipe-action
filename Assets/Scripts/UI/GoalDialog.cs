using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoalDialog : MonoBehaviour
{
    private enum GoalDialogState
    {
        WAIT,
        OPEN,
        SHOW_TEXT,
        FINISH,
    }
    private GoalDialogState state = GoalDialogState.WAIT;
    private GameObject dialog;
    private Image itemImage;
    private Text itemName;
    private Text itemDetail;
    private Button confirmButton;

    private Vector2 scale = new Vector2(0.0f, 100.0f);
    private float alpha = 0.0f;
    private Color32 imageColor = new Color32(255, 255, 255, 0);
    private Color32 textColor = new Color32(255, 255, 255, 0);

    private float openDialogSpeed = 100.0f;
    private float alphaSpeed = 100.0f;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        dialog = transform.Find("Canvas/Dialog").gameObject;
        itemImage = dialog.transform.Find("Item").gameObject.GetComponent<Image>();
        itemName = itemImage.transform.Find("ItemNameText").gameObject.GetComponent<Text>();
        itemDetail = itemImage.transform.Find("ItemDetailText").gameObject.GetComponent<Text>();
        confirmButton = dialog.transform.Find("ConfirmButton").gameObject.GetComponent<Button>();
        confirmButton.onClick.AddListener(OnClickConfirmButton);
        scale = new Vector2(0.0f, 100.0f);
        dialog.transform.localScale = scale;
        alpha = 0.0f;
        imageColor = new Color32(255, 255, 255, 0);
        textColor = new Color32(255, 255, 255, 0);
        itemImage.color = imageColor;
        itemName.color = textColor;
        itemDetail.color = textColor;
        confirmButton.gameObject.SetActive(false);
        state = GoalDialogState.OPEN;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case GoalDialogState.WAIT:
                break;
            case GoalDialogState.OPEN:
                if (scale.x < 100.0f)
                {
                    scale.x += openDialogSpeed * Time.unscaledDeltaTime;
                    dialog.transform.localScale = scale;
                }
                else
                {
                    scale.x = 100.0f;
                    dialog.transform.localScale = scale;
                    state = GoalDialogState.SHOW_TEXT;
                }
                break;
            case GoalDialogState.SHOW_TEXT:
                if (alpha < 255.0f)
                {
                    alpha += alphaSpeed * Time.unscaledDeltaTime;
                    textColor.a = (byte)alpha;
                    imageColor.a = (byte)alpha;
                    itemImage.color = imageColor;
                    itemName.color = textColor;
                    itemDetail.color = textColor;
                }
                else
                {
                    confirmButton.gameObject.SetActive(true);
                    state = GoalDialogState.FINISH;
                }
                break;
            default:
                break;
        }
    }

    public void OnClickConfirmButton()
    {
        gameObject.SetActive(false);
        SceneManager.LoadScene("MapScene");
    }
}
