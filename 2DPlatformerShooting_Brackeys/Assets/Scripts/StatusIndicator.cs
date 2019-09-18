using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusIndicator : MonoBehaviour
{
    [SerializeField]
    RectTransform healthBarRect;
    [SerializeField]
    Text healthText;

    private void Start()
    {
        if (healthBarRect == null)
            Debug.LogError("No health bar object referenced");
        if (healthText == null)
            Debug.Log("No helath text object referenced");
    }

    public void SetHealth(int _cur, int _max)
    {
        float _value = (float)_cur / _max;

        healthBarRect.localScale = new Vector3(_value, healthBarRect.localScale.y, healthBarRect.localScale.z);
        healthText.text = _cur + "/" + _max + "HP";
    }
}
