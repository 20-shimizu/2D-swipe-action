using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private PlayerController playerCtrl;
    // 時間経過がスローになっているときの倍率
    [SerializeField]
    private float slowRate;
    // ヒットストップ時などで時間が停止しているときにtrue
    private bool isStopping = false;

    void Start()
    {
        Physics2D.simulationMode = SimulationMode2D.Script;
        playerCtrl = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void FixedUpdate()
    {
        if (!isStopping)
        {
            if (playerCtrl.isAiming) Physics2D.Simulate(slowRate * Time.fixedDeltaTime);
            else Physics2D.Simulate(Time.fixedDeltaTime);
        }
    }

    public void HitStop(float stopTime)
    {
        StartCoroutine("TimeStop", stopTime);
    }
    private IEnumerator TimeStop(float time)
    {
        isStopping = true;
        yield return new WaitForSeconds(time);
        isStopping = false;
    }
}
