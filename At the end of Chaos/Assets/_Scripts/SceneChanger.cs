using Photon.Pun.Demo.SlotRacer.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Color = UnityEngine.Color;

public class SceneChanger : MonoBehaviour
{
    public UnityEngine.UI.Image img;

    public bool isLobby;

    public GameObject top, bottom;

    float time = 0f;

    Color color = Color.black; //���� ��
    float fadeTime = 5f; // ��� ������� �ð�
    float start = -5f; //���۽� ����
    float end = 25f; //���� ����
    float easing = 2f; // x < 1 = ease in , x > 1 ease out x = 1 linear  ///  fadeTime�̶� ������ �ð�
    void Start()
    {
        StartCoroutine(Open());
    }

    void Update()
    {
        
    }

    IEnumerator Open()
    {
        yield return new WaitForSeconds(2f);
        if (isLobby) yield return new WaitForSeconds(2f);
        while (color.a > 0f)
        {
            time += Time.deltaTime / fadeTime;
            color.a = Mathf.Lerp(1, 0, time);

            if (isLobby)
            {
                Camera.main.transform.rotation = Quaternion.Euler(
                start + (end - start) * (1 - Mathf.Pow(1 - time, easing))
                , 10, 0);
            }

            img.color = color;
            yield return null;
        }
    }

    IEnumerator Close()
    {
        time = 0;
        fadeTime = 1f;
        AudioSource audio = GetComponent<AudioSource>();

        while (time <= fadeTime)
        {
            time += Time.deltaTime / fadeTime;
            color.a = Mathf.Lerp(0, 1, time / fadeTime);

            if (isLobby) audio.volume = Mathf.Lerp(1, 0, time / fadeTime);


            img.color = color;
            yield return null;
        }
        if (isLobby)
        {
            yield return new WaitForSeconds(2f);
            GetComponent<Lobby>().load.allowSceneActivation = true;
        }
            
    }

    public void ChangeScene()
    {
        StartCoroutine(Close());
    }
}
