using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTest4 : MonoBehaviour
{
    int W = 30;         // 课程要求：W、H不能相等，否则一些BUG测不出来
    int H = 20;
    GameObject[,] mapBlocks;    // 组成地图的所有小方块物体
    GameObject[,] pathBlocks;   // 表示路径的所有的小方块物体

    public GameObject prefabBlock;
    public GameObject prefabPath;

    int[,] map;     // 地图二维数组

    int[,] pathMap; // 记录步数的二维数组
    List<Pos> queue;        // 等待探索的队列
    Pos startPos;   // 起点坐标
    Pos endPos;     // 终点坐标

    void Start()
    {
        // 初始化数组
        map = new int[H, W];
        pathMap = new int[H, W];
        mapBlocks = new GameObject[H, W];
        pathBlocks = new GameObject[H, W];
        queue = new List<Pos>();

        // 创建所有小方块
        CreateBlocks();

        // 创建一堵墙
        for (int i=3; i<15; i++)
        {
            map[7, i] = 1;
        }
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
                if (pathMap[i, j] == int.MaxValue)
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

    IEnumerator BFS()
    {
        // 初始化数据
        startPos = new Pos(3, 3);
        endPos = new Pos(10, 10);

        // 将初始步数设置为很大的值 int.MaxValue
        for (int i=0; i<pathMap.GetLength(0); i++)
        {
            for (int j = 0; j < pathMap.GetLength(1); j++)
            {
                pathMap[i, j] = int.MaxValue;
            }
        }
        pathMap[startPos.y, startPos.x] = 0;

        queue.Add(startPos);    // 把起点坐标加入队列


        // 开始搜索

        for (int i=0; i<1000; i++)        // 先少循环几次，看看效果
        {
            // 1. 从队列中取一个点cur
            Pos cur = queue[0];
            queue.RemoveAt(0);

            // 2. 查找cur的上下左右四个相邻的点
            Pos next;

            // 上
            next = new Pos(cur.x, cur.y - 1);
            if (next.y >= 0 && map[next.y, next.x] == 0)
            {
                if (pathMap[next.y, next.x] > pathMap[cur.y, cur.x] + 1)
                {
                    queue.Add(next);
                    pathMap[next.y, next.x] = pathMap[cur.y, cur.x] + 1;
                }
            }

            if (next.Equals(endPos))
            {
                Debug.Log("找到终点");
                yield break;
            }

            // 下
            next = new Pos(cur.x, cur.y + 1);
            if (next.y < H && map[next.y, next.x] == 0)
            {
                if (pathMap[next.y, next.x] > pathMap[cur.y, cur.x] + 1)
                {
                    queue.Add(next);
                    pathMap[next.y, next.x] = pathMap[cur.y, cur.x] + 1;
                }
            }

            if (next.Equals(endPos))
            {
                Debug.Log("找到终点");
                yield break;
            }

            // 左
            next = new Pos(cur.x - 1, cur.y);
            if (next.x >= 0 && map[next.y, next.x] == 0)
            {
                if (pathMap[next.y, next.x] > pathMap[cur.y, cur.x] + 1)
                {
                    queue.Add(next);
                    pathMap[next.y, next.x] = pathMap[cur.y, cur.x] + 1;
                }
            }

            if (next.Equals(endPos))
            {
                Debug.Log("找到终点");
                yield break;
            }

            // 右
            next = new Pos(cur.x + 1, cur.y);
            if (next.x < W && map[next.y, next.x] == 0)
            {
                if (pathMap[next.y, next.x] > pathMap[cur.y, cur.x] + 1)
                {
                    queue.Add(next);
                    pathMap[next.y, next.x] = pathMap[cur.y, cur.x] + 1;
                }
            }

            if (next.Equals(endPos))
            {
                Debug.Log("找到终点");
                yield break;
            }

            // 刷新显示
            Debug.Log("队列长度：" + queue.Count);
            RefreshMap(map);
            RefreshPathMap(pathMap);

            yield return new WaitForSeconds(0.01f);
        }

    }

    void BFSShowPath()
    {
        // 从终点开始，反推
        Pos pos = endPos;
        int step = pathMap[pos.y, pos.x];       // 终点的步数
        while (!pos.Equals(startPos))
        {
            if (pos.y > 0 && pathMap[pos.y - 1, pos.x] == step - 1)
            {
                pos = new Pos(pos.x, pos.y - 1);
                step -= 1;
            }
            else if (pos.y < H - 1 && pathMap[pos.y + 1, pos.x] == step - 1)
            {
                pos = new Pos(pos.x, pos.y + 1);
                step -= 1;
            }
            else if (pos.x > 0 && pathMap[pos.y, pos.x - 1] == step - 1)
            {
                pos = new Pos(pos.x - 1, pos.y);
                step -= 1;
            }
            else if (pos.x < W - 1 && pathMap[pos.y, pos.x + 1] == step - 1)
            {
                pos = new Pos(pos.x + 1, pos.y);
                step -= 1;
            }
            else
            {
                Debug.Log("!!!!!错误!!!!!!!!!");
                return;
            }
            var go = pathBlocks[pos.y, pos.x];
            var render = go.GetComponent<MeshRenderer>();
            render.material.color = Color.blue;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(BFS());
        }

        // 开始计算路线
        BFSShowPath();
    }
}
