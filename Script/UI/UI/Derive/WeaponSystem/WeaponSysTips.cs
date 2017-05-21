using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSysTips : BaseUI {

    private Text Name;
    private Text Desc;
    private Text Damage;

    public override void resetUIInfo()
    {
        this.uiEnum = UIEnum.weaponSysTips;
        this.uiNode = UINode.pop;
    }

    public override void onStart()
    {
        base.onStart();
        Name = this.cacheTrans.Find("Name").GetComponent<Text>();
        Desc = this.cacheTrans.Find("Desc").GetComponent<Text>();
        Damage = this.cacheTrans.Find("Damage").GetComponent<Text>();
    }

    public override void refreshUI()
    {
        base.refreshUI();
        WeaponSysItemData dt = this.data as WeaponSysItemData;
        if (dt != null)
        {
            Name.text = dt.Name;
            Desc.text = dt.Desc;
            Damage.text = (dt.BaseDamage + dt.AddDamage).ToString();
            this.cacheTrans.position = dt.Pos;
        }
    }

}
