using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    // チュートリアルなどがある場合はStATEに追加
    private enum STATE
    {
        ON_GAME, // ゲーム中、操作可能
        GOAL_ITEM_APPEARING, // ゴールアイテムが出てくる演出中
    }

    public bool isGoalItemAppearing;
    private STATE state;
    private TimeManager timeManager;
    // Start is called before the first frame update
    void Start()
    {
        state = STATE.ON_GAME;
        timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        isGoalItemAppearing = state == STATE.GOAL_ITEM_APPEARING;
        if (state == STATE.GOAL_ITEM_APPEARING)
        {
            timeManager.TimeStop(true);
        }
    }

    public void AppearGoalItem()
    {
        state = STATE.GOAL_ITEM_APPEARING;
    }
}
