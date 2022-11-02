using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
        Destroy(collision.gameObject);
    }

    public static GameMode Instance { get; private set; }
    public Transform bird;
    PlayerBirds playerBird; //bird身上的脚本组件
    public Transform center;

    public float maxDist = 3;
    public float maxForce = 700;

    //尝试次数，小鸟几条命
    public int playerLives = 4;

    //剩余敌人数量
    public int enemyNum = 0;
    LineRenderer[] lines;

    FollowCam cam;

    public Transform prefabPoint;
    Transform[] points;

    bool isBirdFlying = false;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        bird.position = center.position;
        playerBird = bird.GetComponent<PlayerBirds>();

        Rigidbody2D rigid = bird.GetComponent<Rigidbody2D>();
        rigid.isKinematic = true;

        lines = center.parent.GetComponentsInChildren<LineRenderer>();
        lines[0].SetPosition(0, bird.position);
        lines[1].SetPosition(0, bird.position);

        cam = Camera.main.GetComponent<FollowCam>();

        //统计敌人数量
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemyNum = enemies.Length;
        //绘制抛物线的点
        points = new Transform[20];
        for(int i = 0; i < points.Length; i++)
        {
            points[i] = Instantiate(prefabPoint);
            points[i].gameObject.SetActive(false);
        }

    }
    public void BeginDrag()
    {
        if (isBirdFlying)
        {
            return;
        }
        //显示所有的辅助点
        ShowPoints(true);
    }
    public void Drag(Vector3 mousePos)
    {
        if (isBirdFlying)
        {
            return;
        }
        //屏幕坐标转世界坐标
        Vector3 pos = Camera.main.ScreenToWorldPoint(mousePos);
        pos.z = 0;

        //小鸟离中心点的距离
        Vector3 v = pos - center.position;

        if (v.magnitude > maxDist)
        {
            v = v.normalized * maxDist;
        }
        bird.position = center.position + v;

        //设置皮筋
        lines[0].SetPosition(0, bird.position);
        lines[1].SetPosition(0, bird.position);

        //绘制辅助线
        Rigidbody2D rigid = bird.GetComponent<Rigidbody2D>();
        float f = v.magnitude / maxDist * maxForce;
        float v0m = f * Time.fixedDeltaTime / rigid.mass;
        Vector2 v0 = -v.normalized * v0m;

        //过了时间t，小鸟位置
        //p = bird.position+(v0 * t +0.5f * Physics2D.gravity * t * t);
        float t = 0;
        for (int i = 0; i < points.Length; i++)
        {
            t += 0.2f;
            Vector2 p = (Vector2)bird.position + (v0 * t + 0.5f * Physics2D.gravity * t * t);
            points[i].position = p;
        }
    }
    public void EndDrag()
    {
        if (isBirdFlying)
        {
            return;
        }
        //判断小鸟和中心点距离，准备弹出
        Vector3 v = bird.position - center.position;
        if(v.magnitude < 0.01f)
        {
            return;
        }
        //施加力,弹出小鸟
        Rigidbody2D rigid = bird.GetComponent<Rigidbody2D>();
        rigid.isKinematic = false;
        float f = v.magnitude / maxDist * maxForce;
        rigid.AddForce(f * -v.normalized);
        playerBird.StartFly();

        cam.isFollow = true;
        isBirdFlying = true;

        //隐藏所有的辅助点
        ShowPoints(false);

        //皮筋归位
        lines[0].SetPosition(0, center.position);
        lines[1].SetPosition(0, center.position);
    }
    
    public void OnPlayerBirdDie()
    {
        playerLives--;
        
        if (playerLives > 0)
        {
            //小鸟复位
            bird.position = center.position;
            isBirdFlying = false;
            cam.isFollow = false;
            cam.Follow();
            playerBird.ResetBird(); 
        }
        else
        {
            //等待几秒，然后判断Game Over
            Invoke("DelayGameOver", 2);
        }
    }

    void DelayGameOver()
    {
        if (enemyNum <= 0)
        {
            //游戏已经成功
            return;
        }
        Debug.Log("you loss");
    }
    public void ShowPoints(bool visible)
    {
        foreach(var p in points)
        {
            p.gameObject.SetActive(visible);
        }
    }
    public void OnPigDie()
    {
        enemyNum--;
        if (enemyNum <= 0)
        {
            Debug.Log("You Win!");
        }
    }
}
