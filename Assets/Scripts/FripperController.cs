using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FripperController : MonoBehaviour
{
    //HingeJointコンポーネント
    private HingeJoint myHingeJoint;

    //初期の傾き
    private float defaultAngle = 20;
    //弾いた時の傾き
    private float flickAngle = -20;

    //フリッパーの状態
    //下がっている
    private const int FRIPPER_STATE_DOWN = 0;
    //上がっている
    private const int FRIPPER_STATE_UP = 1;
    int fripperState = FRIPPER_STATE_DOWN;

    //タッチ非対応デバイス用
    //フリッパー操作キー
    UnityEngine.KeyCode targetKeyCode;

    //タッチ対応デバイス用
    //フリッパー操作対象のタッチID
    int? fripperControlTouchId = null;
    //フリッパー操作対象のタッチ
    Touch fripperControlTouch;
    //フリッパー操作を受け付けるタッチのx座標範囲
    float fripperControlTouchMaxX = 0.0f;
    float fripperControlTouchMinX = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        //UnityEditorで実行する場合
        //オブジェクトのタグによって操作キーを定義
        if(tag == "LeftFripperTag")
        {
            targetKeyCode = KeyCode.LeftArrow;
        }
        else if(tag == "RightFripperTag")
        {
            targetKeyCode = KeyCode.RightArrow;
        }

        //iPhone実機で実行する場合
        //オブジェクトのタグによってタッチ操作を受け付けるx座標範囲を定義
        if (tag == "LeftFripperTag")
        {
            fripperControlTouchMaxX = Screen.width / 2.0f;
            fripperControlTouchMinX = 0;
        }
        else if (tag == "RightFripperTag")
        {
            fripperControlTouchMaxX = (float)Screen.width;
            fripperControlTouchMinX = Screen.width / 2.0f;
        }

        //フリッパー操作対象のタッチインスタンス
        fripperControlTouch = new Touch();

        //HinjeJointコンポーネント取得
        this.myHingeJoint = GetComponent<HingeJoint>();

        //フリッパーの傾きを設定
        SetAngle(this.defaultAngle);
    }

    // Update is called once per frame
    void Update()
    {
        //UnityEditorで実行
        MoveFripperOnUnityEditor();

        //iPhone実機で実行
        MoveFripperOnIphone();
    }

    private void MoveFripperOnUnityEditor()
    {
        //フリッパーを下げる
        //フリッパーがすでに下がっている場合、何もしない
        if(fripperState != FRIPPER_STATE_DOWN)
        {
            //操作キーを離した時
            if (Input.GetKeyUp(targetKeyCode))
            {
                //フリッパーを下げる
                SetAngle(this.defaultAngle);
                //フリッパーの状態を「下がっている」に更新
                fripperState = FRIPPER_STATE_DOWN;
            }
        }

        //フリッパーを上げる
        //フリッパーがすでに上がっている場合、何もしない
        if(fripperState != FRIPPER_STATE_UP)
        {
            //操作キーを押した直後の時
            if (Input.GetKeyDown(targetKeyCode))
            {
                //フリッパーを上げる
                SetAngle(this.flickAngle);
                //フリッパーの状態を「上がっている」に更新
                fripperState = FRIPPER_STATE_UP;
            }
        }
    }

    private void MoveFripperOnIphone()
    {
        //フリッパーを下げる
        //フリッパーがすでに下がっている場合、何もしない
        if(fripperState != FRIPPER_STATE_DOWN)
        {
            //全タッチを調査
            for(int i = 0; i < Input.touchCount; i++)
            {
                //フリッパー操作対象のタッチである場合
                if(Input.touches[i].fingerId == fripperControlTouchId)
                {
                    //フリッパー操作対象のタッチの状態を取得
                    fripperControlTouch.phase = Input.touches[i].phase;
                    //フリッパー操作対象のタッチの状態が「解除直後」の時
                    if(fripperControlTouch.phase == TouchPhase.Ended)
                    {
                        //フリッパーを下げる
                        SetAngle(this.defaultAngle);
                        //フリッパー操作対象のタッチIDを破棄
                        fripperControlTouchId = null;
                        //フリッパーの状態を「下がっている」にする
                        fripperState = FRIPPER_STATE_DOWN;
                        break;
                    }
                }
            }
        }

        //フリッパーを上げる
        //フリッパーがすでに上がっている場合、何もしない
        if(fripperState != FRIPPER_STATE_UP)
        {
            //全タッチを調査
            for(int i = 0; i < Input.touchCount; i++)
            {
                //タッチの状態が「発生直後」かつタッチしているx座標が本フリッパー操作範囲内である場合
                if(Input.touches[i].phase == TouchPhase.Began &&
                    (Input.touches[i].position.x >= fripperControlTouchMinX) &&
                    (Input.touches[i].position.x < fripperControlTouchMaxX))
                //if(Input.touches[i].phase == TouchPhase.Moved)
                {
                    //フリッパーを上げる
                    SetAngle(this.flickAngle);
                    //フリッパー操作対象のタッチIDを記憶
                    fripperControlTouchId = Input.touches[i].fingerId;
                    //フリッパーの状態を「上がっている」にする
                    fripperState = FRIPPER_STATE_UP;
                    break;
                }
            }
        }
    }

    //フリッパーの傾きを設定
    public void SetAngle(float angle)
    {
        JointSpring jointSpr = this.myHingeJoint.spring;
        jointSpr.targetPosition = angle;
        this.myHingeJoint.spring = jointSpr;
    }
}
