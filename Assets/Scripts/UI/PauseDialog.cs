using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseDialog : MonoBehaviour
{
    private GameObject dialog;
    private Button closeButton;
    private Button moveToMapButton;
    private StageManager stageManager;

    void Start()
    {
        dialog = transform.Find("Canvas/Dialog").gameObject;
        closeButton = dialog.transform.Find("CloseButton").gameObject.GetComponent<Button>();
        moveToMapButton = dialog.transform.Find("MoveToMapButton").gameObject.GetComponent<Button>();
        gameObject.SetActive(false);
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        closeButton.onClick.AddListener(OnClickCloseButton);
        moveToMapButton.onClick.AddListener(OnClickMoveToMapButton);
    }

    private void OnClickCloseButton()
    {
        stageManager.OnGame();
        gameObject.SetActive(false);
    }
    private void OnClickMoveToMapButton()
    {
        stageManager.OnGame();
        gameObject.SetActive(false);
        SceneManager.LoadScene("MapScene");
    }
}
