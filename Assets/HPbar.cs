using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;

public class HPbar: MonoBehaviour
{
    public Slider slider;   // 获得Slider对象

    public void SetHP(int currentHP)
    {
        // 设置slider的value为当前血量
        slider.value = currentHP;   
    }

    public void SetMaxHP(int maxHP)
    {
        // 设置slider的最大值，并设置value为最大血量
        slider.maxValue = maxHP;
        slider.value = maxHP;
    }
}
