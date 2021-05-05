using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System;
using System.Text;


public class GameDataMgr : BaseManager<GameDataMgr>
{
    public Dictionary<int, Player> playerInfos = new Dictionary<int, Player>();
    public Dictionary<(double, double), int> water = new Dictionary<(double, double), int>();
    public Dictionary<(int X, int Y), int> forest = new Dictionary<(int, int), int>();
    private Dictionary<string, int> mapInfos = new Dictionary<string, int>();
    public int[] deathCounter;
    public int row, col;
    public int[,] map;
    public string[,] loadedMap;
    private int mapWidth, mapHeight;
    public bool restart = false;
    public bool end = true;
    //private static string playerInfo_Url =  Application.persistentDataPath+"/PlayerInfo.csv";
    public void Init()
    {
        // Object obj = ResMgr.GetInstance().Load<Object>("Female_A");
        // Debug.Log(obj);

        string info = ResMgr.GetInstance().Load<TextAsset>("CSV/PlayerInfo").text;
        List<Player> players = new List<Player>();
        players = info.Split('\n')
                        .Skip(1)
                        .Select(v => Player.FromCSV(v))
                        .ToList();
        deathCounter = new int[players.Count];
        for (int i = 0; i < players.Count; i++)
        {
            if (!playerInfos.ContainsKey(players[i].id))
            {
                playerInfos.Add(players[i].id, players[i]);

            }
            deathCounter[i] = 0;
        }


        loadedMap = LoadMap();
        //Debug.Log(loadedMap);
        map = new int[row, col];

    }

