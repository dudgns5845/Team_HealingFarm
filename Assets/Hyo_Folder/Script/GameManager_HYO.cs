using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager_HYO : MonoBehaviour
{
    public Text nickName;
    public Text mapName;

    void Start()
    {
        nickName.text = DataBaseManager.instance.User.nickName;
        mapName.text = DataBaseManager.instance.User.mapName;

        DataBaseManager.instance.GetMapInfo(GetMap);
    }

    public GameObject[] items;
    
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

    public int selectItem;
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1)) selectItem = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) selectItem = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) selectItem = 2;

        if (Input.GetMouseButtonDown(0))
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

    void SaveMap(bool result)
    {

    }
    public void SaveMapBtn()
    {
        DataBaseManager.instance.SaveMap(SaveMap);

    }
}
