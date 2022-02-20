using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_Select_Lang : MonoBehaviour
{
    [SerializeField]
    private ELanguageKey eLanguageKey = ELanguageKey.NONE;

    public void DoClick_Button()
    {
        LanguageManager.DoChange_LocalLang(eLanguageKey);
    }


}
