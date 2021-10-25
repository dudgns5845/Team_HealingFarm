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
 
    // 내 정보 저장
    public void SaveUser(Action<bool> complete)
    {
        StartCoroutine(ISaveUser(complete));
    }

    IEnumerator ISaveUser(Action<bool> complete)
    {
        User.nickName = "돼지토끼";
        User.mapName = "~healing farm~";

        string json = JsonUtility.ToJson(User);

        //경로
        string path = "USER_INFO";// +FirebaseAuth.DefaultInstance.CurrentUser.nickName;
        var task = dataBase.GetReference(path).SetRawJsonValueAsync(json);
        yield return new WaitUntil(() => task.IsCompleted);
        if (task.Exception == null)
        {
            print("유저 정보 저장 성공");
            complete(true);
        }
        else
        {
            print("유저 정보 저장 실패 : " + task.Exception);
            complete(false);
        }
    }

    //정보 가져오기

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
                print("유저 정보 가져오기 성공");
                User = JsonUtility.FromJson<User>(task.Result.GetRawJsonValue());

                Debug.Log("닉네임: " +User.nickName+ ", 농장 이름: " + User.mapName);
                complete(true);
            }
            else
            {
                print("유저 정보 없음");
                //User.email = FirebaseAuth.DefaultInstance.CurrentUser.Email;
                SaveUser((result) =>
                {
                    // Firebase와 통신 끝나면 들어온다
                    complete(true);
                });

            }
        }
    }
}
