using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;  //Slider的调用需要引用UI源文件

public class HPCtrollerForStarving : MonoBehaviour
{
    public Slider STARVING;  //实例化一个Slider
    public GameDataMgr gameDataMgr;

    private String Oname;
	private GameObject obj;
	private int id;
    
    private void Start()
    {
        //Value的值介于0-1之间，且为浮点数
        STARVING.value = 100;
    }

    void Update()
    {
        obj = this.transform.parent.parent.gameObject;
		Oname = obj.name;
        // STARVING.value -= Time.deltaTime / 0.5f;
        // GameDataMgr.GetInstance().playerInfos[0].starving = (float)STARVING.value;
        Oname = Oname.Split('_')[1];
		id = int.Parse(Oname);
        if(!GameDataMgr.GetInstance().playerInfos[id].alive){
            GameDataMgr.GetInstance().playerInfos[id].starving = 0;
            return;
        }
        GameDataMgr.GetInstance().playerInfos[id].starving -= Time.deltaTime / 0.1f;
        if(GameDataMgr.GetInstance().playerInfos[id].starving<0){
            GameDataMgr.GetInstance().playerInfos[id].starving = 0;
        }
        STARVING.value = (float)GameDataMgr.GetInstance().playerInfos[id].starving;
        
    }
}