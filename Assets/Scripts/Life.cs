using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Life : MonoBehaviour
{
    RectTransform rt;

    public GameObject unityChan;    //ユニティちゃん情報格納用
    public GameObject explosion;    //爆発アニメーション
    public Text gameOverText;       //ゲームオーバーの文字
    private bool gameOver = false;  //ゲームオーバー判定

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    void Update()
    {
        if(rt.sizeDelta.y <= 0) //ライフゲージが0以下になったら
        {
            if(gameOver == false)
            {
                //ユニティちゃん消滅アニメーション
                Instantiate(explosion, unityChan.transform.position + new Vector3(0, 1, 0), unityChan.transform.rotation);
            }

            GameOver();

        }

        if (gameOver)
        {
            gameOverText.enabled = true;

            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene("Title");
            }
        }
    }

    public void LifeDown(int ap)
    {
        //RectTranformのサイズを取得し、マイナスにする
        rt.sizeDelta -= new Vector2(0, ap);
    }

    public void LifeUp(int hp)  //hp:healpoint（回復体力）
    {
        //RectTransformのサイズを取得し、プラスする
        rt.sizeDelta += new Vector2(0, hp);
        //最大値を超えたら、最大値で上書きする
        if(rt.sizeDelta.y > 240f)
        {
            //rt.sizeDelta = new Vector2(51f, 240f);
            rt.sizeDelta = new Vector2(37.5f, 234.1f);
        }
    }

    public void GameOver()
    {
        gameOver = true;
        Destroy(unityChan);
        //Debug.Log("落ちてしまった！");

    }
}
