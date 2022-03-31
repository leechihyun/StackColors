using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;
using Cinemachine;
using System;


public enum EGameState {
    READY,
    RUN,
    CHARGE,
    KICK,
    BONUS,
    CLEAR,
    DEAD,
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [Header("Component")]
    [SerializeField] PlayableDirector playableDirector;

    [Header("Scene Objects")]
    public Player player;
    [SerializeField] Map[] maps;

    [Header("Config")]
    [SerializeField] EGameState eState;


    //Callbacks
    public event Action doReadyState;
    public event Action doRunState;
    public event Action doChargeState;
    public event Action doKickState;
    public event Action doBonusState;
    public event Action doClearState;
    public event Action doDeadState;

    public event Action doBonusUpdate;

    //Property
    public EGameState GameState => eState;
    public Map nowMap;


    private void Awake() {
        instance = this;

        //test
        int stageNum = Mathf.Clamp(NowStage.stageNumber, 0, 2);
        //int stageNum = NowStage.stageNumber = 2;
        nowMap = maps[stageNum];
        nowMap.gameObject.SetActive(true);
    }

    private void Start() {
        ChangeState(EGameState.READY);
    }


    private void Update() {
        switch(eState){
            case EGameState.CHARGE:

                break;
            case EGameState.KICK:
                break;
            case EGameState.BONUS:
                doBonusUpdate?.Invoke();

                break;
            default:
                break;
        }
    }

    public void ChangeState(EGameState eState) {
        this.eState = eState;

        switch (eState) {
            case EGameState.READY:
                doReadyState?.Invoke();
                break;
            case EGameState.RUN:
                doRunState?.Invoke();
                break;
            case EGameState.CHARGE:
                doChargeState?.Invoke();
                break;
            case EGameState.KICK:
                doKickState?.Invoke();
                break;
            case EGameState.BONUS:
                doBonusState?.Invoke();
                break;
            case EGameState.CLEAR:
                doClearState?.Invoke();
                break;
        }
    }


    public void PlayTimeline(TimelineAsset timelineAsset)
    {
        playableDirector.Play(timelineAsset);
    }


    #region Button Callback
    public void PressStartButton()
    {
        ChangeState(EGameState.RUN);
    }

    public void PressNextStage()
    {
        NowStage.stageNumber++;
        SceneManager.LoadScene("GameScene");
    }

    #endregion

    public void SetTimeScale(float scale, float duration)
    {
        StartCoroutine(SetTimeScaleRoutine(scale, duration));
    }

    IEnumerator SetTimeScaleRoutine(float scale, float duration)
    {
        Time.timeScale = scale;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }

    #region Timeline Signal
    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    public void KickState()
    {
        ChangeState(EGameState.KICK);
    }
    
    public void BonusState()
    {
        ChangeState(EGameState.BONUS);
    }

    #endregion

}
