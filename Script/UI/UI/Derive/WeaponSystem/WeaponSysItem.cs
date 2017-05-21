using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WeaponSysItem : BaseUI
{
    private Text weaponName;
    private Text weaponCost;
    private GameObject weapon;
    private WeaponSysItemData dt = null;

    public override void resetUIInfo()
    {
        this.uiEnum = UIEnum.none;
        this.uiNode = UINode.none;
    }

    public override void onStart()
    {
        base.onStart();
        weaponName = this.cacheTrans.Find("weaponBtn/weaponName").GetComponent<Text>();
        weaponCost = this.cacheTrans.Find("weaponCost").GetComponent<Text>();
        UIEventTrigger uiEventTriggger = this.cacheObj.AddComponent<UIEventTrigger>();
        uiEventTriggger.setClickHandler(onBtnClick);
        uiEventTriggger.setEnterHandler(OpenUITip);
        uiEventTriggger.setExitHandler(CloseUITip);
    }

    private void CloseUITip()
    {
        UIMgr.Instance.closeUI(UIEnum.weaponSysTips);
    }

    private void OpenUITip()
    {
        dt.Pos = this.cacheObj.transform.position;
        UIMgr.Instance.openUI(UIEnum.weaponSysTips, dt);       
    }

    private void onBtnClick()
    {
        dt = this.data as WeaponSysItemData;
        if (dt != null)
        {
            Message msg = new Message(MsgCmd.On_Change_Weapon, this);
            msg["type"] = dt.Type;
            msg.Send();
        }
    }

    public override void refreshUI()
    {
        WeaponSysItemData dt = this.data as WeaponSysItemData;
        if (dt != null)
        {
            weaponName.text = dt.Name.ToString();
            weaponCost.text = "售价: " + dt.CostMoney.ToString();
            if (weapon == null)
            {
                //演示在control做处理
                string path = dt.Path;
                ResMgr.Instance.load(path, (obj) =>
                {
                    weapon = obj as GameObject;
                    weapon.transform.SetParent(this.cacheTrans);
                    weapon.transform.position = new Vector3(this.cacheTrans.position.x, this.cacheTrans.position.y, this.cacheTrans.position.z-1f);
                    weapon.transform.localScale = dt.Scale;
                    weapon.transform.localEulerAngles = new Vector3(0, 0, 0);
                });
            }
        }
    }
}

