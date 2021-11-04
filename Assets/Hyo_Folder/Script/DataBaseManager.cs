using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using System;

[Serializable]
public class EndTime
{
    public int myCurrCoin;
    //public DateTime exitTime;
    // mm/dd/yyyy hh:mm:ss

    public int month;
    public int day;
    public int year;
    public int hour;
    public int minute;
    public int second;

    public EndTime(int month, int day, int year, int hour, int minute, int second)
    {
        this.month = month;
        this.day = day;
        this.year = year;
        this.hour = hour;
        this.minute = minute;
        this.second = second;
    }
}

[Serializable]
public class User
{

    public string nickName = "";
    public string mapName = "";

}

[Serializable]
public struct ItemInfo
{
    public int type;
    public Vector3 pos;
    public Vector3 rot;
}

[Serializable]
public struct ItemDataJson
{
    public List<ItemInfo> itemdatajsons;
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
    }

    //Save time , coin 
    public EndTime exitTimeJson;
   
    public void SaveTimeCoin(Action<bool> complete)
    {
        StartCoroutine(ISaveTimeCoin(complete));
    }
    IEnumerator ISaveTimeCoin(Action<bool> complete)
    {
        string json =JsonUtility.ToJson(exitTimeJson);

        //���
        string path = "USER_INFO/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId+"/MyCoin";
        var task = dataBase.GetReference(path).SetRawJsonValueAsync(json);
        yield return new WaitUntil(() => task.IsCompleted);
        if (task.Exception == null)
        {
            print("�ð� ���� �Ϸ�");
            complete(true);
        }
        else
        {
            print("�ð� ���� ���� : " + task.Exception);
            complete(false);
        }
    }
    //Get Time
    public void GetTimeCoin(Action<bool> complete)
    {
        StartCoroutine(IGetTimeCoin(complete));
    }
    IEnumerator IGetTimeCoin(Action<bool> complete)
    {
        string path = "USER_INFO/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId+ "/MyCoin";
        var task = dataBase.GetReference(path).GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);
        if (task.Exception == null)
        {
            if (task.Result.Exists)
            {
                print(task.Result.GetRawJsonValue());
                print("���� ���� �ε� �Ϸ�");
                exitTimeJson = JsonUtility.FromJson<EndTime>(task.Result.GetRawJsonValue());
                //Debug.Log(exitTimeJson.exitTime);
                //Debug.Log(exitTimeJson.myCurrCoin);
                //Debug.Log(exitTimeJson.year);
                //Debug.Log(exitTimeJson.month);
                //Debug.Log(exitTimeJson.day);
                //Debug.Log(exitTimeJson.hour);
                //Debug.Log(exitTimeJson.minute);
                DateTime testDate = SetDate(exitTimeJson);
                Debug.Log(testDate);
                complete(true);
            }
            else
            {
                print("���� ���� �ε� ����" + task.Exception);

                GetTimeCoin((result) =>
                {
                    complete(true);
                });

            }
        }
    }

    public DateTime SetDate(EndTime endTimeValue)
    {
        DateTime temp = new DateTime(endTimeValue.year, endTimeValue.month, endTimeValue.day, endTimeValue.hour, endTimeValue.minute, endTimeValue.second);
        return temp;
    }

    public EndTime SetNow(DateTime now)
    {
        EndTime temp = new EndTime(now.Month, now.Day, now.Year, now.Hour, now.Minute, now.Second);
        return temp;
    }

    public User User;

    //save user info
    public void SaveUser(Action<bool> complete)
    {

        StartCoroutine(ISaveUser(complete));
    }

    IEnumerator ISaveUser(Action<bool> complete)
    {
        string json = JsonUtility.ToJson(User);

        //���
        string path = "USER_INFO/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        string pathF = "USER_INFO/" +User.nickName;
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

    //get user info
    public void GetUserInfo(Action<bool> complete)
    {
        StartCoroutine(IGetUserInfo(complete));
    }
    IEnumerator IGetUserInfo(Action<bool> complete)
    {
        string path = "USER_INFO/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        var task = dataBase.GetReference(path).GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);
        if (task.Exception == null)
        {
            if (task.Result.Exists)
            {
                print(task.Result.GetRawJsonValue());
                print("���� ���� �������� ����");
                User = JsonUtility.FromJson<User>(task.Result.GetRawJsonValue());

                Debug.Log("�г���: " + User.nickName + ", ���� �̸�: " + User.mapName);
                complete(true);
            }
            else
            {
                print("���� ���� ����");

                SaveUser((result) =>
                {
                    // Firebase�� ��� ������ ���´�
                    complete(true);
                });

            }
        }
    }

    public ItemDataJson ItemDataJson;
    public List<ItemInfo> itemDatas = new List<ItemInfo>();
    
    //save map info
    public void SaveMap(Action<bool> complete)
    {

        StartCoroutine(ISaveMap(complete));
    }

    IEnumerator ISaveMap(Action<bool> complete)
    {
        ItemDataJson.itemdatajsons = itemDatas;
        string json = JsonUtility.ToJson(ItemDataJson);

        //���
        string path = "USER_INFO/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId +"/MAP_INFO";
        string pathF = "USER_INFO/" + User.nickName + "/";
        var task = dataBase.GetReference(path).SetRawJsonValueAsync(json);
        yield return new WaitUntil(() => task.IsCompleted);
        if (task.Exception == null)
        {
            print("�� ���� ���� �Ϸ�");
            complete(true);
        }
        else
        {
            print("�� ���� ���� ���� : " + task.Exception);
            complete(false);
        }
    }
    //get map info
    public void GetMapInfo(Action<bool> complete)
    {
        StartCoroutine(IGetMapInfo(complete));
    }
    IEnumerator IGetMapInfo(Action<bool> complete)
    {
        string path = "USER_INFO/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId+"/MAP_INFO";
        var task = dataBase.GetReference(path).GetValueAsync();
      
        yield return new WaitUntil(() => task.IsCompleted);
        if (task.Exception == null)
        {
            if (task.Result.Exists)
            {
                print(task.Result.GetRawJsonValue());
                print("�� ���� �ε� �Ϸ�");
                ItemDataJson = JsonUtility.FromJson<ItemDataJson>(task.Result.GetRawJsonValue());
                
                itemDatas = ItemDataJson.itemdatajsons;

                
                complete(true);
            }
            else
            {
                print("�� ���� �ε� ����");

                SaveMap((result) =>
                {
                    // Firebase�� ��� ������ ���´�
                    complete(true);
                });

            }
        }
    }

    //Get friends
    public void GetFriend(Action<bool> complete)
    {
        StartCoroutine(IGetFriend(complete));
    }
    IEnumerator IGetFriend(Action<bool> complete)
    {
        string path = "USER_INFO/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId + "/Friends";
        var task = dataBase.GetReference(path).GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);
        if (task.Exception == null)
        {
            if (task.Result.Exists)
            {
                print(task.Result.GetRawJsonValue());
                print("ģ�� ���� �������� ����");
                User = JsonUtility.FromJson<User>(task.Result.GetRawJsonValue());

                Debug.Log("�г���: " + User.nickName + ", ���� �̸�: " + User.mapName);
                complete(true);
            }
            else
            {
                print("ģ�� ���� ����");

                SaveUser((result) =>
                {
                    // Firebase�� ��� ������ ���´�
                    complete(true);
                });

            }
        }
    }
    //add firends
    [Serializable]
    public class Friend
    {
        public string nickNameF = "";
    }

    public Friend friend;
    public void AddFriend(Action<bool> complete)
    {
        StartCoroutine(IAddFriend(complete));
    }

    IEnumerator IAddFriend(Action<bool> complete)
    {
        string json = JsonUtility.ToJson(friend);

        //���
        string path = "USER_INFO/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId + "/Friends";
        var task = dataBase.GetReference(path).SetRawJsonValueAsync(json);
        yield return new WaitUntil(() => task.IsCompleted);
        if (task.Exception == null)
        {
            print("ģ�� �߰� ����");
            complete(true);
        }
        else
        {
            print("ģ�� �߰� ���� : " + task.Exception);
            complete(false);
        }
    }


}
