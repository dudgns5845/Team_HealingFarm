using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameManager_HYO : MonoBehaviour
{
    
    public Text nickName;
    public Text mapName;

    void Start()
    {
        print(DateTime.Now);
       


        //시간 차 계산 (coin)
        DataBaseManager.instance.GetTimeCoin(GetCoinText);


        //Get UserInfo
        nickName.text = DataBaseManager.instance.User.nickName;
        mapName.text = DataBaseManager.instance.User.mapName;

        DataBaseManager.instance.GetMapInfo(GetMap);

    }


    //coin 
    public int goneTime;
    public int myCurrCoinJson;
    public int myCoin;
    public int getCoin = 10;
    public int coinPerM = 1;
    public Text myCoinText;

    public void CoinSystem()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitinfo;

        if (Physics.Raycast(ray, out hitinfo))
        {
            if(hitinfo.transform.tag == "Animal")
            {
                myCoin += getCoin;
            }
        }
    }
    void GetCoinText(bool result)
    {
        TimeSpan timeSpan;
        DateTime userIndate;
        DateTime start;

        start = DateTime.Now;
        userIndate = DataBaseManager.instance.SetDate(DataBaseManager.instance.exitTimeJson);
        print("원래 있던거: " + userIndate);
        timeSpan = start - userIndate;
        goneTime = (int)timeSpan.TotalMinutes; //minutes Diff
        print("한번 봅시다  - " + goneTime);

        myCurrCoinJson = DataBaseManager.instance.exitTimeJson.myCurrCoin;
        myCoin += myCurrCoinJson + (goneTime * coinPerM);

        //coin Tostring()
        string coinString = myCoin.ToString();
        myCoinText.text = coinString;

        //뒤로가기 누르면 종료 로딩 화면
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                SceneManager.LoadScene("LodingScene");
            }
        }
    }
    public void OnclickGameExit()
    {
        print("coin save");
        DateTime exit = DateTime.Now;

        //DataBaseManager.instance.exitTimeJson = new EndTime(exit.Month, exit.Year, exit.Day, exit.Hour, exit.Minute, exit.Second);
        DataBaseManager.instance.exitTimeJson = DataBaseManager.instance.SetNow(DateTime.Now);

        //DataBaseManager.instance.exitTimeJson.exitTime = exit;
        DataBaseManager.instance.exitTimeJson.myCurrCoin = myCoin;
        DataBaseManager.instance.SaveTimeCoin(GetCoinText);
    }

    //inventory
    public GameObject inventory;
    public void OnclickInventory()
    {
        if(inventory.activeSelf == false)
        {
            inventory.SetActive(true);
        }
        else
        {
            inventory.SetActive(false);
        }
        

    }

    //itemPlace
    void GetMap(bool result)
    {
        List<ItemInfo> itemDatas = DataBaseManager.instance.itemDatas;
        for (int i = 0; i < itemDatas.Count; i++)
        {
            GameObject item = Instantiate(items[itemDatas[i].type]);

            item.transform.position = itemDatas[i].pos;

            item.transform.eulerAngles = itemDatas[i].rot;

        }

    }

    //select item
    public GameObject[] items;
    public Button[] itemBtns;
    public int selectItem;

    //touch
    private Touch tempTouchs;
    private Vector3 touchedPos;

    private bool touchOn;

    public void SelectItemBtn()
    {
        for(int i = 0; i < itemBtns.Length; i++)
        {
            if(itemBtns[i] == gameObject)
            {
                selectItem = i;
            }
        }
    }
  
    void Update()
    {
        //coin text
        string coinString = myCoin.ToString();
        myCoinText.text = coinString;

       
        //if (Input.GetKeyDown(KeyCode.Alpha1)) selectItem = 0;
        //if (Input.GetKeyDown(KeyCode.Alpha2)) selectItem = 1;
        //if (Input.GetKeyDown(KeyCode.Alpha3)) selectItem = 2;
        //if (Input.GetKeyDown(KeyCode.Alpha4)) selectItem = 3;
        //if (Input.GetKeyDown(KeyCode.Alpha5)) selectItem = 4;
        //if (Input.GetKeyDown(KeyCode.Alpha6)) selectItem = 5;

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            touchOn = false;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject item = Instantiate(items[selectItem], hit.point, Quaternion.identity);

                ItemInfo data = new ItemInfo();
                data.type = selectItem;
                data.pos = hit.point;

                data.rot = item.transform.eulerAngles;

                DataBaseManager.instance.itemDatas.Add(data);

                //touchOn = false;

                //if (Input.touchCount > 0)
                //{    //터치가 1개 이상이면
                //    for (int i = 0; i < Input.touchCount; i++)
                //    {
                //        tempTouchs = Input.GetTouch(i);
                //        //touch phase
                //        if (tempTouchs.phase == TouchPhase.Began)
                //        {
                //            //Get world position
                //            touchedPos = Camera.main.ScreenToWorldPoint(tempTouchs.position);
                //            touchOn = true;
                //        }
                //        else if (tempTouchs.phase == TouchPhase.Ended)
                //        {   
                            

                //            break;   
                //        }
                //    }
                //}
                
            }
        }
    }

    void SaveMap(bool result)
    {

    }
    public void SaveMapBtn()
    {
        DataBaseManager.instance.SaveMap(SaveMap);

    }

    //friendsInfo

    public InputField friendName;

    public GameObject friendscrollView;
    public void OnClickFriendsBtn()
    {
        friendscrollView.SetActive(true);
    }
    public void ExitFriendsView()
    {
        friendscrollView.SetActive(false);
    }
    
    //add friend
    void GetFriendText(bool result)
    {

    }

    public void AddFriendBtn()
    {
        DataBaseManager.instance.friend.nickNameF = friendName.text;
        DataBaseManager.instance.AddFriend(GetFriendText);
    }

}
