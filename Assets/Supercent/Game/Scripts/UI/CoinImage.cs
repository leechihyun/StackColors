using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CoinImage : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] AnimationCurve curve;

    public Vector3 targetPosition;


    private void OnEnable()
    {
        image.color = new Color(1f, 1f, 1f, 0f);
        StartCoroutine(FadeRoutine());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator FadeRoutine()
    {
        float time = 0f;
        Color color = image.color;

        while(time < 0.5f)
        {
            image.color = Color.Lerp(color, Color.white, time / 0.5f);
            time += Time.deltaTime;
            yield return null;
        }

        time = 0f;
        float duration = Random.Range(0.5f, 0.75f);
        Vector3 startPos = transform.localPosition;
        while(time < duration)
        {
            transform.localPosition = Vector3.Lerp(startPos, targetPosition, curve.Evaluate(time / duration));

            time += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = targetPosition;

        gameObject.SetActive(false);
    }

}
