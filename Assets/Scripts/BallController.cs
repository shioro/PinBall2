using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    //ボールが見える可能性のあるz軸の最大値
    private float visiblePosZ = -6.5f;

    //ゲームオーバーを表示するテキスト
    private GameObject gamenOverText;

    //スコアに関する定義
    //スコアを表示するテキスト
    private GameObject scoreText;

    private int score = 0;

    private int smallStarScore = 5;
    private int largeStarScore = 15;
    private int smallCloudScore = 10;
    private int largeCloudScore = 20;

    // Start is called before the first frame update
    void Start()
    {
        //シーン中のGameOverTextオブジェクトを取得
        this.gamenOverText = GameObject.Find("GameOverText");

        //シーン中のScoreTextオブジェクトを取得
        this.scoreText = GameObject.Find("ScoreText");
    }

    // Update is called once per frame
    void Update()
    {
        //ボールが画面外に出た場合
        if(this.transform.position.z < this.visiblePosZ)
        {
            //ゲームオーバーを表示
            this.gamenOverText.GetComponent<Text>().text = "Game Over";
        }
    }

    //衝突時に呼ばれる関数
    private void OnCollisionEnter(Collision other)
    {
        //タグによって点数を変える
        if (other.gameObject.tag == "SmallStarTag")
        {
            score += smallStarScore;
        }
        else if (other.gameObject.tag == "LargeStarTag")
        {
            score += largeStarScore;
        }
        else if (other.gameObject.tag == "SmallCloudTag")
        {
            score += smallCloudScore;
        }
        else if (other.gameObject.tag == "LargeCloudTag")
        {
            score += largeCloudScore;
        }

        //スコアを表示
        this.scoreText.GetComponent<Text>().text = string.Format("Score:{0}", score);
    }
}
