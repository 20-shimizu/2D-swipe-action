using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageIcon : MonoBehaviour
{
    [SerializeField]
    private int stageNum = 0;
    private string stageName = "SampleScene";
    private EntryDialog entryDialog;

    void Start()
    {
        if (stageNum > 0)
        {
            stageName = "Stage" + stageNum.ToString();
        }
        entryDialog = GameObject.Find("MapDialogManager").GetComponent<EntryDialog>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            entryDialog.ShowDialog(stageName);
            other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
}