    public string[,] LoadMap()
    {
        string info = ResMgr.GetInstance().Load<TextAsset>("Map/map1").text;
        string[] topography;
        topography = info.Split('\n');
        row = topography.GetLength(0);
        string[,] map = new string[30, 30];
        for (int i = 0; i < row; i++)
        {
            string[] topographyLine;
            topographyLine = topography[i].Split('\t');
            col = topographyLine.GetLength(0);
            for (int j = 0; j < col; j++)
            {
                map[i, j] = topographyLine[j];
            }
        }
        return map;
    }
    public void playerAttack(int id, double posX, double posY)
    {
        if (!playerInfos[id].alive)
            return;

        bool canAttack = false;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i != 0 || j != 0)
                {
                    double nx = posX + i;
                    double ny = posY + j;
                    if (nx < 0 || nx >= row || ny < 0 || ny >= col || map[(int)nx, (int)ny] != 0)
                    {
                        continue;
                    }
                    else if (hasPeople((int)nx, (int)ny) != -1)
                    {
                        Player temp = playerInfos[hasPeople((int)nx, (int)ny)];
                        if (!canAttack)
                        {
                            playerInfos[id].averageHealthOnAttack = (float)(playerInfos[id].averageHealthOnAttack * playerInfos[id].attackCount +
                            playerInfos[id].health) / (playerInfos[id].attackCount + 1);
                            playerInfos[id].attackCount += 1;
                            playerInfos[id].aggressivity = playerInfos[id].attackCount * 1.0f / playerInfos[id].canAttackCount;
                        }

                        canAttack = true;

                        if (temp.health == 0)
                            continue;
                        temp.health = Math.Max(0, temp.health - 20);
                        if (temp.health == 0)
                        {
                            temp.alive = false;
                            playerInfos[id].starving = Math.Min(100, playerInfos[id].starving + 20);
                            playerInfos[id].thirsty = Math.Min(100, playerInfos[id].thirsty + 20);
                        }
                    }
                }

            }
        }
    }
    public int hasPeople(int posX, int posY)
    {
        foreach (int id in playerInfos.Keys)
        {
            if ((int)playerInfos[id].posX == posX && (int)playerInfos[id].posY == posY && playerInfos[id].alive) return id;
        }
        return -1;
    }
    public int[,] getPlayerVision(int id, double posX, double posY, int visionRad = 2)
    {
        //0-普通 可通过 1-水 不可通过 2-树林 不可通过 3-普通 不可通过 4-有死人 5-有活人
        int[,] ret = new int[2 * visionRad + 1, 2 * visionRad + 1];

        for (int i = -visionRad; i <= visionRad; i++)
        {
            for (int j = -visionRad; j <= visionRad; j++)
            {
                if (posX + i < 0 || posX + i >= row || posY + j < 0 || posY + j >= col)
                {
                    ret[i + visionRad, j + visionRad] = 3;
                }
                else
                {
                    ret[i + visionRad, j + visionRad] = map[(int)posX + i, (int)posY + j];
                }
                if (hasPeople((int)posX + i, (int)posY + j) != -1)
                {
                    ret[i + visionRad, j + visionRad] = 4;
                }
            }
        }
        return ret;
    }

    public void updatePlayerPos(int id, double posX, double posY)
    {
        if (posX < 0 || posX >= row || posY < 0 || posY >= col || map[(int)posX, (int)posY] != 0 || !playerInfos[id].alive)
        {
            return;
        }
        int temp = hasPeople((int)posX, (int)posY);
        //Debug.Log(temp);
        if (temp == -1)
        {
            playerInfos[id].posX = posX;
            playerInfos[id].posY = posY;
        }

    }
    public void playerRestart(int id)
    {
        // if(posX<0||posX>=row || posY<0 ||posY>=col ||map[(int)posX,(int)posY]!=0){
        //     return;
        // }
        playerInfos[id].alive = true;
        playerInfos[id].dead = false;
        playerInfos[id].health = 100;
        playerInfos[id].thirsty = 100;
        playerInfos[id].starving = 100;
        playerInfos[id].posX = playerInfos[id].startX;
        playerInfos[id].posY = playerInfos[id].startY;
    }
    public void playerDrink(int id, double posX, double posY)
    {
        int[] direction = new int[5] { 0, 1, 0, -1, 0 };
        Boolean drinkable = false;
        if (playerInfos[id].alive)
        {
            for (int i = 0; i < 4; i++)
            {
                int nx = (int)posX + direction[i];
                int ny = (int)posY + direction[i + 1];
                if (nx >= 0 && ny >= 0 && nx < row && ny < col)
                {
                    if (map[nx, ny] == 1)
                    {
                        drinkable = true;
                        int change_x = nx;
                        int change_y = ny;
                    }
                }
            }
            if (drinkable)
            {

                playerInfos[id].averageThirstyOnDrink = (float)(playerInfos[id].averageThirstyOnDrink * playerInfos[id].drinkCount + playerInfos[id].thirsty) / (playerInfos[id].drinkCount + 1);
                playerInfos[id].drinkCount += 1;
                // Debug.Log($"Drink, {playerInfos[id].drinkCount}, {playerInfos[id].averageThirstyOnDrink}");
                playerInfos[id].thirsty = 100;
                //Debug.Log($"Drunk, {playerInfos[id].thirsty}");
            }
        }

    }

    public void putObject(GameObject o, float posX, float posY, int rotate, float posZ = 0.1f, string name = null)
    {
        float x = -(row - 1) / 2.0f + posX;
        float y = -(col - 1) / 2.0f + posY;
        GameObject prefabInstancei = GameObject.Instantiate(o);
        if (name != null)
        {
            prefabInstancei.name = name;
        }
        GameObject container = GameObject.Find("Container");
        prefabInstancei.transform.parent = container.transform;

        prefabInstancei.transform.position = new Vector3(y, posZ, -x);
        if (rotate != 0)
            prefabInstancei.transform.Rotate(0, rotate, 0);
    }

    public void playerEat(int id, double posX, double posY)
    {
        int[] direction = new int[5] { 0, 1, 0, -1, 0 };
        //Boolean eatable = false;
        if (playerInfos[id].alive)
        {
            for (int i = 0; i < 4; i++)
            {
                int nx = (int)posX + direction[i];
                int ny = (int)posY + direction[i + 1];
                if (nx >= 0 && ny >= 0 && nx < row && ny < col)
                {
                    if (map[nx, ny] == 2 && forest[(nx, ny)] == 1)
                    {
                        //eatable = true;
                        playerInfos[id].averageStarveOnEat = (float)(playerInfos[id].averageStarveOnEat * playerInfos[id].eatCount + playerInfos[id].thirsty) / (playerInfos[id].eatCount + 1);
                        playerInfos[id].eatCount += 1;
                        // Debug.Log($"Eat, {playerInfos[id].eatCount}, {playerInfos[id].averageStarveOnEat}");
                        playerInfos[id].starving = Math.Min(100, playerInfos[id].starving + 50);
                        //Debug.Log($"Ate, {playerInfos[id].starving}");
                        forest[(nx, ny)] = 0;

                        //Change UI of the forest
                        map[nx, ny] = 0;

                        float x = -(row - 1) / 2.0f + nx;
                        float y = -(col - 1) / 2.0f + ny;
                        Vector3 posi = new Vector3(y, 0, -x);

                        GameObject container = GameObject.Find("Container");
                        String name = "forest_" + nx.ToString() + "_" + ny.ToString();
                        GameObject toDelete = GameObject.Find(name);
                        foreach (Transform child in toDelete.transform)
                        {
                            child.GetComponent<MeshFilter>().mesh = null;
                        }
                        toDelete.GetComponent<BoxCollider>().enabled = false;
                        GameObject.Destroy(toDelete);
                        GameObject empty = Resources.Load("FreeDimensionForge/LowPolyEnvironmentsPack/Art/ForestEnvironmentPack/Prefabs/ForestEmpty") as GameObject;
                        putObject(empty, nx, ny, 0, 0, "empty_" + ((int)nx).ToString() + "_" + ((int)ny).ToString());


                        return;
                    }
                }
            }
        }
    }


    //用于更新地图块的信息，仅在地图生成的时候调用
    public void updateMapInfo(int posX, int posY, int type)
    {
        map[posX, posY] = type;
        if (type == 2)
        {
            forest[(posX, posY)] = 1;
        }
    }
    public void updateMapSize(int width, int height)
    {
        mapWidth = width;
        mapHeight = height;
    }

    //可以通过坐标查询地图某位置的地形 返回值为以下4个之一 0,1,2,3
    //0-普通 可通过 1-水 不可通过 2-树林 不可通过 3-普通 不可通过
    public int getMapByPos(int posX, int posY)
    {
        // if(mapInfos.ContainsKey(posX.ToString()+"-"+posY.ToString()))
        //     return mapInfos[posX.ToString()+"-"+posY.ToString()];
        // else
        //     return 3;//不存在
        return map[posX, posY];
    }
    //查询地图宽度
    public int getMapWidth()
    {
        return mapWidth;
    }
    //查询地图高度
    public int getMapHeight()
    {
        return mapHeight;
    }


}

