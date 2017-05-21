using ChuMeng;
using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInfo
{
    public WeaponType Type;
    public string LeftPath;
    public string RightPath;
    public int CostMoney;
    public int BaseDamage;
    public int AddDamage;
    public string Name;
    public string Desc;
}

public class WeaponFactory : Singleton<WeaponFactory>
{
    private Dictionary<WeaponType, WeaponInfo> dictInfo = null;

    public override void init()
    {
        dictInfo = new Dictionary<WeaponType, WeaponInfo>();
        initConfig();
    }

    /// <summary>
    /// 创建武器，弓箭特殊处理
    /// </summary>
    /// <param name="wType"></param>
    /// <param name="entity"></param>
    public void createWeapon(WeaponType wType, BaseEntity entity)
    {
        if (!dictInfo.ContainsKey(wType))
        {
            Debug.Log("<color=red>配置表未配置此武器</color>");
            return;
        }
        WeaponInfo info = dictInfo[wType];
        //弓箭做特殊处理
        if (wType == WeaponType.bow)
        {
            string leftPath = info.LeftPath;
            string rightPath = info.RightPath;
            loadWeapon(leftPath, typeof(WeaponBow), entity, info);
            loadWeapon(rightPath, typeof(WeaponArrow), entity, info);
        }
        else
        {
            string rightPath = info.RightPath;
            loadWeapon(rightPath, getWeaponType(wType), entity, info);
        }
    }

    private static void loadWeapon(string path, Type t, BaseEntity entity, WeaponInfo info)
    {
        ResMgr.Instance.load(path, (obj) =>
        {
            GameObject go = obj as GameObject;
            BaseWeapon bw = go.GetComponent<BaseWeapon>();
            if (bw == null)
            {
                bw = go.AddComponent(t) as BaseWeapon;
            }
            bw.setAgent(entity);
            bw.setInfo(info);
        });
    }


    /// <summary>
    /// 武器加载路径
    /// </summary>
    /// <param name="wType"></param>
    /// <returns></returns>
    private static string getPath(WeaponType wType)
    {
        string path = "";
        switch (wType)
        {
            case WeaponType.Glock17:
                path = "Weapon/Glock17";
                break;
            case WeaponType.UMP45:
                path = "Weapon/UMP45";
                break;
            case WeaponType.Mac10:
                path = "Weapon/Mac-10";
                break;
            case WeaponType.M79:
                path = "Weapon/M79";
                break;
            case WeaponType.Shot971:
                path = "Weapon/Shot97-1";
                break;
            case WeaponType.mp7:
                path = "Weapon/mp7";
                break;
            default:
                Debug.Log("现在没有这把武器");
                break;
        }
        return path;
    }
    private static Type getWeaponType(WeaponType wType)
    {
        Type t = null;
        switch (wType)
        {
            case WeaponType.Glock17:
                t = typeof(WeaponGlock17);
                break;
            case WeaponType.UMP45:
                t = typeof(WeaponUMP45);
                break;
            case WeaponType.Mac10:
                t = typeof(WeaponMac10);
                break;
            case WeaponType.M79:
                t = typeof(WeaponM79);
                break;
            case WeaponType.Shot971:
                t = typeof(WeaponShot971);
                break;
            case WeaponType.mp7:
                t = typeof(weaponmp7);
                break;
            default:
                Debug.Log("<color=red>现在没有这把武器</color>");
                break;
        }
        return t;
    }

    /// <summary>
    ///武器静态数据 
    /// </summary>
    private void initConfig()
    {
        List<SupplyConfigData> lst = GameData.SupplyConfig;
        for (int i = 0; i < lst.Count; i++)
        {
            WeaponInfo data = new WeaponInfo();
            data.Type = (WeaponType)lst[i].tempId;
            data.RightPath = lst[i].rightPath;
            data.LeftPath = lst[i].leftPath;
            data.CostMoney = lst[i].costMoney;
            data.BaseDamage = lst[i].baseDamage;
            data.AddDamage = lst[i].addDamage;
            data.Name = lst[i].Name;
            data.Desc = lst[i].Desc;
            if (!dictInfo.ContainsKey(data.Type))  { dictInfo.Add(data.Type, data); }
        }
    }

    /// <summary>
    /// 接口 获取所有武器信息
    /// </summary>
    /// <param name="wType"></param>
    /// <returns></returns>
    public WeaponInfo getWeaponInfo(WeaponType wType)
    {
        if (dictInfo.ContainsKey(wType))
        {
            return dictInfo[wType];
        }
        return null;
    }

    public List<WeaponInfo> getWeaponInfo()
    {
        return new List<WeaponInfo>(dictInfo.Values);
    }

}

