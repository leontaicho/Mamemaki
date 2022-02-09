using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleAction : MonoBehaviour
{
    enum TitleState
    {
        Init,
        Fade,
        Logo,
        LogoFade,
        Menu,
        Tutorial,
        Load,
    };
    TitleState state;

    public Image Fade;
    public GameObject Logo;
    public GameObject Menu;
    public Image Menu_1;
    public Image Menu_12;
    public Image Menu_2;
    public Image Menu_22;
    public Image Tutorial;
    Animator FadeLogo;
    Animator FadeTutorial;

    float Alpha = 1.0f;
    int Select = 10;

    // Start is called before the first frame update
    void Start()
    {
        FadeLogo = Logo.gameObject.GetComponent<Animator>();
        FadeTutorial = Tutorial.gameObject.GetComponent<Animator>();

        TitleInit();
    }

    void TitleInit()
    {
        Fade.gameObject.SetActive(true);
        Logo.gameObject.SetActive(true);
        Tutorial.gameObject.SetActive(false);
        Menu.gameObject.SetActive(false);

        state = TitleState.Fade;
        Alpha = 1.0f;
        Select = 0;
    }

    void FadeLogoFinish()
    {
        Fade.gameObject.SetActive(false);
        Logo.gameObject.SetActive(false);
        Menu.gameObject.SetActive(true);

        Menu_1.gameObject.SetActive(false);
        Menu_12.gameObject.SetActive(true);
        Menu_2.gameObject.SetActive(false);
        Menu_22.gameObject.SetActive(true);
    }

    void FadeMenuFinish()
    {
        state = TitleState.Menu;
    }

    void FadeFinish()
    {
        state = TitleState.Tutorial;
    }

    void FadeTutorialFinish()
    {
        Tutorial.gameObject.SetActive(false);
        state = TitleState.Menu;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case TitleState.Init:
                TitleInit();
                break;

            case TitleState.Fade:

                Alpha -= 0.005f;
                Fade.color = new Color(1.0f, 1.0f, 1.0f, Alpha);

                if(Alpha <= 0)
                {
                    state = TitleState.Logo;
                    Alpha = 0;
                }
                break;

            case TitleState.Logo:
                if(Input.GetButtonDown("BtnA"))
                {
                    state = TitleState.LogoFade;
                }
                break;

            case TitleState.LogoFade:
                FadeLogo.SetTrigger("FadeLogo");
                Invoke("FadeLogoFinish", 5.1f);
                Invoke("FadeMenuFinish", 1.4f);
                break;

            case TitleState.Menu:
                if(Input.GetAxis("HorizontalPad") <= -1 || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    Select = 0;
                    Menu_1.gameObject.SetActive(true);
                    Menu_12.gameObject.SetActive(false);
                    Menu_2.gameObject.SetActive(false);
                    Menu_22.gameObject.SetActive(true);
                }
                else if(Input.GetAxis("HorizontalPad") >= 1 || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    Select = 1;
                    Menu_1.gameObject.SetActive(false);
                    Menu_12.gameObject.SetActive(true);
                    Menu_2.gameObject.SetActive(true);
                    Menu_22.gameObject.SetActive(false);
                }

                if (Input.GetButtonDown("BtnA"))
                {
                    switch(Select)
                    {
                        case 0:
                            Fade.gameObject.SetActive(true);
                            state = TitleState.Load;
                            break;

                        case 1:
                            Tutorial.gameObject.SetActive(true);
                            Invoke("FadeFinish", 1.0f);
                            break;
                    }
                }
                break;

            case TitleState.Tutorial:
                if (Input.GetButtonDown("BtnA"))
                {
                    FadeTutorial.SetTrigger("Fade");
                    Invoke("FadeTutorialFinish", 1.0f);
                }
                break;

            case TitleState.Load:
                Fade.color = new Color(1.0f, 1.0f, 1.0f, Alpha);
                Alpha += 0.005f;
                if (Alpha >= 1)
                {
                    state = TitleState.Init;
                }
                break;
        }
    }
}
