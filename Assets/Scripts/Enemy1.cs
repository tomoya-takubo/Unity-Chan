using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    Rigidbody2D rigidbody2D;
    //public int speed;
    private int speed = -3;

    public GameObject explosion;

    public GameObject item;

    public int attackPoint;
    //public Life life;
    //Enemyオブジェクトもプレハブ化するため、Lifeスクリプト取得は
    //Tag捜索メソッドで特定して取得する方法で行う
    private Life life;

    //メインカメラのタグ名 constは定数（絶対に変わらない値）
    private const string MAIN_CAMERA_TAG_NAME = "MainCamera";
    //カメラに写っているかの判定
    //private bool _isRendered = false;
    public bool _isRendered = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

        life = GameObject.FindGameObjectWithTag("HP").GetComponent<Life>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isRendered)
        {
            rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);
        }

        if(gameObject.transform.position.y < Camera.main.transform.position.y - 8
            || gameObject.transform.position.x < Camera.main.transform.position.x - 10)
        {
            Destroy(gameObject);
        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (_isRendered)
        {
            
            if (col.gameObject.tag == "Bullet")
            {
                Destroy(gameObject);
                Instantiate(explosion, transform.position, transform.rotation);

                //４分の１の確率で回復アイテムを落とすように設定する
                if (Random.Range(0, 2) == 0)
                {
                    Instantiate(item, transform.position, transform.rotation);
                }

            }

        }

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "UnityChan")
        {
            //LifeスクリプトのLifeDownメソッドを実行
            life.LifeDown(attackPoint);
        }
    }

    //Rendererがカメラに写っている間に呼ばれ続ける
    void OnWillRenderObject()
    {
        //Debug.Log(Camera.current.tag);

        //メインカメラに映ったときだけ、_isRendereredをtrue
        if (Camera.current.tag == MAIN_CAMERA_TAG_NAME)
        {
            _isRendered = true;
        }
    }
}
