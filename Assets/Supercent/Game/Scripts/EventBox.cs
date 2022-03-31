using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class EventBox : MonoBehaviour
{
    [SerializeField] TimelineAsset timelineAsset;
    [SerializeField] EGameState changeState;
    bool isPlayed = false;

    private void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnTriggerEnter(Collider other) {
        if (!isPlayed && other.CompareTag("Player")) {
            GameManager.instance.ChangeState(changeState);
            GameManager.instance.PlayTimeline(timelineAsset);
            isPlayed = true;
        }
    }
}
