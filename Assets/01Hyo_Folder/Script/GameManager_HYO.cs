using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

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

    //item market
    public Button[] marketItems;
    public GameObject marketPopUp;

    public void MarketSystem()
    {
        marketPopUp.SetActive(true);
    }
    public void BuyBtn()
    {

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

        timeSpan = start - userIndate;
        goneTime = (int)timeSpan.TotalMinutes; //minutes Diff


        myCurrCoinJson = DataBaseManager.instance.exitTimeJson.myCurrCoin;
        myCoin += myCurrCoinJson + (goneTime * coinPerM);

        //coin Tostring()
        string coinString = myCoin.ToString();
        myCoinText.text = coinString;

        
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
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

        Application.Quit();
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
    //Market
    public GameObject market;
    public void OnclickMarket()
    {
        if (market.activeSelf == false)
        {
            market.SetActive(true);
        }
        else
        {
            market.SetActive(false);
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

    public void ItemBench1()
    {
        selectItem = 0;
    }
    public void ItemBench2()
    {
        selectItem = 1;
    }
    public void fence()
    {
        selectItem = 2;
    }
    public void lightHolder()
    {
        selectItem = 3;
    }
    public void house()
    {
        selectItem = 4;
    }
    public void tree()
    {
        selectItem = 5;
    }

    void Update()
    {
        //coin text
        string coinString = myCoin.ToString();
        myCoinText.text = coinString;
           
        
        if (inventory.activeSelf == true)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        GameObject item = Instantiate(items[selectItem], hit.point, Quaternion.identity);

                        ItemInfo data = new ItemInfo();
                        data.type = selectItem;
                        data.pos = hit.point;

                        data.rot = item.transform.eulerAngles;

                        DataBaseManager.instance.itemDatas.Add(data);
                    }
                }                   
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
        if (friendscrollView.activeSelf == false)
        {
            friendscrollView.SetActive(true);
        }
        else
        {
            friendscrollView.SetActive(false);
        }
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
