using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DDOLObj : MonoBehaviour
{
    public static DDOLObj Instance;
    private List<BaseControl> controls = null;

    private void Awake()
    {
        MonoBehaviour.DontDestroyOnLoad(this.gameObject);
        this.gameObject.AddComponent<TimeMgr>();
        GameObject go = GameObject.Find("EventSystem");
        if (go == null)
        {
            go = new GameObject("EventSystem");
            go.AddComponent<EventSystem>();
            go.AddComponent<StandaloneInputModule>();
        }
        Instance = this;
    }

    private void updateGold()
    {
        Message msg = new Message(MsgCmd.On_Change_Value, this);
        msg["type"] = BType.money;
        msg["val"] = 3;
        msg.Send();
    }

    private void Start()
    {
        ServerTest.Instance.initServer();
        controls = new List<BaseControl>();
        controls.Add(new WeaponSystemControl());
        controls.Add(new MainMeunControl());
        initControl();
    }

    //初始化control监听
    private void initControl()
    {
        for (int i = 0; i < controls.Count; i++)
        {
            controls[i].initListener();
            controls[i].initEnum();
        }
    }


}

