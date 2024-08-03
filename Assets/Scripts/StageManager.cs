using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private PlayerController playerCtrl;
    // 時間経過がスローになっているときの倍率
    [SerializeField]
    private float slowRate;

    void Start()
    {
        Physics2D.simulationMode = SimulationMode2D.Script;
        playerCtrl = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void FixedUpdate()
    {
        if (playerCtrl.isAiming) Physics2D.Simulate(slowRate * Time.fixedDeltaTime);
        else Physics2D.Simulate(Time.fixedDeltaTime);
    }
}
