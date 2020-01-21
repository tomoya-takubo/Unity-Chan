using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed = 4f;    //歩くスピード
    //public float speed;    //歩くスピード

    //***ジャンプ変数開始***//
    public float jumpPower = 700;   //ジャンプ力
    public LayerMask groundLayer;   //Linecastで判定するLayer
    //***ジャンプ変数終了***//

    public GameObject mainCamera;

    //***弾生成はじめ***//
    public GameObject bullet;
    //***弾生成おわり***//

    public Life life;

    private Rigidbody2D rigidbody2D;
    private Animator anim;

    //***ジャンプ変数開始***//
    private bool isGrounded;    //着地判定
    //***ジャンプ変数終了***//

    //***無敵時間開始***//
    private Renderer renderer;
    //***無敵時間終了***//

    private bool gameClear = false;
    public Text gameClearText;

    // Start is called before the first frame update
    void Start()
    {
        //各コンポーネントをキャッシュしておく（紐づけ）
        anim = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        renderer = GetComponent<Renderer>();

    }

    //***ジャンプ変数開始***//

    void Update()
    {
        //Linecastでユニティちゃんの足元に地面があるか判定
        isGrounded = Physics2D.Linecast(
            transform.position + transform.up * 1,
            transform.position - transform.up * 0.05f,
            groundLayer);

        if (!gameClear)
        {
            //スペースキーを押し、
            if (Input.GetKeyDown("space"))
            {
                //着地していた時
                if (isGrounded)
                {
                    //Dashアニメーションを止めて、Jumpアニメーションを実行
                    anim.SetBool("Dash", false);
                    anim.SetTrigger("Jump");
                    //着地判定をfalse
                    isGrounded = false;
                    //AddForceにて上方向へ力を加える
                    rigidbody2D.AddForce(Vector2.up * jumpPower);
                }
            }

        }
              

        //上下の移動速度を取得
        float velY = rigidbody2D.velocity.y;
        //移動速度が0.1より大きければ上昇
        bool isJumping = velY > 0.1f ? true : false;
        //移動速度が-0.1より小さければ下降
        bool isFalling = velY < -0.1f ? true : false;
        //結果をアニメータービューの変数へ反映する
        anim.SetBool("isJumping", isJumping);
        anim.SetBool("isFalling", isFalling);

        if (!gameClear)
        {
            //***弾生成開始***//
            if (Input.GetKeyDown("left ctrl"))
            {
                anim.SetTrigger("Shot");
                Instantiate(bullet, transform.position + new Vector3(0f, 1.2f, 0f), transform.rotation);
                //Instantiate(bullet, transform.position + new Vector3(3f, 1.2f, 0f), transform.rotation);
            }
            //***弾生成終端***//

            if (gameObject.transform.position.y < Camera.main.transform.position.y - 8)
            {
                life.GameOver();
            }

        }

    }

    //***ジャンプ変数終了***//

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!gameClear)
        {
            //左キー:-1、右キー：1
            float x = Input.GetAxisRaw("Horizontal");   //-1, 0, 1までの数字を返してくれる
                                                        //float x = Input.GetAxis("Horizontal");   //-1～1までの数字を返してくれる

            //左か右を入力したら
            if (x != 0)
            {
                //入力方向へ移動
                rigidbody2D.velocity = new Vector2(x * speed, rigidbody2D.velocity.y);
                //localScale.xを-1にすると画像が反転する
                Vector2 temp = transform.localScale;
                temp.x = x;
                transform.localScale = temp;

                //Wait→Dash
                anim.SetBool("Dash", true);

                //画面中央柄左に4異動した位置をユニティちゃんが超えたら
                if (transform.position.x > mainCamera.transform.position.x - 4)
                {
                    //カメラの位置を取得
                    Vector3 cameraPos = mainCamera.transform.position;
                    //ユニティっちゃんの位置から右に４移動した位置を画面中央にする
                    cameraPos.x = transform.position.x + 4;
                    mainCamera.transform.position = cameraPos;
                }

                //カメラ表示領域の左下をワールド座標に変換
                Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
                //カメラ表示領域の右上をワールド座標に変換
                Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
                //ユニティちゃんのポジションを取得
                Vector2 pos = transform.position;
                //ユニティちゃんんおx座標の移動範囲をClampメソッドで制限
                pos.x = Mathf.Clamp(pos.x, min.x, max.x);
                transform.position = pos;

                //左も右も入力していなかったら
            }
            else
            {
                //横移動の速度を0にしてピタッと止まるようにする
                rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
                //Dash→Wait
                anim.SetBool("Dash", false);
            }
        } else
        {
            gameClearText.enabled = true;
            anim.SetBool("Dash", true);
            rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);
            Invoke("CallTitle", 5);
        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "ClearZone")
        {
            //Debug.Log("GameClear!");
            gameClear = true;
        }
    }

    void CallTitle()
    {
        SceneManager.LoadScene("Title");
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //Enemyとぶつかったときにコルーチンを実行
        if (col.gameObject.tag == "Enemy")
        {
            StartCoroutine("Damage");
        }
    }

    IEnumerator Damage()
    {
        //レイヤーをPlayerDamageに変更
        gameObject.layer = LayerMask.NameToLayer("PlayerDamage");
        //while分を10回ループ
        int count = 10;
        while (count > 0)
        {
            //透明にする
            renderer.material.color = new Color(1, 1, 1, 0);
            //0.05秒待つ
            yield return new WaitForSeconds(0.05f);
            //元に戻す
            renderer.material.color = new Color(1, 1, 1, 1);
            //0.05秒待つ
            yield return new WaitForSeconds(0.05f);
            count--;
        }

        //レイヤーをPlayerに戻す
        gameObject.layer = LayerMask.NameToLayer("Player");

    }
}