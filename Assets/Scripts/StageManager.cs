using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private PostProcessController postProcessCtrl;
    private PlayerController playerCtrl;
    // 時間経過がスローになっているときの倍率
    [SerializeField]
    private float slowRate;
    // ヒットストップ、ダイアログ表示などで時間が停止しているときにtrue
    private bool isStopping = false;
    private float vignetteIntensity;

    void Start()
    {
        Physics2D.simulationMode = SimulationMode2D.Script;
        playerCtrl = GameObject.Find("Player").GetComponent<PlayerController>();
        postProcessCtrl = GameObject.Find("GlobalPostProcessVolume").GetComponent<PostProcessController>();
    }

    void FixedUpdate()
    {
        if (!isStopping)
        {
            if (playerCtrl.isAiming)
            {
                Physics2D.Simulate(slowRate * Time.fixedDeltaTime);
                vignetteIntensity = Mathf.PingPong(Time.time / 4.0f, 0.1f) + 0.2f;
            }
            else
            {
                Physics2D.Simulate(Time.fixedDeltaTime);
                vignetteIntensity = 0.0f;
            }
            postProcessCtrl.SetVignetteIntensity(vignetteIntensity);
        }
    }

    public void TimeStop(bool isStop)
    {
        isStopping = isStop;
    }

    public void HitStop(float stopTime)
    {
        StartCoroutine("TimeStopForSeconds", stopTime);
    }
    private IEnumerator TimeStopForSeconds(float time)
    {
        isStopping = true;
        yield return new WaitForSeconds(time);
        isStopping = false;
    }
}
