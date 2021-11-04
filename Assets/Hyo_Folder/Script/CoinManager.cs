using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Firebase;
using Firebase.Database;

public class CoinManager : MonoBehaviour
{
    //�ð�
    public Text timeUI;
    // total coin
    public int[] Money;
    // current coin 
    public int moneyCurr;
    // Ŭ���� ��
    public int getMoney = 10;    // ���
    public Text MymoneyText;

    //�ð�����
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
            // moneyCurr�� ��ŭ ������ ����
            for (int i = 0; i <= moneyCurr; i++)
            {
                // ����, i��° �迭�� ���� 1000�̻��̶��
                // �ű⼭ 1000�� ���� �� �迭�� 1�� �����ش�.
                if (Money[i] >= 1000)
                {
                    Money[i] -= 1000;
                    Money[i + 1] += 1;
                }
                // ����, i��° �迭�� ���� �������
                if (Money[i] < 0)
                {
                    // ����, i�� ���� ���� ���� �ڻ��� ������ ������
                    // �� �迭���� 1�� ���� ������ i��° �迭�� 1000�� ���Ѵ�.
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
        // �÷��̾ �� �� �ִ� ��ȭ�� ���·� ǥ��
        float a = Money[moneyCurr];
        // ����, moneyCurr�� 0���� ũ�ٸ� �Ҽ���
        if (moneyCurr > 0)
        {
            float b = Money[moneyCurr - 1];
            a += b / 1000;
        }
        // ����, 0�� ���ٸ� �ٷ� ���
        if (moneyCurr == 0)
        {
            a += 0;
        }
        //���ڷ� �ٲٱ� A~ 
        char unit = (char)(65 + moneyCurr);
        string p;
        p = (float)(Math.Truncate(a * 100) / 100) + unit.ToString();
        MymoneyText.text = p;

        return p;

    }

}
