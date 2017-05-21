using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSysUI : BaseUI
{
    private GameObject slot;
    private Text money;
    private Text crystalLife;
    private Dictionary<int, WeaponSysItem> dictItem = null;

    public override void resetUIInfo()
    {
        uiEnum = UIEnum.weaponSys;
        this.uiNode = UINode.main;
    }

    public override void onStart()
    {
        base.onStart();
        slot = this.cacheTrans.Find("itemContent/weaponSlot").gameObject;
        money = this.cacheTrans.Find("Message/Money/money").GetComponent<Text>();
        crystalLife = this.cacheTrans.Find("Message/Crystal/crystalLife").GetComponent<Text>();
        slot.SetActive(false);
        dictItem = new Dictionary<int, WeaponSysItem>();
        MessageCenter.Instance.addListener(MsgCmd.On_BB_Change_Value, onPropertyChanage);
    }

    private void onPropertyChanage(Message msg)
    {
        BType type = (BType)msg["type"];
        int val = (int)msg["val"];
        switch (type)
        {
            case BType.money:
                money.text = "金币: " + val;
                break;
            case BType.crystalLife:
                crystalLife.text = "水晶生命: " + val;
                break;
        }
    }

    public override void refreshUI()
    {
        insSlot();
    }

    private void insSlot()
    {
        WeaponSystemData dt = this.data as WeaponSystemData;
        if (dt == null)
        {
            return;
        }
        money.text = "金币: " + dt.Money.ToString();
        crystalLife.text = "水晶生命: " + dt.Score.ToString();
        List<WeaponSysItemData> lst = dt.WeaponInfoLst;
        if (lst != null && lst.Count > 0)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                if (!dictItem.ContainsKey(i))
                {
                    GameObject go = MonoBehaviour.Instantiate(slot, slot.transform.parent) as GameObject;
                    go.SetActive(true);
                    WeaponSysItem item = go.AddComponent<WeaponSysItem>();
                    dictItem[i] = item;
                }
                dictItem[i].setData(lst[i]);
            }
        }
    }
}

