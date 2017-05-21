using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class EntityMainPlayer : EntityDynamicActor {

    private float moveSpeed = 0.1f;
    private WeaponType wType = WeaponType.none;
    private BaseWeapon rightWeapon = null;
    private BaseWeapon leftWeapon = null;
    private Transform leftHand;
    private Transform rightHand;
    public VRTK_ControllerEvents vrtkEvents;

    public WeaponType WType
    {
        get
        {
            return this.wType;
        }
        set
        {
            if (this.wType != value)
            {
                if (rightWeapon != null) { rightWeapon.onDispose(); }
                if (leftWeapon != null) { leftWeapon.onDispose(); }
                this.wType = value;
                onChangeWeapon();
            }
        }
    }

    public Transform LeftHand
    {
        get
        {
            if (leftHand == null)
            {
                leftHand = this.CacheTrans.Find("Controller (left)");
            }
            return leftHand;
        }
    }

    public Transform RightHand
    {
        get
        {
            if (rightHand == null)
            {
                rightHand = this.CacheTrans.Find("Controller (right)");
            }
            return rightHand;
        }
    }

    private Transform eye;
    public Transform Eye
    {
        get
        {
            if (eye == null)
            {
                eye = this.CacheTrans.Find("Camera (eye)");
            }
            return eye;
        }
    }

    public override void onAwake()
    {
        this.EType = EntityType.player;
        this.UID = 19950803;
        this.HP = 100;
        this.OrgHP = 100;
        EntityMgr.Instance.addEntity(this);
        sendHPMsg();
    }

    public override void onStart()
    {
        base.onStart();
        vrtkEvents.TriggerPressed += onFire;
        vrtkEvents.TouchpadOnPress += onPlayerMove;
        vrtkEvents.ButtonTwoPressed += onOpenWeaponSysUI;
        this.WType = WeaponType.Glock17;
    }

    private void onOpenWeaponSysUI(object sender, ControllerInteractionEventArgs args)
    {
        Message msg = new Message(MsgCmd.Open_WeaponSystem_UI, this);
        //Vector3 pos = this.Eye.position + this.Eye.forward * 12;
        //msg["Pos"] = new Vector3(pos.x, 4f, pos.z);
        //Quaternion rot = this.Eye.rotation;
        //rot.x = 0;
        //rot.z = 0;
        //msg["Rot"] = rot;
        msg.Send();
    }

    private void OnEnable()
    {
        MessageCenter.Instance.addListener(MsgCmd.On_Change_Weapon, onChangeWeaponMsg);
        MessageCenter.Instance.addListener(MsgCmd.On_Change_Value, onChangeValue);
    }
    private void OnDisable()
    {
        MessageCenter.Instance.removeListener(MsgCmd.On_Change_Weapon, onChangeWeaponMsg);
        MessageCenter.Instance.removeListener(MsgCmd.On_Change_Value, onChangeValue);
    }

    private void onChangeWeaponMsg(Message msg)
    {
        WeaponType type = (WeaponType)msg["type"];
        this.WType = type;
    }

    public override void onUpdate()
    {
        base.onUpdate();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            this.WType = WeaponType.Glock17;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            this.WType = WeaponType.bow;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            this.WType = WeaponType.M79;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            this.WType = WeaponType.Mac10;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            this.WType = WeaponType.Shot971;
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            this.WType = WeaponType.UMP45;
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            this.WType = WeaponType.mp7;
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            Message msg = new Message(MsgCmd.Open_WeaponSystem_UI, this);
            msg.Send();
        }
    }

    private void onChangeWeapon()
    {
        WeaponFactory.Instance.createWeapon(this.WType, this);
    }

    private void onFire(object sender, ControllerInteractionEventArgs args)
    {
        if (rightWeapon != null)
        {
            rightWeapon.onFire();
        }
        if (leftWeapon != null)
        {
            vrtkEvents.TriggerReleased += bowOnFire;
            vrtkEvents.TriggerOnPress += onPress;
            leftWeapon.onFire();
        }
    }

    private void bowOnFire(object sender, ControllerInteractionEventArgs args)
    {
        if (rightWeapon != null)
            rightWeapon.bowOnFire();
        if (leftWeapon != null)
        {
            vrtkEvents.TriggerReleased -= bowOnFire;
            vrtkEvents.TriggerOnPress -= onPress;
            leftWeapon.bowOnFire();
        }
    }

    private void onPress(object sender, ControllerInteractionEventArgs args)
    {
        if (this.leftWeapon != null)
        {
            Vector3 rightVec = RightHand.InverseTransformPoint(LeftHand.position);
            leftWeapon.bowOnPull(rightVec.z);
        }
    }

    public void bowGiveUP()
    {
        if (leftWeapon != null)
        {
            WeaponBow bow = leftWeapon as WeaponBow;
            if (bow != null)
            {
                bow.onGiveUP();
            }
        }
        if (rightWeapon != null)
        {
            WeaponArrow arrow = rightWeapon as WeaponArrow;
            if (arrow != null)
            {
                arrow.onGiveUP();
            }
        }
    }

    public void setRightWeapon(BaseWeapon bw)
    {
        rightWeapon = bw;
    }

    public void setLeftWeapon(BaseWeapon bw)
    {
        leftWeapon = bw;
    }

    public override void onDispose()
    {
        base.onDispose();
        vrtkEvents.TriggerPressed -= onFire;
    }


    Quaternion rot = Quaternion.identity;
    Vector3 dir = Vector3.zero;
    private void onPlayerMove(object sender, ControllerInteractionEventArgs args)
    {
        dir = Eye.forward * args.touchpadAxis.y;
        rot = this.CacheTrans.rotation;
        rot = Quaternion.Euler(0, args.touchpadAxis.x, 0) * rot;
        this.CacheTrans.rotation = Quaternion.Lerp(this.CacheTrans.rotation, rot, 0.25f);// rot;
        CC.Move(dir * moveSpeed);
    }

    public override void onDamage(float damage)
    {
        this.HP -= damage;
        sendHPMsg();
        UIMgr.Instance.onDamageColor();
    }

    private void sendHPMsg()
    {
        Message msg = new Message(MsgCmd.On_HP_Change_Value, this);
        msg["HP"] = this.HP;
        msg["OrgHP"] = this.OrgHP;
        msg.Send();
    }

}
