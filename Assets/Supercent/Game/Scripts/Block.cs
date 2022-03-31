using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EBlockState
{
    STAND,
    GET,
    KICKED,
    ENDBOUND,
}

public class Block : MonoBehaviour
{
    [Header("Config")]
    public EBlockState eState;
    public EColor startEColor;
    public EColor nowEColor;
    public Vector3 scale;

    [Header("Components")]
    [SerializeField] public Rigidbody rigidbody;
    [SerializeField] public Collider collider;
    [SerializeField] MeshRenderer meshRenderer;


    public bool isBonus = false;

    private void Start() {
        ChangeColor(nowEColor);

        GameManager.instance.player.doFeverColorChange += (EColor eColor) =>
        {
            if (eState == EBlockState.STAND)
            {
                ChangeColor(eColor);
            }
        };

        GameManager.instance.player.doFeverOff += () =>
        {
            if (eState == EBlockState.STAND)
                ChangeColor(startEColor);
        };
    }

    public void ChangeColor(EColor eColor) {
        this.nowEColor = eColor;
        meshRenderer.material.color = ColorType.GetColor(eColor);
    }

    public void FeverChangeColor(EColor color)
    {

    }

    public void ChangeState(EBlockState eState)
    {
        this.eState = eState;
    }


    private void OnCollisionEnter(Collision other) {
        if (eState == EBlockState.KICKED && other.collider.CompareTag("KickFloor")) {
            Vector3 velocity = rigidbody.velocity;
            Vector3 randDirection = new Vector3(Random.Range(0f, 0.25f),
                                                Random.Range(0f, 1f),
                                                Random.Range(0f, 0.25f)).normalized;
            Vector3 randPosition = new Vector3(Random.Range(-scale.x, scale.x),
                                               Random.Range(-scale.y, scale.y),
                                               Random.Range(-scale.z, scale.z));

            rigidbody.AddForceAtPosition(randDirection * 10f, randPosition, ForceMode.VelocityChange);

            eState = EBlockState.ENDBOUND;
        }
    }
}
