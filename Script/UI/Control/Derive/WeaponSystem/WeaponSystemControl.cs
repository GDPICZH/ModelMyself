using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystemControl : BaseControl
{
    private List<WeaponSysItemData> lstInfo = null;

    public override void initEnum()
    {
        this.uiEnum = UIEnum.weaponSys;
        lstInfo = new List<WeaponSysItemData>();
    }

    public override void initListener()
    {
        MessageCenter.Instance.addListener(MsgCmd.Open_WeaponSystem_UI, onOpenUI);
    }

    private void onOpenUI(Message msg)
    {
        if (UIMgr.Instance.isOpen(this.uiEnum))
        {
            UIMgr.Instance.closeUI(this.uiEnum);
            return;
        }

        if (lstInfo != null && lstInfo.Count <= 0)
        {
            List<WeaponInfo> lst = WeaponFactory.Instance.getWeaponInfo();
            for (int i = 0; i < lst.Count; i++)
            {
                WeaponSysItemData dt = new WeaponSysItemData();
                dt.Type = lst[i].Type;
                dt.CostMoney = lst[i].CostMoney;
                dt.BaseDamage = lst[i].BaseDamage;
                dt.AddDamage = lst[i].AddDamage;
                dt.Path = dt.Type == WeaponType.bow ? lst[i].LeftPath : lst[i].RightPath;
                dt.Name = lst[i].Name;
                dt.Desc = lst[i].Desc;
                dt.Scale = dt.Type == WeaponType.bow ? new Vector3(10, 10, 0.1f) : new Vector3(100, 100, 1);
                lstInfo.Add(dt);
            }
        }
        WeaponSystemData data = new WeaponSystemData();
        data.WeaponInfoLst = lstInfo;

        BaseEntity player = EntityMgr.Instance.getEntityById(19941001);
        if (player != null)
        {
            data.Money = player.getValue(BType.money);
        }
        this.updateUI(data);
    }

}

