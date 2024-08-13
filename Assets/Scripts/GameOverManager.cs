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

    public void HideGameOverUI()
    {
        gameObject.SetActive(false);
    }

    void Retry()
    {
        HideGameOverUI();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void GoToStageSelect()
    {
        HideGameOverUI();
        SceneManager.LoadScene("MapScene");
    }
}