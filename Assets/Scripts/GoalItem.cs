using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GoalItem : MonoBehaviour
{
    private AudioManager audioManager;
    private enum GoalItemState
    {
        DESCENT,
        GLOW_UP,
        GLOWING,
        SHOW_DIALOG,
    }
    private GoalItemState state = GoalItemState.DESCENT;
    private PostProcessController postProcessController;
    private float bloomIntensity = 0.0f;
    private StageManager stageManager;
    private GameObject mainCamera;
    private float appearSpeed = 4.0f;
    private float lightSpeed = 2.0f;
    private float endLightIntensity = 3.0f;
    private Light2D spotLight;
    private Vector2 pos;
    private float time = 0.0f;
    private SpriteRenderer spriteRenderer;

    // private GoalDialog goalDialog;
    private StageDialogManager stageDialogManager;

    void Start()
    {
        audioManager = GameObject.Find("AudioSource").GetComponent<AudioManager>();
        postProcessController = GameObject.Find("GlobalVolume").GetComponent<PostProcessController>();
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        mainCamera = GameObject.Find("Main Camera");
        spotLight = transform.Find("SpotLight").gameObject.GetComponent<Light2D>();
        spotLight.intensity = 0.0f;
        pos = transform.position;
        stageDialogManager = GameObject.Find("StageDialogManager").GetComponent<StageDialogManager>();
    }
    public void Initialize(Sprite sprite)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
    }
    void Update()
    {
        bloomIntensity = Mathf.PingPong(Time.unscaledTime / 0.1f, 5.0f) + 5.0f;
        postProcessController.SetBloomIntensity(bloomIntensity);
        switch (state)
        {
            case GoalItemState.DESCENT:
                if (transform.position.y > mainCamera.transform.position.y)
                {
                    pos.y -= appearSpeed * Time.unscaledDeltaTime;
                    transform.position = pos;
                }
                else
                {
                    audioManager.PlaySE("AppearGoalItem");
                    state = GoalItemState.GLOW_UP;
                }
                break;
            case GoalItemState.GLOW_UP:
                if (spotLight.intensity < endLightIntensity)
                {
                    spotLight.intensity = spotLight.intensity + lightSpeed * Time.unscaledDeltaTime;
                }
                else
                {
                    state = GoalItemState.GLOWING;
                }
                break;
            case GoalItemState.GLOWING:
                if (time < 3.0f)
                {
                    time += Time.unscaledDeltaTime;
                }
                else
                {
                    audioManager.PlaySE("GoalDialog");
                    stageDialogManager.ShowDialog("GoalDialog");
                    state = GoalItemState.SHOW_DIALOG;
                }
                break;
            case GoalItemState.SHOW_DIALOG:
                break;
            default:
                break;
        }
    }
}
