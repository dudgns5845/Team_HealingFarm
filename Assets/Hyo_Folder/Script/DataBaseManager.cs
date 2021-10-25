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
    public string userID = "";
    public string userName = "";

}
public class DataBaseManager : MonoBehaviour
{
    DatabaseReference dataRef;
    FirebaseDatabase dataBase;
    private void Start()
    {
        dataRef = FirebaseDatabase.DefaultInstance.RootReference;
        dataBase = FirebaseDatabase.DefaultInstance;
        writeNewUser("", "");
        // SaveUserInfo(Complete);
    }

    void Complete(bool aaa)
    {

    }

    public User userInfo;
 
    private void writeNewUser(string userID, string username)
    {
        //User user = new User(userID,username);
        //string json = JsonUtility.ToJson(user);

        //dataRef.Child("users").Child(username).SetRawJsonValueAsync(json);
      
        userInfo.userID = "userID!!!";
        userInfo.userName = "효진";

        string json = JsonUtility.ToJson(userInfo);

        dataRef.Child("users").Child("info").SetRawJsonValueAsync(json);
    }

    // 내 정보 저장
    public void SaveUserInfo(Action<bool> complete)
    {
        StartCoroutine(ISaveUserInfo(complete));
    }

    IEnumerator ISaveUserInfo(Action<bool> complete)
    {
        userInfo.userID = "userID!!!";
        userInfo.userName = "효진";

        string json = JsonUtility.ToJson(userInfo);

        string path = "USER_INFO/aaa";// +FirebaseAuth.DefaultInstance.CurrentUser.UserId;
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

}
