using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour {

    protected EntityMainPlayer agent;
    private GameObject bullet;
    private Transform firePoint;
    private ParticleSystem fxSystem;
    protected WeaponInfo info;


    private void Start()
    {
        onStart();
    }

    public virtual void onStart()
    {
        bullet = this.transform.Find("model/Bullet").gameObject;
        firePoint = this.transform.Find("model/firePoint");
        fxSystem = this.transform.Find("model/firePoint/FireFX").GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = fxSystem.main;
        main.playOnAwake = false;
        bullet.SetActive(false);
    }

    public virtual void onDispose()
    {
        Destroy(this);
        Destroy(this.gameObject);
    }

    /// <summary>
    /// 设置agent(武器的持有者实体),重置位置
    /// </summary>
    /// <param name="entity"></param>
    public virtual void setAgent(BaseEntity entity)
    {
        if (entity != null)
        {
            agent = entity as EntityMainPlayer;
            resetTrans();
        }
    }

    public virtual void setInfo(WeaponInfo info)
    {
        this.info = info;
    }

    public virtual void resetTrans()
    {
        agent.setRightWeapon(this);
        this.transform.SetParent(agent.RightHand);
        this.transform.localPosition = new Vector3(0, 0, 0);
        this.transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    public virtual bool isCanUse()
    {
        return true;
    }

    public virtual void onFire()
    {
        fxSystem.Play();
        Ray ray = new Ray(firePoint.position, firePoint.forward);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 300))
        {
            if (hitInfo.collider.tag == "Wall")
            {
                EffectInfo info = new EffectInfo(hitInfo.point, new Vector3(180, 0, 0), hitInfo.collider.transform);
                EffectMgr.Instance.createEffect(10001, info);
                EffectMgr.Instance.createEffect(10004, info);
            }
            else if (hitInfo.collider.tag == "Ground")
            {
                EffectInfo info = new EffectInfo(hitInfo.point, new Vector3(-90, 0, 0), hitInfo.collider.transform);
                EffectMgr.Instance.createEffect(10002, info);
                EffectMgr.Instance.createEffect(10003, info);
            }
            else if (hitInfo.collider.gameObject.GetComponent<EntityMonster>() != null)
            {
                EffectInfo info = new EffectInfo(new Vector3(hitInfo.point.x, hitInfo.point.y - 1f, hitInfo.point.z), new Vector3(0, 0, 0), hitInfo.collider.transform);
                EffectMgr.Instance.createEffect(10007, info, false);
                int damage = (int)this.info.BaseDamage;  
                hitInfo.collider.gameObject.GetComponent<EntityMonster>().onDamage(damage);
            }
        }
    }


    /// <summary>
    /// 弓箭射击
    /// </summary>
    public virtual void bowOnFire()
    {

    }
    //拉弓 定义到子类 强转也可以
    public virtual void bowOnPull(float dis)
    {

    }

}
