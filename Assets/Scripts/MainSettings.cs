﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSettings : MonoBehaviour {

    //UI캔버스 오브젝트
    public GameObject defaultUI;
    public GameObject cuttonUI;
    public GameObject menuUI;
    public GameObject settingsUI;
    public GameObject keyUI;
    public GameObject creditUI;
    public GameObject loadingUI;
    public GameObject[] menuText = new GameObject[4];
    public GameObject[] settingsText = new GameObject[4];
    public GameObject[] settingsSlider = new GameObject[3];
    private int menuPosition;
    private int[] textPosition = new int[2];
    
    //환경설정 변수들
    private float volumeBGM;
    private float volumeEffect;
    private float mouseSensitivity;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    //초기화 메소드
    public void initialization()
    {
        if (PlayerPrefs.HasKey("volumeBGM") == true)
        {
            volumeBGM = MainManager.instance.getVolumeBGM();
        }
        else
        {
            PlayerPrefs.SetFloat("volumeBGM", 0.5f);
            MainManager.instance.setVolumeBGM(0.5f);
            volumeBGM = 0.5f;
        }

        if (PlayerPrefs.HasKey("volumeEffect") == true)
        {
            volumeEffect = MainManager.instance.getVolumeEffect();
        }
        else
        {
            PlayerPrefs.SetFloat("volumeEffect", 0.5f);
            MainManager.instance.setVolumeEffect(0.5f);
            volumeEffect = 0.5f;
        }

        if (PlayerPrefs.HasKey("mouseSensitivity") == true)
        {
            mouseSensitivity = PlayerPrefs.GetFloat("mouseSensitivity");
        }
        else
        {
            PlayerPrefs.SetFloat("mouseSensitivity", 60f);
            MainManager.instance.setMouseSensitivity(60f);
            mouseSensitivity = 60f;
        }

        settingsSlider[0].GetComponent<Slider>().value = volumeBGM;
        settingsSlider[1].GetComponent<Slider>().value = volumeEffect;
        settingsSlider[2].GetComponent<Slider>().value = mouseSensitivity;

        cuttonOnOff(true);
        menuOnOff(true);
    }

    public void returnToGame()
    {
        PlayerPrefs.SetFloat("volumeBGM", volumeBGM);
        PlayerPrefs.SetFloat("volumeEffect", volumeEffect);
        PlayerPrefs.SetFloat("mouseSensitivity", mouseSensitivity);
        MainManager.instance.setVolumeBGM(volumeBGM);
        MainManager.instance.setVolumeEffect(volumeEffect);
        MainManager.instance.setMouseSensitivity(mouseSensitivity);

        MainManager.instance.setVolumes();

        cuttonOnOff(false);
        menuOnOff(false);
        MainManager.instance.mainOnOff(true);

        MainManager.instance.setCharactorStatus(0);
    }

    //마우스가 눌렸을 때 실행될 메소드
    public void mouseClicked()
    {
        //마우스 왼쪽 버튼이 눌렸을 때
        if (Input.GetMouseButtonDown(0))
        {
            if (menuPosition == 0)
            {
                if (textPosition[0] == 0)
                {
                    settingsOnOff(true);
                }
                else if (textPosition[0] == 1)
                {
                    keyOnOff(true);
                }
                else if (textPosition[0] == 2)
                {
                    creditOnOff(true);
                }
                else if (textPosition[0] == 3)
                {
                    returnToGame();
                }
            }
            else if (menuPosition == 1)
            {
                if (textPosition[1] == 3)
                {
                    menuOnOff(true);
                }
            }
            else if (menuPosition == 2 || menuPosition == 3)
            {
                menuOnOff(true);
            }
        }

        //마우스 오른쪽 버튼이 눌렸을 때
        if (Input.GetMouseButtonDown(1))
        {
            if (menuPosition == 0)
            {
                returnToGame();
            }
            else
            {
                menuOnOff(true);
            }
        }
    }

    //키보드가 눌렸을 때 실행될 메소드
    public void keyboardPushed()
    {
        //W가 눌렸을 때, 현재 위치의 숫자를 1 뺌
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (menuPosition == 0 || menuPosition == 1)
            {
                if (textPosition[menuPosition] > 0)
                {
                    textPosition[menuPosition]--;
                    movePosition();
                }
            }

        }
        //S가 눌렸을 때, 현재 위치의 숫자를 1 더함
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (menuPosition == 0)
            {
                if (textPosition[menuPosition] < 3)
                {
                    textPosition[menuPosition]++;
                    movePosition();
                }
            }
            else if (menuPosition == 1)
            {
                if (textPosition[menuPosition] < 3)
                {
                    textPosition[menuPosition]++;
                    movePosition();
                }
            }
        }
        //A가 눌렸을 때, 왼쪽으로 한칸 이동
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (menuPosition == 1)
            {
                if (textPosition[1] == 0 && volumeBGM > 0f)
                {
                    volumeBGM -= 0.1f;
                    settingsSlider[0].GetComponent<Slider>().value = volumeBGM;
                }
                if (textPosition[1] == 1 && volumeEffect > 0f)
                {
                    volumeEffect -= 0.1f;
                    settingsSlider[1].GetComponent<Slider>().value = volumeEffect;
                }
                if (textPosition[1] == 2 && mouseSensitivity > 0)
                {
                    mouseSensitivity -= 10f;
                    settingsSlider[2].GetComponent<Slider>().value = mouseSensitivity;
                }
            }
        }
        //D가 눌렸을 때, 오른쪽으로 한칸 이동
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (menuPosition == 1)
            {
                if (textPosition[1] == 0 && volumeBGM < 1f)
                {
                    volumeBGM += 0.1f;
                    settingsSlider[0].GetComponent<Slider>().value = volumeBGM;
                }
                if (textPosition[1] == 1 && volumeEffect < 1f)
                {
                    volumeEffect += 0.1f;
                    settingsSlider[1].GetComponent<Slider>().value = volumeEffect;
                }
                if (textPosition[1] == 2 && mouseSensitivity < 100)
                {
                    mouseSensitivity += 10f;
                    settingsSlider[2].GetComponent<Slider>().value = mouseSensitivity;
                }
            }
        }
        //ESC를 눌렀을 때
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuPosition == 0)
            {
                returnToGame();
            }
            else
            {
                menuOnOff(true);
            }
        }
    }

    //cutton,default UI화면 on/off하는 메소드
    public void cuttonOnOff(bool isSet)
    {
        cuttonUI.GetComponent<Canvas>().enabled = isSet;
        defaultUI.GetComponent<Canvas>().enabled = !isSet;
    }

    //메인메뉴 UI화면 on/off하는 메소드
    public void menuOnOff(bool isSet)
    {
        menuUI.GetComponent<Canvas>().enabled = isSet;
        //isSet이 true일 경우 초기화
        if (isSet)
        {
            menuPosition = 0;
            textPosition[0] = 0;
            menuText[0].GetComponent<Text>().fontSize = 60;
            menuText[0].GetComponent<Text>().color = new Color(155 / 255f, 155 / 255f, 0);
            for (int i = 1; i < 4; i++)
            {
                menuText[i].GetComponent<Text>().fontSize = 50;
                menuText[i].GetComponent<Text>().color = new Color(158 / 255f, 158 / 255f, 158 / 255f);
            }
            settingsOnOff(false);
            keyOnOff(false);
            creditOnOff(false);
        }
    }
    //환경설정메뉴 UI화면 on/off하는 메소드
    public void settingsOnOff(bool isSet)
    {
        settingsUI.GetComponent<Canvas>().enabled = isSet;
        //isSet이 true일 경우 초기화
        if (isSet)
        {
            menuPosition = 1;
            textPosition[1] = 0;
            settingsText[0].GetComponent<Text>().fontSize = 60;
            settingsText[0].GetComponent<Text>().color = new Color(155 / 255f, 155 / 255f, 0);
            for (int i = 1; i < 4; i++)
            {
                settingsText[i].GetComponent<Text>().fontSize = 50;
                settingsText[i].GetComponent<Text>().color = new Color(158 / 255f, 158 / 255f, 158 / 255f);
            }
            settingsSlider[0].GetComponent<Slider>().value = volumeBGM;
            settingsSlider[1].GetComponent<Slider>().value = volumeEffect;
            settingsSlider[2].GetComponent<Slider>().value = mouseSensitivity;

            menuOnOff(false);
            keyOnOff(false);
            creditOnOff(false);
        }
    }

    //조작키설명메뉴 UI화면 on/off하는 메소드
    public void keyOnOff(bool isSet)
    {
        keyUI.GetComponent<Canvas>().enabled = isSet;
        //isSet이 true일 경우 초기화
        if (isSet)
        {
            menuPosition = 2;
            menuOnOff(false);
            settingsOnOff(false);
            creditOnOff(false);
        }
    }

    //크레딧메뉴 UI화면 on/off하는 메소드
    public void creditOnOff(bool isSet)
    {
        creditUI.GetComponent<Canvas>().enabled = isSet;
        //isSet이 true일 경우 초기화
        if (isSet)
        {
            menuPosition = 3;
            menuOnOff(false);
            settingsOnOff(false);
            keyOnOff(false);
        }
    }

    public void movePosition()
    {
        if (menuPosition == 0)
        {
            for (int i = 0; i < 4; i++)
            {
                if (textPosition[menuPosition] == i) continue;
                menuText[i].GetComponent<Text>().fontSize = 50;
                menuText[i].GetComponent<Text>().color = new Color(158 / 255f, 158 / 255f, 158 / 255f);
            }
            menuText[textPosition[menuPosition]].GetComponent<Text>().fontSize = 60;
            menuText[textPosition[menuPosition]].GetComponent<Text>().color = new Color(155 / 255f, 155 / 255f, 0);
        }
        else if (menuPosition == 1)
        {
            for (int i = 0; i < 4; i++)
            {
                if (textPosition[menuPosition] == i) continue;
                settingsText[i].GetComponent<Text>().fontSize = 50;
                settingsText[i].GetComponent<Text>().color = new Color(158 / 255f, 158 / 255f, 158 / 255f);
            }
            settingsText[textPosition[menuPosition]].GetComponent<Text>().fontSize = 60;
            settingsText[textPosition[menuPosition]].GetComponent<Text>().color = new Color(155 / 255f, 155 / 255f, 0);
        }
    }
}
