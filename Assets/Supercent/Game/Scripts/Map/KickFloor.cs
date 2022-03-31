using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class KickFloor : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI text;
    [SerializeField] public MeshRenderer meshRenderer;
    [SerializeField] public ParticleSystem[] particleSystems;
    [SerializeField] public bool isChecked = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Block"))
        {
            isChecked = true;

            particleSystems[0].gameObject.SetActive(true);
            particleSystems[1].gameObject.SetActive(true);

            Color.RGBToHSV(meshRenderer.material.color, out float H, out float S, out float V);
            meshRenderer.material.color = Color.HSVToRGB(H, 0.8f, V);
        }
    }


}
