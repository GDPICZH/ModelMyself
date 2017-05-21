using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodPart : BasePart
{
    private SpriteRenderer bloodSp;
    private SpriteRenderer bgbloodSp;
    private GameObject bgPart;

    public BloodPart(PartType type) : base(type)
    {
    }

    public override void create(Transform parent, EntityInfo info)
    {
        bgPart = new GameObject(this.PType.ToString() + "bg");
        bgPart.transform.SetParent(parent);
        bgPart.transform.localPosition = new Vector3(0, 2.05f, .15f);
        bgPart.transform.localScale = new Vector3(1.01f, 0.15f, 1);
        bgbloodSp = bgPart.AddComponent<SpriteRenderer>();
        bgbloodSp.sprite = SpriteMgr.Instance.getSprite("whiteblood2");
        bgbloodSp.color = new Color(0, 0, 0, 0.15f);
        bgbloodSp.sortingOrder = 0;

        part = new GameObject(this.PType.ToString());
        part.transform.SetParent(parent);
        part.transform.localPosition = new Vector3(0, 2.05f, 0.15f);
        part.transform.localScale = new Vector3(1f, 0.1f, 1);
        bloodSp = part.AddComponent<SpriteRenderer>();
        bloodSp.sprite = SpriteMgr.Instance.getSprite("whiteblood2");
        bloodSp.color = Color.red;
        bloodSp.sortingOrder = 1;
    }


    public override void setFloat(float rate)
    {
        if (part != null)
        {
            float end = (1 - rate) / 0.1f * -0.05f;
            part.transform.localScale = new Vector3(rate, 0.1f, 1f);
            part.transform.localPosition = new Vector3(end, 2.05f, 0.15f);
        }
    }

}
