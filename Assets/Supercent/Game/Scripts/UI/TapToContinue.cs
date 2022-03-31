using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TapToContinue : MonoBehaviour
{
    private void OnEnable()
    {
        transform.DOScale(Vector3.one * 1.1f, 1f).SetLoops(-1, LoopType.Yoyo);
    }


}
