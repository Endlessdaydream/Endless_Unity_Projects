using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

// Pos代替Vector2，代表位置。整数
public struct Pos
{
    public int x;
    public int y;

    public Pos(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static float AStarDistance(Pos p1, Pos p2)
    {
        float d1 = Mathf.Abs(p1.x - p2.x);
        float d2 = Mathf.Abs(p1.y - p2.y);
        return d1 + d2;
    }

    public override string ToString()
    {
        return string.Format("[{0}_{1}]", x, y);
    }
}

public class AScore : IComparable<AScore>
{
    // G是从起点出发的步数
    public float G = 0;
    // H是估算的离终点距离
    public float H = 0;

    public float F
    {
        get { return G + H; }
    }

    public Pos parent;

    public AScore(float g, float h)
    {
        G = g;
        H = h;
    }

    // 比较器，方便列表排序
    public int CompareTo(AScore a2)
    {
        return F.CompareTo(a2.F);
    }
}

public class GameMap : MonoBehaviour {

    int W = 30;
    int H = 20;

    int[,] map;
    public GameObject prefab_wall;
    public GameObject prefab_path;

    GameObject[,] mapBlocks;        // 事先创建好所有方块，和map数组一一对应
    GameObject[,] pathBlocks;        // 事先创建好所有方块，和map数组一一对应

    public enum SearchMethod
    {
        BFS,
        DFS,
        AStar,
    }
    public SearchMethod searchMethod = SearchMethod.BFS;

    GameObject pathParent;

    const int START = 8;
    const int END = 9;
    const int WALL = 1;

    enum GameState
    {
        SetBeginPoint,
        SetEndPoint,
        StartCalculation,
        Calculation,
        ShowPath,
        Finish,
    }

    GameState gameState = GameState.SetBeginPoint;

    public void ReadMapFile()
    {
        string path = Application.dataPath + "//" + "map.txt";
        if (!File.Exists(path))
        {
            return;
        }

        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            StreamReader read = new StreamReader(fs, Encoding.UTF8);
            string strReadline = "";
            int y = 0;

            // 跳过第一行
            read.ReadLine();
            strReadline = read.ReadLine();

            while (strReadline!=null && y<H)
            {
                for (int x=0; x<W && x<strReadline.Length; ++x)
                {
                    int t;
                    switch(strReadline[x])
                    {
                        case '1':
                            t = 1;
                            break;
                        case '8':
                            t = 8;
                            break;
                        case '9':
                            t = 9;
                            break;
                        default:
                            t = 0;
                            break;
                    }
    //                Debug.Log("x, y"+ x +" " + y);
                    map[y, x] = t;
                }
                y += 1;
                strReadline = read.ReadLine();
            }
            read.Dispose();//文件流释放  
        }
        // 因为用了using，所以遇到异常或执行结束，会自动关闭文件
    }

    void InitMap()
    {
        for (int i=0; i<mapBlocks.GetLength(0); i++)
        {
            for (int j=0; j<mapBlocks.GetLength(1); j++)
            {
                mapBlocks[i, j] = Instantiate(prefab_wall, new Vector3(j, 0.5f, i), Quaternion.identity);
                mapBlocks[i, j].SetActive(false);
            }
        }

        for (int i = 0; i < pathBlocks.GetLength(0); i++)
        {
            for (int j = 0; j < pathBlocks.GetLength(1); j++)
            {
                pathBlocks[i, j] = Instantiate(prefab_path, new Vector3(j, 0.25f, i), Quaternion.identity);
                pathBlocks[i, j].SetActive(false);
            }
        }
    }

	void Start () {
        pathParent = GameObject.Find("PathParent");
        map = new int[H, W];
        mapBlocks = new GameObject[map.GetLength(0), map.GetLength(1)];
        pathBlocks = new GameObject[map.GetLength(0), map.GetLength(1)];

        ReadMapFile();

        InitMap();
        RefreshMap();
    }

