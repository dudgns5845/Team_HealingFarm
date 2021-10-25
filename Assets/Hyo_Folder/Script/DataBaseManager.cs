using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using System;

[System.Serializable]
public class User
{

    public string nickName = "";
    public string mapName = "";

}
public class DataBaseManager : MonoBehaviour
{
   
    public static DataBaseManager instance = null;

    void Awake()
    {
        if (null == instance)
        {

            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {

            Destroy(this.gameObject);
        }
    }


    FirebaseDatabase dataBase;
    private void Start()
    {
        dataBase = FirebaseDatabase.DefaultInstance;

        SaveUser(Complete);
    }

    void Complete(bool aaa)
    {

    }

    public User User;
 
    // �� ���� ����
    public void SaveUser(Action<bool> complete)
    {
        StartCoroutine(ISaveUser(complete));
    }

    IEnumerator ISaveUser(Action<bool> complete)
    {
        User.nickName = "�����䳢";
        User.mapName = "~healing farm~";

        string json = JsonUtility.ToJson(User);

        //���
        string path = "USER_INFO";// +FirebaseAuth.DefaultInstance.CurrentUser.nickName;
        var task = dataBase.GetReference(path).SetRawJsonValueAsync(json);
        yield return new WaitUntil(() => task.IsCompleted);
        if (task.Exception == null)
        {
            print("���� ���� ���� ����");
            complete(true);
        }
        else
        {
            print("���� ���� ���� ���� : " + task.Exception);
            complete(false);
        }
    }

    //���� ��������

    public void GetUserInfo(Action<bool> complete)
    {
        StartCoroutine(IGetUserInfo(complete));
    }
    IEnumerator IGetUserInfo(Action<bool> complete)
    {
        string path = "USER_INFO/"; //+ FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        var task = dataBase.GetReference(path).GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);
        if (task.Exception == null)
        {
            if (task.Result.Exists)
            {
                print(task.Result.GetRawJsonValue());
                print("���� ���� �������� ����");
                User = JsonUtility.FromJson<User>(task.Result.GetRawJsonValue());

                Debug.Log("�г���: " +User.nickName+ ", ���� �̸�: " + User.mapName);
                complete(true);
            }
            else
            {
                print("���� ���� ����");
                //User.email = FirebaseAuth.DefaultInstance.CurrentUser.Email;
                SaveUser((result) =>
                {
                    // Firebase�� ��� ������ ���´�
                    complete(true);
                });

            }
        }
    }
}
