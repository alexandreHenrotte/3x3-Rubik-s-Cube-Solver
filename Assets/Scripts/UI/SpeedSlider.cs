using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedSlider : MonoBehaviour
{
    Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.minValue = 100f;
        slider.maxValue = 2000f;
        slider.value = 400f;
        slider.onValueChanged.AddListener(delegate { OnSliderChange(); });
    }

    public void OnSliderChange()
    {
        Face.rotatingSpeed = slider.value;
    }
}
