using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class PlayerCarrier : MonoBehaviour
{
    [Header("Scene Object")]
    [SerializeField] public Transform carrierPoint;
    [SerializeField] public Transform dustPad;

    [Header("Config")]
    [SerializeField] AnimationCurve blockCurve;
    [SerializeField] float curveForceX = 2f, curveForceZ = 1f;
    [SerializeField] float stackHeight = 0f;
    [SerializeField] float sortSpeed = 10f;
    [SerializeField] List<Block> blocks;


    //Property
    public int BlockCount => blocks.Count;
    public float StackHeight => stackHeight;


    public void PushStack(Block block) {
        blocks.Add(block);
        block.transform.parent = carrierPoint;

        stackHeight += block.scale.y;
    }

    public void SortBlocks(Vector3 deltaPosition){
        Vector3 topPosition = new Vector3(-deltaPosition.x * curveForceX,
                                          stackHeight,
                                          -deltaPosition.z * curveForceZ);
        float yPosition = 0f;

        for (int i=blocks.Count-1; i>=0; --i) {
            Block curBlock = blocks[i];
            Vector3 nextPosition = new Vector3(Mathf.Lerp(0f, topPosition.x, blockCurve.Evaluate(1f - (i / (float)blocks.Count))),
                                               yPosition,
                                               Mathf.Lerp(0f, topPosition.z, blockCurve.Evaluate(1f - (i / (float)blocks.Count))));

            curBlock.transform.localPosition = Vector3.Lerp(curBlock.transform.localPosition, nextPosition, Time.deltaTime * sortSpeed);
            
            yPosition += 0.02f;
            yPosition += curBlock.scale.y;
        }
    }

    public void KickBlocks(float force) {
        for (int i=0; i<blocks.Count; ++i) {
            Block curBlock = blocks[i];
            curBlock.transform.parent = null;

            curBlock.rigidbody.useGravity = true;
            curBlock.rigidbody.isKinematic = false;
            curBlock.collider.isTrigger = false;

            float currentForce = Mathf.Lerp(0f, force, 1f - i / (float)blocks.Count);
            Vector3 forcePosition = curBlock.transform.TransformPoint(0f, 0f, -0.5f);
            Vector3 forceDirection = Vector3.forward;
            curBlock.rigidbody.AddForceAtPosition(currentForce * forceDirection, forcePosition, ForceMode.VelocityChange);

            curBlock.ChangeState(EBlockState.KICKED);
        }
    }

    public bool FailBlock()
    {
        if (blocks.Count > 0)
        {
            Block remove = blocks[blocks.Count - 1];
            blocks.RemoveAt(blocks.Count - 1);

            remove.gameObject.SetActive(false);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ChangeColor(EColor eColor)
    {
        for (int i=0; i<blocks.Count; ++i)
        {
            Block curBlock = blocks[i];

            curBlock.ChangeColor(eColor);
        }
    }



}
