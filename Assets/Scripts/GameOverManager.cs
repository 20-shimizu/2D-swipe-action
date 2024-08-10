using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public Button retryButton;
    public Button stageSelectButton;

    void Start()
    {
        // ボタンにリスナーを追加
        retryButton.onClick.AddListener(Retry);
        stageSelectButton.onClick.AddListener(GoToStageSelect);

        // 初期状態では非表示
        gameObject.SetActive(false);
    }

    public void ShowGameOverUI()
    {
        gameObject.SetActive(true);
    }

    void Retry()
    {
        Debug.Log("Retry");
    }

    void GoToStageSelect()
    {
        Debug.Log("Go to Stage Select");
    }
}