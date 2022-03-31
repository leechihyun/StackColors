using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLine : MonoBehaviour
{
    public List<Block> blocks;
    public float intervalZ;
    public EColor eColor;


    private void OnValidate()
    {
        blocks.Clear();

        foreach(Transform child in transform)
        {
            Block block = child.GetComponent<Block>();
            blocks.Add(block);
        }
    }

    [ContextMenu("Sort Straight")]
    private void SortStraight() {
        for (int i=0; i<transform.childCount; ++i) {
            Transform child = transform.GetChild(i);
            Block block = child.GetComponent<Block>();

            block.transform.localPosition = new Vector3(0f, 0f, intervalZ * i);
        }
    }

    [ContextMenu("Set Color")]
    void SetColor()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            Transform child = transform.GetChild(i);
            Block block = child.GetComponent<Block>();
            block.startEColor = eColor;
            block.nowEColor = eColor;
        }
    }

}