    void RefreshMap()
    {
        for (int i = 0; i < mapBlocks.GetLength(0); i++)
        {
            for (int j = 0; j < mapBlocks.GetLength(1); j++)
            {
                GameObject b = mapBlocks[i, j].gameObject;
                switch (map[i,j])
                {
                    case 0:
                        b.SetActive(false);
                        break;

                    case WALL:
                        {
                            b.SetActive(true);
                            Material m = mapBlocks[i, j].GetComponent<MeshRenderer>().material;
                            m.color = Color.white;
                        }
                        break;

                    case START:
                    case END:
                        {
                            b.SetActive(true);
                            Material m = mapBlocks[i, j].GetComponent<MeshRenderer>().material;
                            m.color = Color.red;
                        }
                        break;
                }
            }
        }
    }

    Pos startPos;
    Pos endPos;

    bool SetPoint(int n)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // We need to actually hit an object
            RaycastHit hitt = new RaycastHit();
            Physics.Raycast(ray, out hitt, 100);
            if (hitt.transform != null && hitt.transform.name == "Ground")
            {
                int x = (int)hitt.point.x;
                int y = (int)hitt.point.z;

                map[y, x] = n;
                if (n == START)
                {
                    startPos = new Pos( x, y );
                }
                else if (n == END)
                {
                    endPos = new Pos( x, y );
                }
                return true;
            }
        }
        return false;
    }

    delegate bool Func(Pos cur, int ox, int oy);

    int cur_depth = 0;
    int[,] steps;       // 步数数组

    // Breadth First Search, 广度优先搜索
    IEnumerator BFS()
    {
        steps = new int[map.GetLength(0),map.GetLength(1)];

        // map_search和map一样大小，每个元素的值：int.MaxValue代表不可通过或者未探索，其他值代表移动的步数

        for (int i=0; i<H; ++i)
        {
            for (int j=0; j<W; ++j)
            {
                steps[i, j] = int.MaxValue;
            }
        }

        Queue<Pos> queue = new Queue<Pos>();
        //List<Pos> queue = new List<Pos>();

        Func func = (Pos cur, int ox, int oy) =>
        {
            if (map[cur.y + oy, cur.x + ox] == END)
            {
                steps[cur.y + oy, cur.x + ox] = steps[cur.y, cur.x] + 1;
                gameState = GameState.ShowPath;
                return true;
            }
            if (map[cur.y + oy, cur.x + ox] == 0)
            {
                // 核心，最重要的判断
                if (steps[cur.y + oy, cur.x + ox] > steps[cur.y, cur.x] + 1)
                {
                    steps[cur.y + oy, cur.x + ox] = steps[cur.y, cur.x] + 1;
                    queue.Enqueue(new Pos(cur.x+ox, cur.y+oy));
                }
            }
            return false;
        };

        steps[startPos.y, startPos.x] = 0;
        queue.Enqueue(startPos);

        while (queue.Count > 0)
        {
            Debug.Log("队列长度"+queue.Count);
            Pos cur = queue.Dequeue();

            // 上
            if (cur.y > 0 )
            {
                if (func(cur, 0, -1)) { break; }
            }
            // 下
            if (cur.y<H-1)
            {
                if (func(cur, 0, 1)) { break; }
            }
            // 左
            if (cur.x>0)
            {
                if (func(cur, -1, 0)) { break; }
            }
            // 右
            if (cur.x<W-1)
            {
                if (func(cur, 1, 0)) { break; }
            }

            if (steps[cur.y, cur.x] > cur_depth)
            {
               cur_depth = steps[cur.y, cur.x];
               RefreshPath(steps);
               yield return new WaitForSeconds(0.1f);
            }
        }
        RefreshPath(steps);
        Debug.Log("无法到达目标点");
    }

    IEnumerator DFS()
    {
        steps = new int[map.GetLength(0),map.GetLength(1)];

        // map_search和map一样大小，每个元素的值：int.MaxValue代表不可通过或者未探索，其他值代表移动的步数
        for (int i=0; i<H; ++i)
        {
            for (int j=0; j<W; ++j)
            {
                steps[i, j] = int.MaxValue;
            }
        }

        List<Pos> queue = new List<Pos>();

        Func func = (Pos cur, int ox, int oy) =>
        {
            if (map[cur.y + oy, cur.x + ox] == END)
            {
                steps[cur.y + oy, cur.x + ox] = steps[cur.y, cur.x] + 1;
                gameState = GameState.ShowPath;
                return true;
            }
            if (map[cur.y + oy, cur.x + ox] == 0)
            {
                // 核心，最重要的判断
                if (steps[cur.y + oy, cur.x + ox] > steps[cur.y, cur.x] + 1)
                {
                    steps[cur.y + oy, cur.x + ox] = steps[cur.y, cur.x] + 1;
                    queue.Add(new Pos(cur.x + ox, cur.y + oy));
                }
            }
            return false;
        };

        steps[startPos.y, startPos.x] = 0;
        queue.Add(startPos);

        while (queue.Count > 0)
        {
            Debug.Log("队列长度"+queue.Count);
            queue.Sort((Pos a, Pos b) =>
            {
                // 折线距离，也可以用几何距离代替
                float da = Mathf.Abs(endPos.x - a.x) + Mathf.Abs(endPos.y - a.y);
                float db = Mathf.Abs(endPos.x - b.x) + Mathf.Abs(endPos.y - b.y);
                return da.CompareTo(db);
            });
            Pos cur = queue[0];
            queue.RemoveAt(0);
            //Pos cur = queue[queue.Count - 1];
            //queue.RemoveAt(queue.Count - 1);

            // 上
            if (cur.y > 0 )
            {
                if (func(cur, 0, -1)) { break; }
            }
            // 下
            if (cur.y<H-1)
            {
                if (func(cur, 0, 1)) { break; }
            }
            // 左
            if (cur.x>0)
            {
                if (func(cur, -1, 0)) { break; }
            }
            // 右
            if (cur.x<W-1)
            {
                if (func(cur, 1, 0)) { break; }
            }
            RefreshPath(steps);
            yield return new WaitForSeconds(0.01f);
        }
        RefreshPath(steps);

        yield return null;
    }

    AScore[,] astar_search;
    IEnumerator AStar()
    {
        astar_search = new AScore[map.GetLength(0), map.GetLength(1)];

        List<Pos> list = new List<Pos>();

        astar_search[startPos.y, startPos.x] = new AScore(0,0);
        list.Add(startPos);

        // 每一个点的处理函数
        Func func = (Pos cur, int ox, int oy) =>
        {
            var o_score = astar_search[cur.y + oy, cur.x + ox];

            var cur_score = astar_search[cur.y, cur.x];
            Pos o_pos = new Pos(cur.x + ox, cur.y + oy);
            if (map[cur.y + oy, cur.x + ox] == END)
            {
                var a = new AScore(cur_score.G + 1, 0);
                a.parent = cur;
                astar_search[cur.y + oy, cur.x + ox] = a;
                Debug.Log("寻路完成");
                return true;
            }
            if (map[cur.y + oy, cur.x + ox] == 0)
            {
                if (o_score==null)
                {
                    var a = new AScore(cur_score.G + 1, Pos.AStarDistance(o_pos, endPos));
                    a.parent = cur;
                    astar_search[cur.y + oy, cur.x + ox] = a;
                    list.Add(o_pos);
                }
                else if (o_score.G > cur_score.G+1)     // 很重要的判断，小的步数覆盖大的步数
                {
                    o_score.G = cur_score.G + 1;
                    o_score.parent = cur;
                    //o_score.closed = false;
                    if (!list.Contains(o_pos))
                    {
                        list.Add(o_pos);
                    }
                }
            }
            return false;
        };


        while (list.Count > 0)
        {
            list.Sort((Pos p1, Pos p2) =>
            {
                AScore a1 = astar_search[p1.y, p1.x];
                AScore a2 = astar_search[p2.y, p2.x];
                return a1.F.CompareTo(a2.F);
            });
            Pos cur = list[0];
            list.RemoveAt(0);

            // 上
            if (cur.y > 0)
            {
                if (func(cur, 0, -1)) { break; }
            }
            // 下
            if (cur.y < H - 1)
            {
                if (func(cur, 0, 1)) { break; }
            }
            // 左
            if (cur.x > 0)
            {
                if (func(cur, -1, 0)) { break; }
            }
            // 右
            if (cur.x < W - 1)
            {
                if (func(cur, 1, 0)) { break; }
            }

            int[,] temp_map = new int[map.GetLength(0), map.GetLength(1)];
            for (int i=0; i<H; ++i)
            {
                for (int j=0; j<W; ++j)
                {
                    temp_map[i, j] = int.MaxValue;
                    if (astar_search[i,j] != null)
                    {
                        temp_map[i, j] = (int)astar_search[i, j].F;
                    }
                }
            }
            RefreshPath(temp_map);
            yield return 0;
        }
        gameState = GameState.ShowPath;
        yield return null;
    }


    void BFSShowPath()
    {
        Pos pos = endPos;
        int step = steps[pos.y, pos.x];
        while (!pos.Equals(startPos))
        {
            if (pos.y > 0 && steps[pos.y-1, pos.x]==step-1)
            {
                pos = new Pos(pos.x, pos.y - 1);
                step -= 1;
            }
            else if (pos.y < H - 1 && steps[pos.y+1, pos.x]==step-1)
            {
                pos = new Pos(pos.x, pos.y + 1);
                step -= 1;
            }
            else if (pos.x > 0 && steps[pos.y, pos.x-1]==step-1)
            {
                pos = new Pos(pos.x-1, pos.y);
                step -= 1;
            }
            else if (pos.x < W - 1 && steps[pos.y, pos.x+1]==step-1)
            {
                pos = new Pos(pos.x+1, pos.y);
                step -= 1;
            }
            else
            {
                Debug.Log("!!!!!错误!!!!!!!!!");
                Debug.Log(step);
                Debug.Log(new Vector2(pos.x, pos.y));
            }
            var go = pathBlocks[pos.y, pos.x];
            var render = go.GetComponent<MeshRenderer>();
            render.material.color = Color.blue;
        }
    }

    void AStarShowPath()
    {
        Pos pos = endPos;
        while (!pos.Equals(startPos))
        {
            var go = pathBlocks[pos.y, pos.x];
            var render = go.GetComponent<MeshRenderer>();
            render.material.color = Color.blue;

            pos = astar_search[pos.y, pos.x].parent;
        }
    }

    // Update is called once per frame
    void Update () {
        switch(gameState)
        {
            case GameState.SetBeginPoint:
                if (SetPoint(START))
                {
                    RefreshMap();
                    gameState = GameState.SetEndPoint;
                }
                break;
            case GameState.SetEndPoint:
                if (SetPoint(END))
                {
                    RefreshMap();
                    gameState = GameState.StartCalculation;
                }
                break;
            case GameState.StartCalculation:
                if (searchMethod == SearchMethod.BFS)
                {
                    StartCoroutine(BFS());
                }
                else if (searchMethod == SearchMethod.DFS)
                {
                    StartCoroutine(DFS());
                }
                else if (searchMethod == SearchMethod.AStar)
                {
                    StartCoroutine(AStar());
                }
                gameState = GameState.Calculation;
                break;
            case GameState.Calculation:
                break;
            case GameState.ShowPath:
                if (searchMethod == SearchMethod.BFS)
                {
                    BFSShowPath();
                    gameState = GameState.Finish;
                }
                else if (searchMethod == SearchMethod.DFS)
                {
                    BFSShowPath();
                    gameState = GameState.Finish;
                }
                else if (searchMethod == SearchMethod.AStar)
                {
                    AStarShowPath();
                    gameState = GameState.Finish;
                }
                break;
            case GameState.Finish:
                break;
        }
	}

    void RefreshPath(int[,] temp_map)
    {
        for (int i = 0; i < H; i++)
        {
            for (int j = 0; j < W; j++)
            {
                if (temp_map[i,j] == int.MaxValue)
                {
                    pathBlocks[i, j].SetActive(false);
                }
                else
                {
                    TextMesh[] texts = pathBlocks[i, j].GetComponentsInChildren<TextMesh>();
                    pathBlocks[i, j].SetActive(true);
                    if (searchMethod == SearchMethod.BFS || searchMethod == SearchMethod.DFS)
                    {
                        texts[0].text = steps[i, j].ToString();
                        texts[1].text = "";
                    }
                    else if (searchMethod == SearchMethod.AStar)
                    {
                        texts[0].text = astar_search[i, j].G.ToString();
                        texts[1].text = astar_search[i, j].F.ToString();
                    }
                }
            }
        }
    }
}
