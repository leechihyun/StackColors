using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ClearUI : MonoBehaviour
{
    [SerializeField] AnimationCurve curve;

    [SerializeField] TextMeshProUGUI multipleScoreText;
    [SerializeField] Vector3 multipleScoreNextPosition;


    [SerializeField] RectTransform getGoldPanel;

    [SerializeField] RectTransform goldLine;
    [SerializeField] TextMeshProUGUI plusText;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] Image coinImage;

    [SerializeField] RectTransform nowCoin;
    [SerializeField] int coinsCount = 50;
    [SerializeField] GameObject coinImageParent;
    [SerializeField] GameObject coinImagePrefab;
    [SerializeField] RectTransform coinMoveTarget;

    [SerializeField] GameObject tapToContinueText;

    List<RectTransform> coins = new List<RectTransform>();

    private void Awake()
    {
        for (int i = 0; i < coinsCount; i++)
        {
            GameObject inst = Instantiate(coinImagePrefab);
            inst.SetActive(false);
            inst.transform.parent = coinImageParent.transform;

            CoinImage c = inst.GetComponent<CoinImage>();
            c.targetPosition = coinMoveTarget.localPosition;

            RectTransform rt = inst.GetComponent<RectTransform>();
            coins.Add(rt);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(Routine());
    }

    IEnumerator Routine()
    {
        yield return StartCoroutine(MultipleScoreRoutine());
        yield return new WaitForSeconds(0.3f);
        yield return StartCoroutine(GetGoldRoutine());
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(GetGoldMessageRoutine());
        yield return StartCoroutine(SpawnCoinRoutine());
        yield return new WaitForSeconds(1f);
        tapToContinueText.gameObject.SetActive(true);
    }


    IEnumerator MultipleScoreRoutine()
    {
        float time = 0f;
        float delta = 0f;
        float duration = 0.5f;
        Color color = multipleScoreText.color;
        Vector3 scale = multipleScoreText.transform.localScale;

        while (delta <= 1f)
        {
            multipleScoreText.color = Color.Lerp(color, new Color(1f, 1f, 1f, 1f), curve.Evaluate(delta));
            multipleScoreText.transform.localScale = Vector3.Lerp(scale, Vector3.one, curve.Evaluate(delta));

            time += Time.deltaTime;
            delta = time / duration;
            yield return null;
        }

        yield return new WaitForSeconds(0.25f);

        time = 0f;
        delta = 0f;
        color = multipleScoreText.color;
        Vector3 startPos = multipleScoreText.transform.localPosition;

        while (delta <= 1f)
        {
            multipleScoreText.color = Color.Lerp(color, new Color(0f, 0f, 0f, 0f), curve.Evaluate(delta));
            multipleScoreText.transform.localPosition = Vector3.Lerp(startPos, multipleScoreNextPosition, delta);

            time += Time.deltaTime;
            delta = time / duration;
            yield return null;
        }
    }

    IEnumerator GetGoldRoutine()
    {
        float time = 0f;
        float delta = 0f;
        float duration = 0.5f;
        Vector3 startPos = getGoldPanel.localPosition;

        while(delta <= 1f)
        {
            getGoldPanel.localPosition = Vector3.Lerp(startPos, Vector3.zero, delta);

            time += Time.deltaTime;
            delta = time / duration;
            yield return null;
        }
    }

    IEnumerator GetGoldMessageRoutine()
    {
        float time = 0f;
        float delta = 0f;
        float duration = 0.3f;
        Vector3 goldlineScale = goldLine.transform.localScale;
        Color coinColor = coinImage.color;
        Color plusColor = plusText.color;
        Color textColor = moneyText.color;

        while(delta <= 1f)
        {
            goldLine.localScale = Vector3.Lerp(goldlineScale, Vector3.one, delta);
            coinImage.color = Color.Lerp(coinColor, new Color(1f, 1f, 1f, 1f), delta);
            plusColor = Color.Lerp(plusColor, new Color(1f, 1f, 1f, 1f), delta);
            moneyText.color = Color.Lerp(textColor, new Color(1f, 1f, 1f, 1f), delta);

            time += Time.deltaTime;
            delta = time / duration;
            yield return null;
        }

    }

    IEnumerator SpawnCoinRoutine()
    {
        float minX = -450f, maxX = 450f, minY = -450f, maxY = 450f;
        for (int i = 0; i < coinsCount; i++)
        {
            Vector2 rand = new Vector2(Random.Range(minX, maxX),
                                       Random.Range(minY, maxY));

            coins[i].localPosition = rand;
            coins[i].gameObject.SetActive(true);
        }

        yield return null;

    }

}
