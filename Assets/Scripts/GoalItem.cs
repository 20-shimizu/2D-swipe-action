using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalItem : MonoBehaviour
{
    private PostProcessController postProcessController;
    private float bloomIntensity = 0.0f;
    private StageManager stageManager;
    private MainCameraController cameraController;

    void Start()
    {
        postProcessController = GameObject.Find("GlobalVolume").GetComponent<PostProcessController>();
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        cameraController = GameObject.Find("Main Camera").GetComponent<MainCameraController>();
        cameraController.SetGoalItem(this);
        stageManager.AppearGoalItem();
    }
    void Update()
    {
        bloomIntensity = Mathf.PingPong(Time.time / 0.1f, 5.0f) + 5.0f;
        postProcessController.SetBloomIntensity(bloomIntensity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && !stageManager.isGoalItemAppearing)
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
