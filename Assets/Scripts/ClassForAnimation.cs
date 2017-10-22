using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassForAnimation : MonoBehaviour {
    //문열림 상태 저장변수
    private bool isOpened;
    //자신의 애니메이션을 실행할 애니메이터를 가진 오브젝트
    public GameObject objAnimator;
    //문 열고 닫는 사운드 클립
    public AudioClip audioClipOpen;
    public AudioClip audioClipClose;
    //오브젝트별로 추가로 사용가능한 오브젝트
    public GameObject[] objAdditional;

	// Use this for initialization
	void Start () {
        isOpened = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void playAnimation()
    {
        if (isOpened)
        {
            objAnimator.GetComponent<Animator>().SetBool("isOpened", false);
            GameManager.instance.playSfx(this.transform.position, audioClipClose);
            isOpened = false;
        }
        else
        {
            objAnimator.GetComponent<Animator>().SetBool("isOpened", true);
            GameManager.instance.playSfx(this.transform.position, audioClipOpen);
            isOpened = true;
        }
    }
}
