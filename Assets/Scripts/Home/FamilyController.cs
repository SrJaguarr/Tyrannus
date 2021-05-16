using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FamilyController : MonoBehaviour
{
    public FamilyDB familyDB;

    private Dictionary<Familiar, GameObject> familiars;

    public Transform familiarContainer;

    private ShopListController shopListController;
    private NeedsLoader needsLoader;

    private void Awake()
    {
        familiars = new Dictionary<Familiar, GameObject>();
        shopListController = GameManager._instance.shopListController;
        needsLoader = GameManager._instance.needsLoader;
    }

    public void NewGame()
    {
        foreach(KeyValuePair<Familiar, GameObject> familiar in familiars)
        {
            Destroy(familiar.Value);
        }

        familiars.Clear();

        AddFamiliar(0); //Metemos al presidente unicamente
    }

    public Dictionary<Familiar, GameObject> GetAllFamiliarsInGame()
    {
        return familiars;
    }


    public Familiar GetFamiliar(int n)
    {
        return familyDB.familiars[n];
    }

    public GameObject GetFamiliarButton(int n)
    {
        return familiars[familyDB.familiars[n]];
    }

    //President = 0, Claudia = 1, Julia = 2, Viriato = 3, Kalinka = 4
    public void AddFamiliar(int n)
    {
        familiars.Add(familyDB.familiars[n], Instantiate(familyDB.familiars[n].prefab, familiarContainer));    //Instanciamos todos los familiares determinados 

        if (n != 0)
            familiars[familyDB.familiars[n]].GetComponent<FamiliarButton>().InitializeSelf(familyDB.familiars[n]);

        shopListController.AddFamiliarToShopList(familyDB.familiars[n]);
    }

    public void AddSchoolPenalty(float penalty)
    {
        familyDB.familiars[2].schoolPenalty = familyDB.familiars[3].schoolPenalty = familyDB.familiars[4].schoolPenalty = penalty;
    }

    public void RemoveSchoolPenalty()
    {
        familyDB.familiars[2].schoolPenalty = familyDB.familiars[3].schoolPenalty = familyDB.familiars[4].schoolPenalty = 0;
    }

    public void CheckNeeds()
    {
        CheckHeat();

        foreach(KeyValuePair<Familiar, GameObject> familiar in familiars)
        {
            if (shopListController.IsPillOn(familiar.Key))
            {
                if (familiar.Key.isIll)
                    needsLoader.RemoveNeed(familiar.Key.fullName, "ill");

                familiar.Key.isIll = false;
                familiar.Key.daysIll = 0;
                familiar.Key.illProbability = 0.1f;
                shopListController.ShowPills(familiar.Key, false);
            }

            if (shopListController.IsFoodOn(familiar.Key))
            {
                if (familiar.Key.isHungry)
                    needsLoader.RemoveNeed(familiar.Key.fullName, "hungry");

                familiar.Key.isHungry = false;
                familiar.Key.daysHungry = 0;
            }
        }
    }

    private void CheckHeat()
    {
        if (!shopListController.toggleHeating.isOn)
        {
            foreach (KeyValuePair<Familiar, GameObject> familiar in familiars)
            {
                if (familiar.Key.isCold)
                    needsLoader.RemoveNeed(familiar.Key.fullName, "cold");

                familiar.Key.isCold = false;
                familiar.Key.daysCold = 0;
               
            }
        }
    }

    public void UpdateNeeds()
    {
        foreach (KeyValuePair<Familiar, GameObject> familiarPair in familiars)
        {
            if (familiarPair.Value.activeSelf)
            {
                Familiar familiar = familiarPair.Key;
                Transform needsParent = familiarPair.Value.transform.GetChild(0);

                if (familiar.isCold)
                {
                    if (familiar.daysCold == 0)
                        needsLoader.AddNeed("cold", needsParent, familiar.fullName);

                    familiar.daysCold++;
                    familiar.illProbability += 0.05f;
                }
                else
                {
                    familiar.isCold = true;
                }

                if (familiar.isHungry)
                {
                    if (familiar.daysHungry == 0)
                        needsLoader.AddNeed("hungry", needsParent, familiar.fullName);

                    familiar.daysHungry++;
                    familiar.illProbability += 0.05f;
                }
                else
                {
                    familiar.isHungry = true;
                }

                if (!familiar.isIll)
                {
                    if (Random.value < familiar.illProbability)                                 //Probabilidad de enfermar
                    {
                        familiar.isIll = true;                                                  //Si enferma se le coloca el icono de enfermedad

                        needsLoader.AddNeed("ill", needsParent, familiar.fullName);
                        shopListController.ShowPills(familiar, true);
                    }
                }
                else
                {
                    familiar.daysIll++;
                }

                //Se comprueban los dias que dura la necesidad para comprobar si el familiar sigue con vida

                if (familiar.daysCold == familiar.maxDaysCold || familiar.daysHungry == familiar.maxDaysHungry || familiar.daysIll == familiar.maxDaysIll)
                {
                    if (familiar.isCold)
                        needsLoader.RemoveNeed(familiar.fullName, "cold");

                    if (familiar.isHungry)
                        needsLoader.RemoveNeed(familiar.fullName, "hungry");

                    if (familiar.isIll)
                        needsLoader.RemoveNeed(familiar.fullName, "ill");

                    shopListController.FamiliarDeletion(familiar);
                    familiarPair.Value.SetActive(false);
                    GameManager._instance.notificationManager.DeathNotification(familiarPair.Key);
                }

                familiar.enjoyedCompany = false;
            }
        }

    }


    

}