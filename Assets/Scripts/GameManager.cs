﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    //싱글톤 패턴을 위한 오브젝트
    public static GameManager instance=null;
    //캐릭터의 상태
    //0 : 기본 상태
    //1 : 자물쇠 열기
    //2 : 환경설정
    private int charactorStatus;
    //환경설정 변수들
    private float volumeEffect;
    private float volumeBGM;
    private float mouseSensitivity;
    //
    private int[] insertFuse=new int[3];
    //배경음악 AudioSource를 가지고 있는 오브젝트
    public GameObject charactor;
    public GameObject audioSourceCam;
    public GameObject audioSourcePhone;
    //머테리얼
    public Material materialMonitor;
    public Material materialScreen;
    //효과음
    public AudioClip audioClipTurnOnComputer;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        charactorStatus = 0;

        if (PlayerPrefs.HasKey("volumeBGM") == false)
        {
            this.volumeBGM = 0.5f;
        }
        else
        {
            this.volumeBGM = PlayerPrefs.GetFloat("volumeBGM");
        }

        if (PlayerPrefs.HasKey("volumeEffect") == false)
        {
            this.volumeEffect = 0.5f;
        }
        else
        {
            this.volumeEffect = PlayerPrefs.GetFloat("volumeEffect");
        }

        if (PlayerPrefs.HasKey("mouseSensitivity") == false)
        {
            this.mouseSensitivity = 60f;
        }
        else
        {
            this.mouseSensitivity = PlayerPrefs.GetFloat("mouseSensitivity");
        }
        //Application.LoadLevelAdditive("ClassRoom");
        audioSourcePhone = GameObject.FindGameObjectWithTag("Smartphone");
        setVolumes();
    }

    // Update is called once per frame
    private void Update()
    {
        if (charactorStatus == 0)
        {
            charactor.GetComponent<Charactor>().mouseClicked();
            charactor.GetComponent<Charactor>().keyboardPushed();
            charactor.GetComponent<Charactor>().controlWalkingSound();
        }
        else if (charactorStatus == 1)
        {
            this.GetComponent<Lock>().mouseClicked();
            this.GetComponent<Lock>().keyboardPushed();
        }
        else if (charactorStatus == 2)
        {
            this.GetComponent<Settings>().mouseClicked();
            this.GetComponent<Settings>().keyboardPushed();
        }
    }
    void FixedUpdate () {
        if (charactorStatus == 0)
        {
            charactor.GetComponent<Charactor>().charMove();
        }
	}

    public void playSfx(Vector3 pos, AudioClip sfx)
    {
        if (volumeEffect == 0f) return;
        //AudioSource를 담을 오브젝트 생성
        GameObject soundObj = new GameObject("Sfx");
        soundObj.transform.position = pos;

        //AudioSource 컴포넌트를 방금 생성한 오브젝트에 추가
        AudioSource audioSource = soundObj.AddComponent<AudioSource>();
        //AudioSource 속성설정
        audioSource.clip = sfx;
        audioSource.minDistance = 2f;
        audioSource.maxDistance = 4f;
        audioSource.volume = volumeEffect;
        //효과음 틀고 끝나면 오브젝트 삭제
        audioSource.Play();
        Destroy(soundObj, sfx.length);
    }

    public void setVolumes()
    {
        charactor.GetComponent<AudioSource>().volume = volumeEffect;
        audioSourceCam.GetComponent<AudioSource>().volume = volumeBGM;
        if (audioSourcePhone != null) audioSourcePhone.GetComponent<AudioSource>().volume = volumeEffect;
    }

    public void checkFuse()
    {
        GameObject[] monitor;
        //퓨즈가 맞게 끼워졌을 경우 컴퓨터 켜짐
        if (insertFuse[0]==2 && insertFuse[1]==3 && insertFuse[2] == 1)
        {
            monitor = GameObject.FindGameObjectsWithTag("Monitor");
            foreach (GameObject m in monitor)
            {
                m.GetComponent<MeshRenderer>().material = materialMonitor;
                playSfx(m.transform.position, audioClipTurnOnComputer);
            }
        }
    }

    public int getCharactorStatus()
    {
        return this.charactorStatus;
    }
    public float getVolumeBGM()
    {
        return this.volumeBGM;
    }
    public float getVolumeEffect()
    {
        return this.volumeEffect;
    }
    public float getMouseSensitivity()
    {
        return this.mouseSensitivity;
    }

    public void setCharactorStatus(int _charactorStatus)
    {
        this.charactorStatus = _charactorStatus;
    }
    public void setVolumeBGM(float _volumeBGM)
    {
        this.volumeBGM = _volumeBGM;
    }
    public void setVolumeEffect(float _volumeEffect)
    {
        this.volumeEffect = _volumeEffect;
    }
    public void setMouseSensitivity(float _mouseSensitivity)
    {
        this.mouseSensitivity = _mouseSensitivity;
    }
    public void setInsertFuse(int index, int color)
    {
        this.insertFuse[index] = color;
    }
}
