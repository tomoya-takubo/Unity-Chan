using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int healPoint = 20;


    //public Life life;
    //プレハブ化するとInspector上からHPにアタッチされたLifeスクリプトをを
    //指定できなくなるため、privateとしてスクリプト内で設定する
    private Life life;

    void Start()
    {
        //HPタグの付いているオブジェクトのLifeスクリプトを取得
        life = GameObject.FindGameObjectWithTag("HP").GetComponent<Life>();

    }
    void OnCollisionEnter2D(Collision2D col)
    {
        //ユニティちゃんと衝突したとき
        if(col.gameObject.tag == "UnityChan")
        {
            //LifeUpメソッドを呼び出す
            life.LifeUp(healPoint);
            //アイテムを削除する
            Destroy(gameObject);
        }
    }
}
