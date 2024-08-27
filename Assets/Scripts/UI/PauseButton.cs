using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    private StageManager stageManager;
    private StageDialogManager stageDialogManager;
    private Button button;
    private Image image;
    private Color32 color = new Color32(255, 255, 255, 255);
    void Start()
    {
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        stageDialogManager = GameObject.Find("StageDialogManager").gameObject.GetComponent<StageDialogManager>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickButton);
        image = GetComponent<Image>();
    }
    void Update()
    {
        if (stageManager.IsOnGame())
        {
            color.a = 255;
            image.color = color;
            button.interactable = true;
        }
        else
        {
            color.a = 20;
            image.color = color;
            button.interactable = false;
        }
    }
    private void OnClickButton()
    {
        stageManager.Pose();
        stageDialogManager.ShowDialog("PauseDialog");
    }
}
