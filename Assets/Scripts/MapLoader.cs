using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System;
using System.Text;

public class MapLoader : MonoBehaviour
{
    
    public GameObject prefab;
    GameObject container;
    GameObject prefabInstancei;
    public Dictionary<int,GameObject> playerMap = new Dictionary<int, GameObject>();
    int row; int col;
    // Start is called before the first frame update
    private static MapLoader _instance;
    public static MapLoader GetInstance(){

        if (_instance == null) {
            _instance = new MapLoader();
        }
        return _instance;
    }
    void Start()
    {
        container = GameObject.Find("Container");
        GameObject pfb_empty = Resources.Load("FreeDimensionForge/LowPolyEnvironmentsPack/Art/ForestEnvironmentPack/Prefabs/ForestEmpty") as GameObject;
        GameObject pfb_tree = Resources.Load("FreeDimensionForge/LowPolyEnvironmentsPack/Art/ForestEnvironmentPack/Prefabs/ForestTreeSpruce_01") as GameObject;
        GameObject pfb_road_cross = Resources.Load("FreeDimensionForge/LowPolyEnvironmentsPack/Art/ForestEnvironmentPack/Prefabs/ForestCrossing_01") as GameObject;
        GameObject pfb_road_straight = Resources.Load("FreeDimensionForge/LowPolyEnvironmentsPack/Art/ForestEnvironmentPack/Prefabs/ForestStraight") as GameObject;
        GameObject pfb_road_Tcross = Resources.Load("FreeDimensionForge/LowPolyEnvironmentsPack/Art/ForestEnvironmentPack/Prefabs/ForestCrossing_02") as GameObject;
        GameObject pfb_road_curve = Resources.Load("FreeDimensionForge/LowPolyEnvironmentsPack/Art/ForestEnvironmentPack/Prefabs/ForestCurve") as GameObject;
        GameObject pfb_water = Resources.Load("FreeDimensionForge/LowPolyEnvironmentsPack/Art/ForestEnvironmentPack/Prefabs/ForestLake") as GameObject;
        GameObject pfb_forest = Resources.Load("FreeDimensionForge/LowPolyEnvironmentsPack/Art/ForestEnvironmentPack/Prefabs/forest") as GameObject;
        GameObject pfb_mountain = Resources.Load("FreeDimensionForge/LowPolyEnvironmentsPack/Art/ForestEnvironmentPack/Prefabs/mountain") as GameObject;
        GameDataMgr.GetInstance().Init();
        GameDataMgr dataMgr = GameDataMgr.GetInstance();
        
        dataMgr.Init();
        foreach(int id in dataMgr.playerInfos.Keys){
            Player player = dataMgr.playerInfos[id];
            GameObject pfb_player = Resources.Load("player/Basic People") as GameObject;

            playerMap.Add(id,pfb_player);
            putObject(pfb_player,(float)player.posX,(float)player.posY,0,0,"player_"+id.ToString());

           // Debug.Log("player_"+id.ToString()+(float)player.posX+" "+(float)player.posY);
        }
        
        //putObject(pfb_tree,0,0,0,0,"tree");
        Debug.Log("Map loaded");
        // GameObject player = Resources.Load("player/Female A") as GameObject;
        // Debug.Log(player);
        // putObject(player,5,-5,0,5);
        /*
        r1: === r2: |   r3:  +  r4: -|   r5: |-  r6: |     r7:    ---
                                                    ---            |
        r8: ==\    r9:  ||      r10:  ||      r11: /==      r12: water   r13: forest
              ||       ==/            \==          ||
        */
        // string[,] topography = new string[,]{
        //     {"r11","r1", "r1","r7", "r1", "r1","r8"},
        //     {"r2", "r12",  "r12", "r2", "r13",  "r13", "r2"},
        //     {"r2", " ",  "r12", "r2", "r11","r8","r2"},
        //     {"r2", "r11","r1","r3", "r4", "r5","r4"},
        //     {"r5", "r9", " ", "r2", "r10","r3", "r4"},
        //     {"r2", " ",  " ", "r2", "r12",  "r2", "r2"},
        //     {"r10","r1", "r1","r6", "r1", "r6","r9"}
        // };

        //string info = ResMgr.GetInstance().Load<TextAsset>("Map/data1").text;
        //string [] topography;
        //topography =  info.Split('\n'); 
        string[,] objs = new string[,]{      
        };
        string[,] map = dataMgr.loadedMap;

        row = dataMgr.row;
        col = dataMgr.col;
        for (int i = 0; i < row; i++){
            for(int j= 0; j < col; j++){
                
                float x = i;
                float y = j;
                int passType = 0;
                switch(map[i,j]){
                    case "r":  
                        if(i-1>=0 && map[i-1, j]=="r"){ //上面是路
                            if(j-1>=0 && map[i, j-1]=="r"){ //左边是路
                                if(j+1<col && map[i, j+1]=="r"){ //右边是路
                                    if(i+1<row && map[i+1, j]=="r"){ //下边是路
                                        putObject(pfb_road_cross, x, y, 0, 0); 
                                    }else{
                                        putObject(pfb_road_Tcross, x, y, 90, 0);
                                    }
                                }else{
                                    if(i+1<row && map[i+1, j]=="r"){ //下边是路
                                        putObject(pfb_road_Tcross, x, y, 0, 0);
                                    }else{
                                        putObject(pfb_road_curve, x, y, 90, 0);
                                    }
                                }
                            }else{
                                if(j+1<col && map[i, j+1]=="r"){ //右边是路
                                    if(i+1<row && map[i+1, j]=="r"){ //下边是路
                                        putObject(pfb_road_Tcross, x, y, 180, 0);
                                    }else{
                                        putObject(pfb_road_curve, x, y, 180, 0);
                                    }
                                }else{
                                    if(i+1<row && map[i+1, j]=="r"){ //下边是路
                                        putObject(pfb_road_straight, x, y, 90, 0);
                                    }else{
                                        putObject(pfb_road_straight, x, y, 90, 0);
                                    }
                                }
                            }
                        }else{
                            if(j-1>=0 && map[i, j-1]=="r"){ //左边是路
                                if(j+1<col && map[i, j+1]=="r"){ //右边是路
                                    if(i+1<row && map[i+1, j]=="r"){ //下边是路
                                        putObject(pfb_road_Tcross, x, y, 270, 0);
                                    }else{
                                        putObject(pfb_road_straight, x, y, 0, 0);
                                    }
                                }else{
                                    if(i+1<row && map[i+1, j]=="r"){ //下边是路
                                        putObject(pfb_road_curve, x, y, 0, 0);
                                    }else{
                                        putObject(pfb_road_straight, x, y, 0, 0);
                                    }
                                }
                            }else{
                                if(j+1<col && map[i, j+1]=="r"){ //右边是路
                                    if(i+1<row && map[i+1, j]=="r"){ //下边是路
                                        putObject(pfb_road_curve, x, y, 270, 0);
                                    }else{
                                        putObject(pfb_road_straight, x, y, 0, 0);
                                    }
                                }else{
                                    if(i+1<row && map[i+1, j]=="r"){ //下边是路
                                        putObject(pfb_road_straight, x, y, 90, 0);
                                    }else{
                                        putObject(pfb_road_straight, x, y, 90, 0);
                                    }
                                }
                            }
                        }
                        break;
                    case "g": putObject(pfb_empty, x, y, 0, 0);  break;
                    case "t": putObject(pfb_forest, x, y, 0, 0,"forest_"+((int)x).ToString()+"_"+((int)y).ToString()); passType = 2; break;
                    case "w": putObject(pfb_water,x,y,0,0,"water_"+((int)x).ToString()+"_"+((int)y).ToString()); passType = 1; break;
                    case "c": putObject(pfb_mountain, x, y, 0, 0,"mountain_"+((int)x).ToString()+"_"+((int)y).ToString()); passType = 1;   break;

                    // case "r2": putObject(pfb_road_straight, x, y, 90, 0);  break;
                    // case "r3": putObject(pfb_road_cross, x, y, 0, 0);  break;
                    // case "r4": putObject(pfb_road_Tcross, x, y, 0, 0);  break;
                    // case "r5": putObject(pfb_road_Tcross, x, y, 180, 0);  break;
                    // case "r6": putObject(pfb_road_Tcross, x, y, 90, 0);  break;
                    // case "r7": putObject(pfb_road_Tcross, x, y, 270, 0);  break;
                    // case "r8": putObject(pfb_road_curve, x, y, 0, 0);  break;
                    // case "r9": putObject(pfb_road_curve, x, y, 90, 0);  break;
                    // case "r10": putObject(pfb_road_curve, x, y, 180, 0);  break;
                    // case "r11": putObject(pfb_road_curve, x, y, 270, 0);  break;
                    // case "r12": putObject(pfb_water,x,y,0,0); passType = 1; break;
                    // case "r13": putObject(pfb_forest, x, y, 0, 0); passType = 2; break;
                    default:    putObject(pfb_empty, x, y, 0, 0);  break;
                }
                dataMgr.updateMapInfo(i, j, passType);
            }
        }
        


        int objRow = objs.GetLength(0);
        int objCol = objs.GetLength(1);
        // Assert.AreEqual(4, objCol);
        for (int i = 0; i < objRow; i++){
            try{
                GameObject pfb = Resources.Load("FreeDimensionForge/LowPolyEnvironmentsPack/Art/ForestEnvironmentPack/Prefabs/"+objs[i, 0]) as GameObject;
                putObject(pfb, float.Parse(objs[i, 1]) - 0.5f, float.Parse(objs[i, 2]) - 0.5f, int.Parse(objs[i, 3]));
            }
            catch{
                Debug.Log("ERROR: obj "+objs[i, 0]+" does not exist!\n");
            }
        }
        setBorder();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(int id in playerMap.Keys){
            //GameObject player_obj = playerMap[id];

            GameObject player_obj = GameObject.Find("player_"+id);
            Player player = GameDataMgr.GetInstance().playerInfos[id];
            float x = (float)(-(row-1)/2.0f + player.posX);
            float y = (float)(-(col-1)/2.0f + player.posY);
            //Debug.Log($"{x},{y},{player.posX},{player.posY}");
            player_obj.transform.position = new Vector3(y,0,-x);
            //player_obj.transform.position = new Vector3((float)(player.posY),0,(float)-player.posX); 
        }
        GameDataMgr dataMgr = GameDataMgr.GetInstance();
        if(dataMgr.restart){
            dataMgr.restart = false;
            string[,] map = dataMgr.loadedMap;
            row = dataMgr.row;
            col = dataMgr.col;
            GameObject pfb_forest = Resources.Load("FreeDimensionForge/LowPolyEnvironmentsPack/Art/ForestEnvironmentPack/Prefabs/forest") as GameObject;
            for (int i = 0; i < row; i++){
                for(int j= 0; j < col; j++){
                    if(map[i,j]=="t" && dataMgr.map[i,j]==0){
                        String name = "empty_"+i.ToString()+"_"+j.ToString();
                        GameObject toDelete = GameObject.Find(name);
                        dataMgr.forest[(i,j)] = 1;
                        dataMgr.map[i,j] = 2;
                        GameObject.Destroy(toDelete);
                        putObject(pfb_forest, (float)i, (float)j, 0, 0,"forest_"+i.ToString()+"_"+j.ToString());
                    }
                }
            }
            dataMgr.end = false;
        }
    }

