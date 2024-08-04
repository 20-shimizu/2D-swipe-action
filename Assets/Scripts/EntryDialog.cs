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
    // Start is called before the first frame update
    void Start()
    {
        entryDialog = transform.Find("Canvas/EntryDialog").gameObject;
        guideText = entryDialog.transform.Find("GuideText").gameObject.GetComponent<Text>();
        entryButton = entryDialog.transform.Find("EntryButton").gameObject.GetComponent<Button>();
        backButton = entryDialog.transform.Find("BackButton").gameObject.GetComponent<Button>();
        entryButton.onClick.AddListener(OnClickEntryButton);
        backButton.onClick.AddListener(OnClickBackButton);
    }

    public void ShowDialog(string stageName)
    {
        entryDialog.SetActive(true);
        guideText.text = stageName + "\nに入りますか？";
        this.stageName = stageName;
    }
    private void OnClickEntryButton()
    {
        SceneManager.LoadScene(stageName);
    }
    private void OnClickBackButton()
    {
        entryDialog.SetActive(false);
    }
}
