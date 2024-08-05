using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            StageClear();
        }
    }
    private void StageClear()
    {
        // (TODO)ダイアログマネージャーをシーンに追加、クリア時用のダイアログを表示
        SceneManager.LoadScene("MapScene");
    }
}
