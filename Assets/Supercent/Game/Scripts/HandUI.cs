using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class HandUI : MonoBehaviour
{
    RectTransform rectTransform;

    Sequence sequence;
    public Vector2 point1, point2;


    private void Awake() {
        rectTransform = GetComponent<RectTransform>();

        rectTransform.localPosition = point1;

        sequence = DOTween.Sequence()
                    .SetAutoKill(false)
                    .Append(rectTransform.DOLocalMove(point2, 1f))
                    .Append(rectTransform.DOLocalMove(point1, 1f))
                    .SetLoops(-1);
    }


    private void OnEnable() {
        sequence.Restart();
    }



}
