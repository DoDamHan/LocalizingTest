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
    /// ���ÿ� ����� �����͸� �о�´�.
    /// </summary>
    /// <returns></returns>
    private IEnumerable OnLoad_Local_SaveData()
    {
        //if (ELanguageKey.NONE == DoLoad_LangKey())
        //{
        //    Debug.Log("������ ��� �����Ͱ� ����. ����� ���� ��� ������ �����Ѵ�.");
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
