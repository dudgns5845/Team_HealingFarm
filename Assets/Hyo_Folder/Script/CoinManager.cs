using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Firebase;
using Firebase.Database;

public class CoinManager : MonoBehaviour
{
    //시간
    public Text timeUI;
    // total coin
    public int[] Money;
    // current coin 
    public int moneyCurr;
    // 클릭당 돈
    public int getMoney = 10;    // 출력
    public Text MymoneyText;

    //시간저장
    //public void SendTime()
    //{
    //    FirebaseDatabase.DefaultInstance.RootReference.Child("TimeStampTest")
    //   .Child("TimeStampValue").SetValueAsync(GetCurrentTimeStamp());
    //    //Time.realtimeSinceStartup

    //}
    //public static object GetCurrentTimeStamp()
    //{
    //    return ServerValue.Timestamp;

    //}

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            print("get Q");
            for (int i = 0; i < 26; i++)
            {
                Money[i] += getMoney;
            }
            
        }
        Theorem();
        MymoneyToString();
    }
    void Theorem()
    {
        // current money
        try
        {
            for (int i = 0; i < 26; i++)
            {
                if (Money[i] > 0)
                {
                    moneyCurr = i;
                }
            }
        }
        catch(IndexOutOfRangeException exception)
        {
            Debug.Log(exception.Message);
        }
        finally
        {
            Debug.Log(moneyCurr);
        }

        try
        {
            // moneyCurr값 만큼 단위를 정리
            for (int i = 0; i <= moneyCurr; i++)
            {
                // 만약, i번째 배열에 돈이 1000이상이라면
                // 거기서 1000을 빼고 윗 배열에 1을 더해준다.
                if (Money[i] >= 1000)
                {
                    Money[i] -= 1000;
                    Money[i + 1] += 1;
                }
                // 만약, i번째 배열의 값이 음수라면
                if (Money[i] < 0)
                {
                    // 만약, i의 값이 나의 현재 자산의 값보다 작으면
                    // 윗 배열에서 1을 빼고 음수인 i번째 배열에 1000을 더한다.
                    if (moneyCurr > i)
                    {
                        Money[i + 1] -= 1;
                        Money[i] += 1000;
                    }
                }
            }
        }
        catch(IndexOutOfRangeException exception)
        {
            Debug.Log(exception.Message);
        }
        
    }
    string MymoneyToString()
    {
        // 플레이어가 볼 수 있는 재화의 형태로 표현
        float a = Money[moneyCurr];
        // 만약, moneyCurr가 0보다 크다면 소수점
        if (moneyCurr > 0)
        {
            float b = Money[moneyCurr - 1];
            a += b / 1000;
        }
        // 만약, 0과 같다면 바로 출력
        if (moneyCurr == 0)
        {
            a += 0;
        }
        //문자로 바꾸기 A~ 
        char unit = (char)(65 + moneyCurr);
        string p;
        p = (float)(Math.Truncate(a * 100) / 100) + unit.ToString();
        MymoneyText.text = p;

        return p;

    }

}
