using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalItem : MonoBehaviour
{
    private PostProcessController postProcessController;
    private float bloomIntensity = 0.0f;

    void Start()
    {
        postProcessController = GameObject.Find("GlobalVolume").GetComponent<PostProcessController>();
    }
    void Update()
    {
        bloomIntensity = Mathf.PingPong(Time.time / 0.1f, 5.0f) + 5.0f;
        postProcessController.SetBloomIntensity(bloomIntensity);
    }

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
