using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    // チュートリアルなどがある場合はStATEに追加
    private enum STATE
    {
        ON_GAME, // ゲーム中、操作可能
        POSE,
        BOSS_DYING,
        GOAL_ITEM_APPEARING, // ゴールアイテムが出てくる演出中
        GAME_OVER,
    }

    private STATE state;
    private TimeManager timeManager;
    private MainCameraController cameraController;
    private StageDialogManager stageDialogManager;
    private GameObject player;
    private GameObject boss;

    public bool IsOnGame() { return state == STATE.ON_GAME; }
    public bool IsPose() { return state == STATE.POSE; }

    void Start()
    {
        state = STATE.ON_GAME;
        timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        cameraController = GameObject.Find("Main Camera").GetComponent<MainCameraController>();
        stageDialogManager = GameObject.Find("StageDialogManager").GetComponent<StageDialogManager>();
        player = GameObject.Find("Player");
        boss = GameObject.Find("Boss");
    }

    void Update()
    {
        switch (state)
        {
            case STATE.ON_GAME:
                timeManager.TimeStop(false);
                if (player != null)
                    cameraController.SetDesiredPos(player.transform.position, true);
                break;
            case STATE.POSE:
                timeManager.TimeStop(true);
                break;
            case STATE.BOSS_DYING:
                if (boss != null)
                    cameraController.SetDesiredPos(boss.transform.position, false);
                timeManager.TimeStop(true);
                break;
            case STATE.GOAL_ITEM_APPEARING:
                timeManager.TimeStop(true);
                break;
            case STATE.GAME_OVER:
                timeManager.TimeStop(true);
                break;
            default:
                break;
        }
    }

    public void OnGame()
    {
        state = STATE.ON_GAME;
    }
    public void Pose()
    {
        state = STATE.POSE;
    }
    public void AppearGoalItem()
    {
        state = STATE.GOAL_ITEM_APPEARING;
    }
    public void DieBossEnemy()
    {
        state = STATE.BOSS_DYING;
    }
    public void GameOver()
    {
        state = STATE.GAME_OVER;
        stageDialogManager.ShowDialog("GameOverDialog");
    }
}
