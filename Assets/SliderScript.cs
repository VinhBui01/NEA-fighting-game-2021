using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{

    public Slider slider;

    public void SetCurrent(int currentvalue)
        {slider.value = currentvalue;}

    public void SetMax(int maxvalue)
    { slider.maxValue = maxvalue; }
}