    public void putObject(GameObject o, float posX, float posY, int rotate, float posZ = 0.1f,string name = null){
        float x = -(row-1)/2.0f + posX;
        float y = -(col-1)/2.0f + posY;
        prefabInstancei = Instantiate(o);
        if(name!=null){
            prefabInstancei.name = name;
        }
        container = GameObject.Find("Container");
        prefabInstancei.transform.parent = container.transform;
        
        prefabInstancei.transform.position = new Vector3(y, posZ, -x);
        if(rotate != 0)
            prefabInstancei.transform.Rotate(0,rotate,0);
    }

    void setBorder(){
        
        float x = -(row-1)/2.0f - 1;
        float y = -(col-1)/2.0f - 1;
        GameObject border = new GameObject();
        border.AddComponent<BoxCollider>();
        var boxCollider = (BoxCollider)border.GetComponent<BoxCollider>();
        boxCollider.center = new Vector3(y, 0, 0);
        boxCollider.size = new Vector3(1,1,col);

        GameObject border2 = new GameObject();
        border2.AddComponent<BoxCollider>();
        boxCollider = (BoxCollider)border2.GetComponent<BoxCollider>();
        boxCollider.center = new Vector3(-y, 0, 0);
        boxCollider.size = new Vector3(1,1,col);

        GameObject border3 = new GameObject();
        border3.AddComponent<BoxCollider>();
        boxCollider = (BoxCollider)border3.GetComponent<BoxCollider>();
        boxCollider.center = new Vector3(0, 0, x);
        boxCollider.size = new Vector3(row,1,1);

        GameObject border4 = new GameObject();
        border4.AddComponent<BoxCollider>();
        boxCollider = (BoxCollider)border4.GetComponent<BoxCollider>();
        boxCollider.center = new Vector3(0, 0, -x);
        boxCollider.size = new Vector3(row,1,1);
    }
}
