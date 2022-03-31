using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System;
using DG.Tweening;

public enum EPlayerState
{
    READY,
    RUN,
    CHARGE,
    FEVER,
    KICK,
    BONUS,
    CLEAR,
    DEAD,
}

public class Player : MonoBehaviour
{   
    [Header("Component")]
    [SerializeField] PlayerCarrier playerCarrier;
    [SerializeField] PlayerController playerController;
    [SerializeField] PlayerCamera playerCamera;
    [SerializeField] Animator animator;
    [SerializeField] SkinnedMeshRenderer meshRenderer;
    [SerializeField] MeshRenderer dustPadMeshRenderer;
    [SerializeField] AudioSource audioSource;
    [SerializeField] CinemachineImpulseSource impulseSource;
    [SerializeField] Transform dustPadModel;

    [Header("Sound Clip")]
    [SerializeField] AudioClip getBlockSFX;
    [SerializeField] AudioClip changeColorSFX;
    [SerializeField] AudioClip feverSFX;

    [Header("UI")]
    [SerializeField] Slider runProgressSlider;
    [SerializeField] Slider feverSlider;
    [SerializeField] Slider chargeSlider;

    [Header("Config")]
    public EColor eColor;
    public EPlayerState eState;
    public int feverCount = 0;
    public int feverMax = 50;
    public float feverDuration = 5f;
    public float feverDelta = 0f;


    //Callback
    public Action<float> doGetBlock;
    public Action<EColor> doFeverColorChange;
    public Action doFeverOff;


    private void Start() {
        GameManager.instance.doRunState += () => 
        {
            this.ChangeState(EPlayerState.RUN);
            animator.SetBool("doRun", true); 
        };

        GameManager.instance.doChargeState += () =>
        {
            this.ChangeState(EPlayerState.CHARGE);
        };

        GameManager.instance.doKickState += () =>
        {
            this.ChangeState(EPlayerState.KICK);
        };

        GameManager.instance.doBonusState += () =>
        {
            this.ChangeState(EPlayerState.BONUS);
        };


        if (NowStage.stageNumber == 0)
            ChangeColor(EColor.RED);
        else if (NowStage.stageNumber == 1)
            ChangeColor(EColor.BLUE);
        else if (NowStage.stageNumber == 2)
            ChangeColor(EColor.YELLOW);

    }

    private void Update() {
        switch (eState)
        {
            case EPlayerState.READY:
                break;
            case EPlayerState.RUN:
                OnRunUpdate();
                break;
            case EPlayerState.FEVER:
                OnFeverUpdate();
                break;
            case EPlayerState.CHARGE:
                OnChargeUpdate();
                break;
            case EPlayerState.KICK:
                OnKickUpdate();
                break;
            case EPlayerState.BONUS:
                break;
            case EPlayerState.CLEAR:
                break;
            default:
                break;
        }
    }

    public void ChangeState(EPlayerState eState)
    {
        this.eState = eState;

        if (eState == EPlayerState.FEVER)
        {
            playerCamera.runCamera.Priority = 9;
            playerCamera.feverCamera.Priority = 10;

            feverCount = 0;
            feverDelta = feverDuration;

            playerController.forwardSpeed = 20f;

            audioSource.PlayOneShot(feverSFX);

            dustPadModel.DOScaleX(20f, 0.5f);
            GameManager.instance.SetTimeScale(0.25f, 0.5f);

            doFeverColorChange?.Invoke(eColor);
        }
        else if (eState == EPlayerState.DEAD)
        {

        }
    }

    public void OnReadyUpdate()
    {

    }

    public void OnFeverUpdate()
    {
        feverDelta -= Time.deltaTime;

        if (feverDelta <= 0f)
        {
            playerCamera.runCamera.Priority = 10;
            playerCamera.feverCamera.Priority = 9;

            playerController.forwardSpeed = 10f;


            dustPadModel.DOScaleX(4, 0.2f);
            ChangeState(EPlayerState.RUN);

            doFeverOff?.Invoke();
        }


        //Translate
        Vector3 deltaPosition = playerController.Move();

        //Sort Carrier
        playerCarrier.SortBlocks(deltaPosition);

        feverSlider.value = feverDelta / feverDuration;
        runProgressSlider.value = transform.position.z / Map.toChargeDistance;

        animator.SetFloat("runMultiple", playerController.velocity.z / 6f);
    }


    public void OnRunUpdate() {
        //Translate
        Vector3 deltaPosition = playerController.Move();

        //Sort Carrier
        playerCarrier.SortBlocks(deltaPosition);

        //Set Camera Position
        playerCamera.SetCameraOffset(playerCarrier.StackHeight);

        feverSlider.value = feverCount / (float)feverMax;
        runProgressSlider.value = transform.position.z / Map.toChargeDistance;

        animator.SetFloat("runMultiple", playerController.velocity.z / 6f);
    }

    public void OnChargeUpdate() {
        Vector3 nextPosition = transform.position;
        nextPosition.x = Mathf.Lerp(nextPosition.x, 0f, Time.deltaTime * 3f);

        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) {
                chargeSlider.value += 0.2f;
            }
        }

        chargeSlider.value -= Time.deltaTime * 0.25f;

        //nextPosition.z += zSpeed * Time.deltaTime;
        float zSpeed = Mathf.Clamp(30f * chargeSlider.value, 10f, 30f);
        nextPosition.z += zSpeed * Time.deltaTime;

        transform.position = nextPosition;
    }

    public void OnKickUpdate() {
        playerCarrier.carrierPoint.position += new Vector3(0f, 0f, 2f * Time.deltaTime);
        playerCarrier.dustPad.position += new Vector3(0f, 0f, 2f * Time.deltaTime);
    }


    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Block")) {
            Block block = other.GetComponent<Block>();
            if (block && block.eState == EBlockState.STAND) {
                if (block.nowEColor == eColor)
                {
                    block.ChangeState(EBlockState.GET);

                    playerCarrier.PushStack(block);
                    audioSource.PlayOneShot(getBlockSFX);

                    if (eState != EPlayerState.FEVER)
                        feverCount++;

                    if (feverCount >= feverMax)
                    {
                        ChangeState(EPlayerState.FEVER);
                    }


                    doGetBlock?.Invoke(playerCarrier.StackHeight);
                }
                else
                {
                    if (!playerCarrier.FailBlock())
                    {
                        GameManager.instance.ChangeState(EGameState.DEAD);
                    }
                }
            }
        }
        else if (other.CompareTag("ColorLine")) {
            ColorLine colorLine = other.GetComponent<ColorLine>();
            if (!colorLine.isChecked)
            {
                colorLine.isChecked = true;
                ChangeColor(colorLine.eColor);

                audioSource.PlayOneShot(changeColorSFX);

                if (eState == EPlayerState.FEVER)
                    doFeverColorChange?.Invoke(eColor);
            }            
        }
        else if (other.CompareTag("Coin")) {

        }
    }

    public void ChangeColor(EColor eColor)
    {
        this.eColor = eColor;
        meshRenderer.material.color = ColorType.GetColor(eColor);
        dustPadMeshRenderer.material.color = ColorType.GetColor(eColor);

        playerCarrier.ChangeColor(eColor);
    }

    public void KickBlocks() {
        playerCarrier.KickBlocks(Mathf.Clamp(150f * chargeSlider.value, 50f, 150f));
    }



    #region Signal
    public void GenerateImpulse()
    {
        impulseSource.GenerateImpulse();
    }

    #endregion
}
