using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    // チュートリアルなどがある場合はStATEに追加
    private enum STATE
    {
        ON_GAME, // ゲーム中、操作可能
        BOSS_DYING,
        GOAL_ITEM_APPEARING, // ゴールアイテムが出てくる演出中
    }

    private STATE state;
    private TimeManager timeManager;
    private MainCameraController cameraController;
    private GameObject player;
    private GameObject boss;
    void Start()
    {
        state = STATE.ON_GAME;
        timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        cameraController = GameObject.Find("Main Camera").GetComponent<MainCameraController>();
        player = GameObject.Find("Player");
        boss = GameObject.Find("Boss");
    }

    void Update()
    {
        // 各stateでカメラが何を追うかを指定する
        switch (state)
        {
            case STATE.ON_GAME:
                cameraController.SetDesiredPos(player.transform.position, true);
                break;
            case STATE.BOSS_DYING:
                if (boss != null)
                    cameraController.SetDesiredPos(boss.transform.position, false);
                timeManager.TimeStop(true);
                break;
            case STATE.GOAL_ITEM_APPEARING:
                timeManager.TimeStop(true);
                break;
            default:
                break;
        }
    }

    public void AppearGoalItem()
    {
        state = STATE.GOAL_ITEM_APPEARING;
    }
    public void DieBossEnemy()
    {
        state = STATE.BOSS_DYING;
    }
}
