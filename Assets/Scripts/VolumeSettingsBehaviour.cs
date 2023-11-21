using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSettingsBehaviour : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Toggle _toggle;

    // private bool isToggleOn;

    // always set the slider.interactable to the toggle value
    private void Start()
    {
        _toggle.onValueChanged.AddListener(delegate {
                        ToggleSliderInteractable();
                        });
    }

    private float saved_slider_value;
    private void ToggleSliderInteractable(){
        _slider.interactable = _toggle.isOn;
        
        if (_toggle.isOn) {
            _slider.value = saved_slider_value;
        }else{
            saved_slider_value = _slider.value;
            _slider.value = 0;
        }

    }
}
