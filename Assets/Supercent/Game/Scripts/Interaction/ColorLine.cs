using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorLine : MonoBehaviour
{
    public EColor eColor;
    [SerializeField] MeshRenderer meshRenderer;
    public bool isChecked = false;


    private void Start()
    {
        meshRenderer.material.color = ColorType.GetColor(eColor);
    }



}
