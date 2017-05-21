using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StaticCamera : MonoBehaviour
{
    private Camera myCamera;
    private GameObject blood;
    private GameObject slot;
    private Image DieBg;
    private GameObject Die;
    private GameObject crystalBroken;
    private Image hpImg;
    private Text countDownText;
    private Tweener tweener = null;
    private Dictionary<int, CrystalHPUIItem> showDict = new Dictionary<int, CrystalHPUIItem>();
    private List<CrystalHPUIItem> unUseLst = new List<CrystalHPUIItem>();

    private void Awake()
    {
        MessageCenter.Instance.addListener(MsgCmd.On_HP_Change_Value, onHpChange);
    }

    private void Start()
    {
        this.GetComponent<Canvas>().planeDistance = 1f;
        slot = this.transform.Find("Main/Crystal").gameObject;
        slot.SetActive(false);
        blood = this.transform.Find("blood").gameObject;
        blood.SetActive(false);
        hpImg = this.transform.Find("Main/HP").GetComponent<Image>();
        DieBg = this.transform.Find("Die/DieBG").GetComponent<Image>();
        Die = this.transform.Find("Die/PlayerDie").gameObject;
        crystalBroken = this.transform.Find("Die/CrystalBroken").gameObject;
        countDownText = this.transform.Find("countText").GetComponent<Text>();
        blood.GetComponent<Image>().color = new Color(1, 0, 0, 0);
        DieBg.GetComponent<Image>().enabled = false;
        Die.SetActive(false);
        crystalBroken.SetActive(false);
        MessageCenter.Instance.addListener(MsgCmd.On_Crystal_HP_Change, onCrystalHPChange);
    }

    private void Update()
    {
        if (myCamera == null)
        {
            myCamera = Camera.main;
            if (myCamera != null)
            {
                this.GetComponent<Canvas>().worldCamera = myCamera;
            }
        }
    }

    public void onDamage()
    {
        blood.SetActive(true);
        if (tweener == null)
            tweener = blood.GetComponent<Image>().DOColor(new Color(1, 0, 0, 1), 0.8f).OnComplete(() =>
            {
                blood.GetComponent<Image>().DOColor(new Color(1, 0, 0, 0), 1f).OnComplete(() =>
                {
                    blood.SetActive(false);
                    tweener = null;
                });
            });
    }

    private void onCrystalHPChange(Message msg)
    {
        int id = (int)msg["id"];
        float hp = (float)msg["hp"];
        float orgHp = (float)msg["orgHP"];
        if (showDict.ContainsKey(id))
        {
            CrystalHPUIItem crystal = showDict[id];
            float fill = hp / orgHp;
            if (fill <= 0)
            {
                GameObject.Destroy(crystal.gameObject);
                showDict.Remove(id);
                DieBg.GetComponent<Image>().enabled = true;
                crystalBroken.SetActive(true);
            }
            else
            {
                crystal.setFill(hp / orgHp);
            }
        }
        else
        {
            GameObject go = MonoBehaviour.Instantiate(slot, slot.transform.parent) as GameObject;
            go.SetActive(true);
            CrystalHPUIItem crystal = go.AddComponent<CrystalHPUIItem>();
            crystal.setFill(hp / orgHp);
            showDict.Add(id, crystal);
        }
    }

    private void onHpChange(Message msg)
    {
        float hp = (float)msg["HP"];
        float orghp = (float)msg["OrgHP"];
        float fill = (hp / orghp) <= 0 ? 0 : hp / orghp;
        hpImg.DOFillAmount(fill, 0.2f);
        if (fill == 0)
        {
            DieBg.GetComponent<Image>().enabled = true;
            Die.SetActive(true);
        }
    }

    public void setCountDown(int count, string desc)
    {
        StartCoroutine(countDown(count, desc));
    }
    private IEnumerator countDown(int count, string desc)
    {
        countDownText.gameObject.SetActive(true);
        countDownText.text = desc + "\n" + "倒计时结束" + count;
        while (count > 0)
        {
            yield return new WaitForSeconds(1f);
            count--;
            countDownText.text = desc + "\n" + "倒计时结束" + count;
        }
        countDownText.gameObject.SetActive(false);
        yield break;
    }
}
