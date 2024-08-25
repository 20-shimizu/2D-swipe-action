using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EntryDialog : MonoBehaviour
{
    private GameObject entryDialog;
    private Text guideText;
    private string stageName;
    private Button entryButton;
    private Button backButton;

    private TimeManager timeManager;
    // Start is called before the first frame update
    void Start()
    {
        entryDialog = transform.Find("Canvas/EntryDialog").gameObject;
        guideText = entryDialog.transform.Find("GuideText").gameObject.GetComponent<Text>();
        entryButton = entryDialog.transform.Find("EntryButton").gameObject.GetComponent<Button>();
        backButton = entryDialog.transform.Find("BackButton").gameObject.GetComponent<Button>();
        entryButton.onClick.AddListener(OnClickEntryButton);
        backButton.onClick.AddListener(OnClickBackButton);

        timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
    }

    public void ShowDialog(string stageName)
    {
        timeManager.TimeStop(true);
        entryDialog.SetActive(true);
        guideText.text = stageName + "\nに入りますか？";
        this.stageName = stageName;
    }
    private void OnClickEntryButton()
    {
        timeManager.TimeStop(false);
        SceneManager.LoadScene(stageName);
    }
    private void OnClickBackButton()
    {
        timeManager.TimeStop(false);
        entryDialog.SetActive(false);
    }
}
