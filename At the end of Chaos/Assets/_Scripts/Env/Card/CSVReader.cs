using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

public delegate void Dele();

[Serializable] public class CardDef
{
    public Sprite img;
    public string title;
    public string desc;
    public int cardCode;
    public int resWood;
    public int resIron;
    public bool reuseable;
    public int rank;
    public int type;
    public Dele Selected;
}
public class CSVReader : MonoBehaviour
{

    //StreamReader sReader;
    TextAsset cardDefCsv;
    bool endOfFile = false;

    [SerializeField] CardDef tmpCard;
    CardManager cMgr;
    void Start()
    {
        cMgr = GameServerManager.instance.GetComponent<CardManager>();
        cardDefCsv = Resources.Load<TextAsset>("CardDef");
        StringReader sReader = new StringReader(cardDefCsv.text); 
        //sReader = new StreamReader("Assets/Resources/CardDef.csv");
        while (!endOfFile)
        {
            string data = sReader.ReadLine();
            if (data == null)
            {
                endOfFile= true;
                break;
            }
            var data_value = data.Split(',');
            tmpCard = new CardDef();
            for (int i = 0; i < data_value.Length; i++)
            {
                if (data_value[i] == "ǥ���̸�") break;
                switch (i)
                {
                    case 0:
                        tmpCard.title = data_value[i];
                        break;

                    case 1:
                        tmpCard.cardCode = int.Parse(data_value[i]);
                        break;

                    case 2:
                        tmpCard.desc = data_value[i];
                        break;

                    case 3:
                        tmpCard.resWood = int.Parse(data_value[i]);
                        break;

                    case 4:
                        tmpCard.resIron = int.Parse(data_value[i]);
                        break;

                    case 5:
                        tmpCard.reuseable = bool.Parse(data_value[i]);
                        break;

                    case 6:
                        tmpCard.rank = int.Parse(data_value[i]);
                        break;

                    case 7:
                        tmpCard.type = int.Parse(data_value[i]);
                        break;

                    default:    
                        break;
                }

            }
            if (tmpCard.type == 1)
            {
                MatchCollection m = Regex.Matches(tmpCard.desc, "([��-�R ]+) .{0,4}?<color=.+?>([+-x]?[0-9]{1,3})");

                for (int j = 0; j < m.Count; j++)
                {
                    int _input = int.Parse(m[j].Groups[2].Value);
                    string _name = m[j].Groups[1].Value;
                    switch (m[j].Groups[1].Value)
                    {
                        //tmpCard.Selected += () =>
                        //{

                        //};
                        case "���ݷ�":
                            tmpCard.Selected += () =>
                            {
                                GunManager.instance.damageMultiplier += _input;
                                Debug.Log(_name + " : " + _input);
                            };
                            break;

                        case "���ݼӵ�":
                            tmpCard.Selected += () =>
                            {
                                GunManager.instance.attackSpeedMultiplier += _input;
                                Debug.Log(_name + " : " + _input);
                            };
                            break;

                        case "�������ӵ�":
                            tmpCard.Selected += () =>
                            {
                                GunManager.instance.reloadMultiplier += _input;
                                Debug.Log(_name + " : " + _input);
                            };
                            break;

                        case "��ź��":
                            tmpCard.Selected += () =>
                            {
                                GunManager.instance.ammoMultiplier += _input;
                                Debug.Log(_name + " : " + _input);
                            };
                            break;

                        case "������ ��":
                            tmpCard.Selected += () =>
                            {
                                GameManager.instance.trainCount += _input;
                                if (GameManager.instance.trainCount < 1)
                                {
                                    Debug.Log("������ �ϳ��� �����!");
                                }
                                Debug.Log(_name + " : " + _input);
                            };
                            break;

                        case "�ִ� ü��":
                            tmpCard.Selected += () =>
                            {
                                TrainManager.instance.healthMultiplier += _input;
                                for (int i = 1; i <= 5; i++)
                                {
                                    TrainManager.instance.GetTrain(i).GetComponent<Train>().maxHealth =
                                    TrainManager.instance.maxHealth * TrainManager.instance.healthMultiplier / 100;

                                    TrainManager.instance.GetTrain(i).GetComponent<Train>().Hp =
                                    Math.Clamp(TrainManager.instance.GetTrain(i).GetComponent<Train>().Hp,
                                    0,
                                    TrainManager.instance.GetTrain(i).GetComponent<Train>().maxHealth);
                                }
                                Debug.Log(_name + " : " + _input);
                            };
                            break;

                        case "�� �����":
                            tmpCard.Selected += () =>
                            {
                                GunManager.instance.pierceAdd += _input;
                                Debug.Log(_name + " : " + _input);
                            };
                            break;

                        case "�̵��ӵ�":
                            tmpCard.Selected += () =>
                            {
                                GameServerManager.instance.player.GetComponent<PlayerMovement>().moveSpeed *= _input;
                                Debug.Log(_name + " : " + _input);
                            };
                            break;

                        case "�ڿ�ä������":
                            tmpCard.Selected += () =>
                            {
                                Debug.Log(_name + " : " + _input);
                            };
                            break;

                        case "���ð�":
                            tmpCard.Selected += () =>
                            {
                                GameManager.instance.timeAfternoonValue += _input;
                                GameManager.instance.wfs_Afternoon = new WaitForSeconds(GameManager.instance.timeAfternoonValue);
                                Debug.Log(_name + " : " + _input);
                            };
                            break;

                        case "������ �ӵ�":
                            tmpCard.Selected += () =>
                            {
                                ZombieManager.instance.speedMultiplier *= 1.25f;
                                Debug.Log(_name + " : " + _input);
                            };
                            break;

                        case "����":
                            tmpCard.Selected += () =>
                            {
                                CardManager.instance.remainWoodI += 10;
                                CardManager.instance.remainWood.text = CardManager.instance.remainWoodI.ToString();
                                Debug.Log(_name + " : " + _input);
                            };
                            break;

                        case "��ö":
                            tmpCard.Selected += () =>
                            {
                                CardManager.instance.remainIronI += 10;
                                CardManager.instance.remainIron.text = CardManager.instance.remainIronI.ToString();
                                Debug.Log(_name + " : " + _input);
                            };
                            break;

                        default:
                            break;
                    }
                }
            } else
            {
                tmpCard.Selected += () =>
                {
                    Debug.Log("���Ҵ�");
                };
            }
            

            cMgr.deck.Add(tmpCard);
        }
    }

    void Update()
    {
        
    }
}
