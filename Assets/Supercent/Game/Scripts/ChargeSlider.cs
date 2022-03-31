using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeSlider : MonoBehaviour
{
    [SerializeField] public Slider slider;
    [SerializeField] float decreaseDuration;
    
    void Update()
    {
        slider.value -= Time.deltaTime / decreaseDuration;
        slider.value = Mathf.Clamp(slider.value, 0f, 1f);
    }
}