[System.Serializable]
public class Player
{
    public int id;
    public double health;
    public double starving;
    public double thirsty;
    public double posX;
    public double posY;
    public double startX;
    public double startY;
    public bool alive;
    public bool dead;
    public float averageThirstyOnDrink;
    public float averageStarveOnEat;
    public double aggressivity;
    public int drinkCount;
    public int eatCount;

    public int attackCount;

    public float averageHealthOnAttack;
    public int canAttackCount;
    public static Player FromCSV(string csvLine)
    {
        string[] values = csvLine.Split(',');
        Player player = new Player();
        player.id = Convert.ToInt32(values[0]);
        player.health = Convert.ToDouble(values[1]);
        player.starving = Convert.ToDouble(values[2]);
        player.thirsty = Convert.ToDouble(values[3]);
        player.posX = Convert.ToDouble(values[4]);
        player.posY = Convert.ToDouble(values[5]);
        player.startX = Convert.ToDouble(values[4]);
        player.startY = Convert.ToDouble(values[5]);
        player.alive = true;
        player.dead = false;
        player.averageHealthOnAttack = 0;
        player.averageStarveOnEat = 0;
        player.averageThirstyOnDrink = 0;
        player.eatCount = 0;
        player.attackCount = 0;
        player.canAttackCount = 0;
        player.drinkCount = 0;

        return player;
    }


}
