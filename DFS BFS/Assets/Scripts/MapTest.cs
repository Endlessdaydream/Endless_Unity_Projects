using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTest : MonoBehaviour
{
    int W = 30;         // 课程要求：W、H不能相等，否则一些BUG测不出来
    int H = 20;
    GameObject[,] mapBlocks;    // 组成地图的所有小方块物体
    GameObject[,] pathBlocks;   // 表示路径的所有的小方块物体

    public GameObject prefabBlock;
    public GameObject prefabPath;

    int[,] map;     // 地图二维数组

    void Start()
    {
        // 初始化数组
        map = new int[H, W];
        mapBlocks = new GameObject[H, W];
        pathBlocks = new GameObject[H, W];

        // 创建所有小方块
        CreateBlocks();
    }

    void CreateBlocks()
    {
        for (int i=0; i<H; i++)
        {
            for (int j=0; j<W; j++)
            {
                mapBlocks[i, j] = Instantiate(prefabBlock, new Vector3(j, 0, i), Quaternion.identity);
                pathBlocks[i, j] = Instantiate(prefabPath, new Vector3(j, 0, i), Quaternion.identity);
            }
        }
    }

    void RefreshMap(int[,] map)
    {
        // 刷新Map
        for (int i = 0; i < H; i++)
        {
            for (int j = 0; j < W; j++)
            {
                if (map[i,j] == 0)
                {
                    mapBlocks[i, j].SetActive(false);
                }
                else if (map[i,j] == 1)
                {
                    mapBlocks[i, j].SetActive(true);
                    mapBlocks[i, j].GetComponent<MeshRenderer>().material.color = Color.white;
                }
                else if (map[i, j] == 2)
                {
                    mapBlocks[i, j].SetActive(true);
                    mapBlocks[i, j].GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }
        }
    }

    void RefreshPathMap(int[,] pathMap)
    {
        // 刷新Map
        for (int i = 0; i < H; i++)
        {
            for (int j = 0; j < W; j++)
            {
                if (pathMap[i, j] == 0)
                {
                    pathBlocks[i, j].SetActive(false);
                }
                else
                {
                    pathBlocks[i, j].SetActive(true);
                }
            }
        }
    }

    void RandomTest(int[,] map, int[,] pathMap)
    {
        for (int i = 0; i < H; i++)
        {
            for (int j = 0; j < W; j++)
            {
                map[i, j] = Random.Range(0, 3);
                pathMap[i, j] = Random.Range(0, 5);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int[,] pathMap = new int[H, W];
            RandomTest(map, pathMap);
            RefreshMap(map);
            RefreshPathMap(pathMap);
        }
    }
}
