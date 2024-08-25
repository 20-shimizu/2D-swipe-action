using UnityEngine;
using System.Collections.Generic;

public class StageDialogManager : MonoBehaviour
{
    [System.Serializable]
    public class DialogEntry
    {
        public string name;
        public GameObject dialogObject;
    }

    public List<DialogEntry> dialogs = new List<DialogEntry>();
    private GameObject currentActiveDialog;

    // // シングルトンインスタンス
    // public static StageDialogManager Instance { get; private set; }

    // private void Awake()
    // {
    //     if (Instance == null)
    //     {
    //         Instance = this;
    //         DontDestroyOnLoad(gameObject);
    //     }
    //     else
    //     {
    //         Destroy(gameObject);
    //     }
    // }

    public void ShowDialog(string dialogName)
    {
        HideCurrentDialog();

        DialogEntry entry = dialogs.Find(d => d.name == dialogName);
        if (entry != null)
        {
            entry.dialogObject.SetActive(true);
            currentActiveDialog = entry.dialogObject;
        }
        else
        {
            Debug.LogWarning($"Dialog '{dialogName}' not found.");
        }
    }

    public void HideCurrentDialog()
    {
        if (currentActiveDialog != null)
        {
            currentActiveDialog.SetActive(false);
            currentActiveDialog = null;
        }
    }

    public void HideAllDialogs()
    {
        foreach (var entry in dialogs)
        {
            entry.dialogObject.SetActive(false);
        }
        currentActiveDialog = null;
    }
}