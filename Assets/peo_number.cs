using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class peo_number : MonoBehaviour
{
    float updateInterval = 1.0f;           //当前时间间隔
    private float accumulated = 0.0f;      //在此期间累积  
    private float frames = 0;              //在间隔内绘制的帧  
    private float timeRemaining;           //当前间隔的剩余时间
    private float fps = 15.0f;             //当前帧 Current FPS
    private float lastSample;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject); //不销毁此游戏对象，在哪个场景都可以显示，，不需要则注释
        timeRemaining = updateInterval;
        lastSample = Time.realtimeSinceStartup; //实时自启动

    }
	
    void Update()
    {
        // 获得游戏帧数
        ++frames;
        float newSample = Time.realtimeSinceStartup;
        float deltaTime = newSample - lastSample;
        lastSample = newSample;
        timeRemaining -= deltaTime;
        accumulated += 1.0f / deltaTime;
  
        if (timeRemaining <= 0.0f)
        {
            fps = accumulated / frames;
            timeRemaining = updateInterval;
            accumulated = 0.0f;
            frames = 0;
        }

    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle
        {
            border = new RectOffset(10, 10, 10, 10),
            fontSize = 50,
            fontStyle = FontStyle.BoldAndItalic,            
        };

        
        int peo_total = 0;
        int peo_alive = 0;
        GameDataMgr dataMgr = GameDataMgr.GetInstance();
        //获得存活人数和总人数
        foreach (int id in dataMgr.playerInfos.Keys){
            //print(id);
            peo_total += 1;
            if ((bool) dataMgr.playerInfos[id].alive.Equals(true)){
                peo_alive += 1;
            }
        }


        //自定义宽度 ，高度大小 颜色，style
        GUI.Label(new Rect(10,10,200,20), "<color=#00ff00><size=20>" + "Alive:" + peo_alive.ToString()+ "</size></color>" + 
         "<color=#00ff00><size=20>" + " / Total:" + peo_total.ToString()+ "</size></color>", style);
        GUI.Label(new Rect(10,30,200,20), "<color=#00ff00><size=20>" + "FPS:" + fps.ToString("f2")+ "</size></color>", style);
    }

}
