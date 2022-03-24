using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollection : MonoBehaviour
{
    public GameObject moneyStack;
    public GameObject moneyObj;
    public int moneyStackWidth = 70;
    private float moneyStackX = 0f;
    private float moneyStackY = 0f;
    //private int banknoteValue = 10;
    //private int totalMoney = 0;

    private int banknoteValue = 10;

    public int BanknoteValue
    {
        get { return banknoteValue; }
        set { banknoteValue = value; }
    }


    private int totalMoney = 0;

    public int TotalMoney
    {
        get { return totalMoney; }
        set { totalMoney = value; }
    }



    private void OnCollisionEnter(Collision collision)
    {
        var collectible = collision.gameObject.GetComponent<Collectible>();

        if (collectible == null || collectible.isDestroyed == true) return;
        int moneyValue = collectible.value;
        collectible.isDestroyed = true;
        Destroy(collision.gameObject);
        if (moneyValue > 0)
            AddMoney2MoneyStack(moneyValue / banknoteValue);
        else
            RemoveMoneyFromMoneyStack(-moneyValue / banknoteValue);
    }

    public void AddMoney2MoneyStack(int banknoteCount)
    {
        for (int i = 0; i < banknoteCount; i++)
        {
            moneyStackX = ((totalMoney % moneyStackWidth) / banknoteValue) * moneyObj.transform.localScale.z + moneyStack.transform.position.z;
            moneyStackY = Convert.ToInt32(totalMoney / moneyStackWidth) * moneyObj.transform.localScale.y + moneyStack.transform.position.y;
            Instantiate(moneyObj, new Vector3(moneyStack.transform.position.x, moneyStackY, moneyStackX), Quaternion.identity, moneyStack.transform);
            totalMoney += banknoteValue;
        }
    }

    public void RemoveMoneyFromMoneyStack(int banknoteCount)
    {
        List<Transform> banknotes = new List<Transform>();

        foreach (Transform banknote in moneyStack.transform)
        {
            banknotes.Add(banknote);
        }

        for (int i = 0; i < banknoteCount; i++)
        {
            if (totalMoney <= 0) return;
            Destroy(banknotes[banknotes.Count - 1].gameObject);
            banknotes.RemoveAt(banknotes.Count - 1);
            totalMoney -= banknoteValue;
        }
    }

}
