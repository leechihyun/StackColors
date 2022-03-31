using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    [Header("Ready UI")]
    [SerializeField] GameObject readyUI;

    [Header("Game UI")]
    [SerializeField] GameObject gameUI;
    [SerializeField] TextMeshProUGUI scoreText;


    [Header("Fever UI")]
    [SerializeField] GameObject feverUI;

    [Header("Charge UI")]
    [SerializeField] GameObject chargeUI;


    [Header("Clear UI")]
    [SerializeField] GameObject clearUI;
    [SerializeField] TextMeshProUGUI lastScoreText;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameManager.instance.doReadyState += () => 
        {
            readyUI.SetActive(true);
        };

        GameManager.instance.doRunState += () =>
        {
            readyUI.SetActive(false);
            gameUI.SetActive(true);
            feverUI.SetActive(true);
        };

        GameManager.instance.doChargeState += () =>
        {
            chargeUI.SetActive(true);
            feverUI.SetActive(false);
        };

        GameManager.instance.doKickState += () =>
        {
            chargeUI.SetActive(false);
        };

        GameManager.instance.doBonusState += () =>
        {

        };


        GameManager.instance.doClearState += () =>
        {
            clearUI.SetActive(true);
        };

        GameManager.instance.player.doGetBlock += (float stackHeight) =>
        {
            int score = (int)(stackHeight * 10);
            scoreText.text = score.ToString();
            lastScoreText.text = score.ToString();
        };
    }



}
