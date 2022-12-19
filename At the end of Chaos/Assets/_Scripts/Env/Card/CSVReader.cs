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
                if (data_value[i] == "표시이름") break;
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
                MatchCollection m = Regex.Matches(tmpCard.desc, "([가-힣 ]+) +.{0,4}?<color=.+?>[+]{0,}([-]{0,}[0-9]{1,3})");

                for (int j = 0; j < m.Count; j++)
                {
                    int _input = int.Parse(m[j].Groups[2].Value);
                    string _name = m[j].Groups[1].Value;
                    switch (m[j].Groups[1].Value)
                    {
                        //tmpCard.Selected += () =>
                        //{

                        //};
                        case "공격력":
                            tmpCard.Selected += () =>
                            {
                                GunManager.instance.damageMultiplier += _input;
                                Debug.Log(_name + " : " + _input);
                            };
                            break;

                        case "공격속도":
                            tmpCard.Selected += () =>
                            {
                                GunManager.instance.attackSpeedMultiplier += _input;
                                Debug.Log(_name + " : " + _input);
                            };
                            break;

                        case "재장전속도":
                            tmpCard.Selected += () =>
                            {
                                GunManager.instance.reloadMultiplier += _input;
                                Debug.Log(_name + " : " + _input);
                            };
                            break;

                        case "장탄수":
                            tmpCard.Selected += () =>
                            {
                                GunManager.instance.ammoMultiplier += _input;
                                Debug.Log(_name + " : " + _input);
                            };
                            break;

                        case "열차의 수":
                            tmpCard.Selected += () =>
                            {
                                GameServerManager.instance.CV(5, _input);
                                Debug.Log(_name + " : " + _input);
                            };
                            break;

                        case "최대 체력":
                            tmpCard.Selected += () =>
                            {
                                GameServerManager.instance.CV(7, _input);
                                Debug.Log(_name + " : " + _input);
                            };
                            break;

                        case "체력":
                            tmpCard.Selected += () =>
                            {
                                GameServerManager.instance.CV(6, _input);
                                Debug.Log(_name + " : " + _input);
                            };
                            break;

                        case "방어구 관통력":
                            tmpCard.Selected += () =>
                            {
                                GunManager.instance.pierceAdd += _input;
                                Debug.Log(_name + " : " + _input);
                            };
                            break;

                        case "이동속도":
                            tmpCard.Selected += () =>
                            {
                                GameServerManager.instance.player.GetComponent<PlayerMovement>().moveSpeed += _input / 10;
                                Debug.Log(_name + " : " + _input);
                            };
                            break;

                        case "자원채집배율":
                            tmpCard.Selected += () =>
                            {
                                Debug.Log(_name + " : " + _input);
                            };
                            break;

                        case "낮시간":
                            tmpCard.Selected += () =>
                            {
                                GameServerManager.instance.CV(1, _input);
                                Debug.Log(_name + " : " + _input);
                            };
                            break;

                        case "좀비의 속도":
                            tmpCard.Selected += () =>
                            {
                                GameServerManager.instance.CV(2, _input);
                                Debug.Log(_name + " : " + _input);
                            };
                            break;

                        case "나무":
                            tmpCard.Selected += () =>
                            {
                                GameServerManager.instance.CV(3, _input);
                                Debug.Log(_name + " : " + _input);
                            };
                            break;

                        case "고철":
                            tmpCard.Selected += () =>
                            {
                                GameServerManager.instance.CV(4, _input);
                                Debug.Log(_name + " : " + _input);
                            };
                            break;

                        case "치명타":
                            tmpCard.Selected += () =>
                            {
                                GameServerManager.instance.CV(8, _input);
                                Debug.Log(_name + " : " + _input);
                            };
                            break;

                        default:
                            break;
                    }
                }
            } else if (tmpCard.type == 3)
            {
                tmpCard.Selected += () =>
                {
                    Debug.Log("총 변경");
                };
            }
            else
            {
                if (tmpCard.cardCode == 3)
                {
                    tmpCard.Selected += () =>
                    {
                        CardManager.instance.remainWoodI += UnityEngine.Random.Range(0, 12);
                        CardManager.instance.remainWood.text = CardManager.instance.remainWoodI.ToString();
                         CardManager.instance.remainIronI += UnityEngine.Random.Range(0, 12);
                        CardManager.instance.remainIron.text = CardManager.instance.remainIronI.ToString();
                    };
                }
                //else if (tmpCard.cardCode == 10)
                //{
                //    tmpCard.Selected += () =>
                //    {
                //        Debug.Log("미할당");
                //    };
                //} else if 
                
            }
            

            cMgr.deck.Add(tmpCard);
        }
    }

    void Update()
    {
        
    }
}
