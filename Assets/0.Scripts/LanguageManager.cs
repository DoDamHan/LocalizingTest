using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine.Events;


public class LanguageManager : Singleton<LanguageManager>
{
    const string langURL = "https://docs.google.com/spreadsheets/d/1dSBoEg3cfCnIDrTWDwiIWbdYfBW4YBON1fjTJLWxmFA/export?format=tsv";

    /// <summary>
    /// 주의 사항 : 데이터를 저장할 때 반드시 데이터 시트에 칼럼과 동일한 순서로 저장할것
    /// </summary>
    public struct LanguageData
    {
        public List<string> listLangValues;

        public LanguageData(List<string> listLangValues)
        {
            this.listLangValues = new List<string>();
            this.listLangValues = listLangValues;
        }
    }

    /// <summary>
    /// 언어 데이터 저장
    /// 키(string) : 어떤 언어데이터인지 식별하는 키
    /// 값(LanguageData) : 각 언어별 데이터가 들어있음
    /// ex) 키(string) : Apple / 값(LanguageData) : 리스트에 "[0]사과	[1]Apple	[2]りんご" 가 저장되어있음
    /// </summary>
    public Dictionary<string, LanguageData> _mapLangData { get; private set; } = new Dictionary<string, LanguageData>();

    public static bool bIsLoadAllLanguageData { get; private set; } = false;

    public static UnityEvent OnUpdateLocalText { get; private set; } = new UnityEvent();

    public ELanguageKey eCurLanguage { get; private set; } = ELanguageKey.NONE;

    private void Start()
    {
        bIsLoadAllLanguageData = false;

        Init_LangKey();

        StartCoroutine(GetLangCo());
    }

    private void Init_LangKey()
    {
        ELanguageKey eLanguageKey = DataManager_Local.DoLoad_LangKey();
        if (ELanguageKey.NONE == eLanguageKey) //로컬에 저장된 언어 키가 없으므로 로컬 기기 정보에 해당하는 언어로 저장한다.
        {
            Debug.Log("로컬에 저장된 언어 키 정보가 없다.");

            SystemLanguage eLang = Application.systemLanguage;
            switch (eLang)
            {
                case SystemLanguage.Korean:
                    eCurLanguage = ELanguageKey.Korean;
                    Debug.Log("로컬 언어 : 한국어");
                    break;

                case SystemLanguage.English:
                    eCurLanguage = ELanguageKey.English;
                    Debug.Log("로컬 언어 : English");
                    break;

                case SystemLanguage.Japanese:
                    eCurLanguage = ELanguageKey.Japanese;
                    Debug.Log("로컬 언어 : Japanese");
                    break;

                case SystemLanguage.French:
                    eCurLanguage = ELanguageKey.French;
                    Debug.Log("로컬 언어 : Franch");
                    break;

                default:
                    eCurLanguage = ELanguageKey.English;
                    Debug.Log("지원하지 않는 언어");
                    break;
            }
            DataManager_Local.DoSave_LangKey(eCurLanguage);
        }
        else
        {
            eCurLanguage = eLanguageKey;
        }
    }


    private IEnumerator GetLangCo()
    {
        UnityWebRequest www = UnityWebRequest.Get(langURL);
        yield return www.SendWebRequest();

        print(www.downloadHandler.text);

        ProcessData(www.downloadHandler.text);
    }

    private void ProcessData(string strData)
    {
        _mapLangData.Clear();

        string[] row = strData.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length;
        //string[,] Sentence = new string[rowSize - 1, columnSize - 1];

        for (int i = 1; i < rowSize; ++i)
        {
            string[] column = row[i].Split('\t');

            string strKey = column[0];

            List<string> listLangValue = new List<string>();
            for (int j = 1; j < columnSize; ++j)
            {
                listLangValue.Add(column[j]);
            }
            LanguageData pLangData = new LanguageData(listLangValue);

            _mapLangData.Add(strKey, pLangData);
        }

        bIsLoadAllLanguageData = true;
        OnUpdateLocalText.Invoke();
    }

    #region 외부에서 참조 가능한 함수들

    /// <summary>
    /// 키에 해당하는 언어 값을 반환한다.
    /// </summary>
    /// <param name="strKey"></param>
    /// <returns></returns>
    public static string DoGetLocalText(string strKey)
    {
        string strLangValue = "";

        LanguageData pData = Instance._mapLangData[strKey];

        strLangValue = pData.listLangValues[(int)Instance.eCurLanguage];

        Debug.Log($"Find Lang : {strLangValue}");
        return strLangValue;
    }

    public static void DoChange_LocalLang(ELanguageKey eLangKey)
    {
        Instance.eCurLanguage = eLangKey;
        OnUpdateLocalText.Invoke();
        DataManager_Local.DoSave_LangKey(Instance.eCurLanguage);
    }

    /// <summary>
    /// OnUpdateLocalText 에 이벤트 함수를 등록하는 기능
    /// </summary>
    /// <param name="OnAction">등록할 함수</param>
    public static void DoAddListner_UpdateLocalText(UnityAction OnAction)
    {
        OnUpdateLocalText.AddListener(OnAction);
    }

# endregion 외부에서 참조 가능한 함수들
}
