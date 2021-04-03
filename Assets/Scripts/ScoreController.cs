using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
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
        //シーン中のScoreTextオブジェクトを取得
        this.scoreText = GameObject.Find("ScoreText");

    }

    // Update is called once per frame
    void Update()
    {

    }

    //衝突時に呼ばれる関数
    private void OnCollisionEnter(Collision other)
    {
        
        //タグによって点数を変える
        if (tag == "SmallStarTag")
        {
            score += smallStarScore;
            Debug.Log(string.Format("小さい星：{0}", score));

        }
        else if (tag == "LargeStarTag")
        {
            score += largeStarScore;
            Debug.Log(string.Format("大きい星：{0}", score));

        }
        else if (tag == "SmallCloudTag")
        {
            score += smallCloudScore;
            Debug.Log(string.Format("小さい雲：{0}", score));

        }
        else if(tag == "LargeCloudTag")
        {
            score += largeCloudScore;
            Debug.Log(string.Format("大きい雲：{0}", score));

        }

        //スコアを表示
        this.scoreText.GetComponent<Text>().text = string.Format("Score:{0}", score);
    }
}
