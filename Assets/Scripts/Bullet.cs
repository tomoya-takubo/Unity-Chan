using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject player;
    private int speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        //Unityちゃんオブジェクトを取得
        player = GameObject.FindWithTag("UnityChan");
        //rigidbody2Dコンポーネントを取得
        Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
        //Unityちゃんの向いている向きに弾を飛ばす
        rigidbody2D.velocity = new Vector2(speed * player.transform.localScale.x, rigidbody2D.velocity.y);
        Debug.Log(rigidbody2D.velocity.y);
        //画像の向きをUnityちゃんに合わせる
        Vector2 temp = transform.localScale;
        temp.x = player.transform.localScale.x;
        transform.localScale = temp;
        //5秒後に消滅
        Destroy(gameObject, 5);
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.tag != "UnityChan")
        //if (col.gameObject.tag == "Enemy")
        //if (col.tag == "Enemy")   .gameObject省略可能   
        {
            Destroy(gameObject);
        }
        

        //Destroy(gameObject);

    }
}
