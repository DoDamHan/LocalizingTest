using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public const string const_str_LocalLangKey = "LocalLangKey";

    private void Awake()
    {
        StartCoroutine(nameof(OnLoad_Local_SaveData));
    }

    /// <summary>
    /// 로컬에 저장된 데이터를 읽어온다.
    /// </summary>
    /// <returns></returns>
    private IEnumerable OnLoad_Local_SaveData()
    {
        //if (ELanguageKey.NONE == DoLoad_LangKey())
        //{
        //    Debug.Log("저정된 언어 데이터가 없다. 기기의 로컬 언어 정보로 저장한다.");
        //}
        yield return null;
    }

    #region Save_Functions
    public void DoSave_LangKey(ELanguageKey eLangKey)
    {
        PlayerPrefs.SetInt(const_str_LocalLangKey, (int)eLangKey);
    }
    #endregion Save_Functions

    #region Load_Functions
    public ELanguageKey DoLoad_LangKey()
    {
        ELanguageKey eLangKey = ELanguageKey.NONE;
        eLangKey = (ELanguageKey)PlayerPrefs.GetInt(const_str_LocalLangKey, -1);

        return eLangKey;
    }
    #endregion Load_Functions
}
