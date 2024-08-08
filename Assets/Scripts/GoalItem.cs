using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class GoalItem : MonoBehaviour
{
    private PostProcessController postProcessController;
    private float bloomIntensity = 0.0f;
    private StageManager stageManager;
    // private MainCameraController cameraController;
    private GameObject mainCamera;
    private float appearSpeed = 4.0f;
    private float lightSpeed = 2.0f;
    private float endLightIntensity = 3.0f;
    private Light2D spotLight;
    private float lightIntensity;
    private GameObject backWindow;
    Vector2 pos;

    void Start()
    {
        postProcessController = GameObject.Find("GlobalVolume").GetComponent<PostProcessController>();
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        mainCamera = GameObject.Find("Main Camera");
        spotLight = transform.Find("SpotLight").gameObject.GetComponent<Light2D>();
        spotLight.intensity = 0.0f;
        backWindow = transform.Find("BackWindow").gameObject;
        pos = transform.position;
    }
    void Update()
    {
        bloomIntensity = Mathf.PingPong(Time.time / 0.1f, 5.0f) + 5.0f;
        postProcessController.SetBloomIntensity(bloomIntensity);
        if (transform.position.y > mainCamera.transform.position.y)
        {
            pos.y -= appearSpeed * Time.deltaTime;
            transform.position = pos;
        }
        else
        {
            if (spotLight.intensity < endLightIntensity)
            {
                lightIntensity += lightSpeed * Time.deltaTime;
                spotLight.intensity = lightIntensity;
            }
        }
    }
    private void StageClear()
    {
        // (TODO)ダイアログマネージャーをシーンに追加、クリア時用のダイアログを表示
        SceneManager.LoadScene("MapScene");
    }
}
