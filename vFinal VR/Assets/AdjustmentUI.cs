using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class AdjustmentUI : MonoBehaviour
{
    public bool deserializeInAwake = true;
    public Slider _value;
    public  TMP_InputField _field;
    
    [Header("Adjustment value")]
    public float min_value;
    public float max_value;
    public float value;
    public float defaultValue;
    
    [Header("Relative")] public bool isRelative;
    public AdjustmentUI[] others;
    [Header("PlayerPref Key")] public string key;
    void Awake()
    {
        key = gameObject.name;
        defaultValue = value;
        if (deserializeInAwake) value = Deserialize();
        _value.maxValue = max_value;
        _value.minValue = min_value;
        _value.onValueChanged.AddListener(e => OnSliderValueChanged(e));
        _field.onValueChanged.AddListener(e => OnInputFieldValueChanged(e));
        OnSliderValueChanged(value);
        OnInputFieldValueChanged(value.ToString());
    }

    public void OnSliderValueChanged(float newValue)
    {
        value = newValue;
        _field.text = value.ToString();
        if (isRelative)
        {
            float total = 0;
            for (int i = 0; i < others.Length; i++)
            {
                total += others[i].value;
            }

            for (int i = 0; i < others.Length; i++)
            {
                others[i].value /= total;
            }
        }
    }

    public void OnInputFieldValueChanged(string newValue)
    {
        value = float.Parse( newValue);
        value = Mathf.Clamp(value, min_value, max_value);
        _value.value = value;
        if (isRelative)
        {
            float total = value;
            for (int i = 0; i < others.Length; i++)
            {
                total += others[i].value;
            }

            for (int i = 0; i < others.Length; i++)
            {
                others[i].value /= total;
            }
        }
    }

    public void SetDefault()
    {
        value = defaultValue;
        PlayerPrefs.SetFloat(key,defaultValue);
        OnSliderValueChanged(value);
        OnInputFieldValueChanged(value.ToString());
    }
    public void Serialize()
    {
        PlayerPrefs.SetFloat(key,value);
    }

    public float Deserialize()
    {
        if (!PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.SetFloat(key, value);
        }

        return PlayerPrefs.GetFloat(key);
    }
}
