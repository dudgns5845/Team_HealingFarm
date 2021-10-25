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
    }


    void Update()
    {
        
    }
}
