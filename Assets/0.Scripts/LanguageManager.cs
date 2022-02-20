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
    /// ���� ���� : �����͸� ������ �� �ݵ�� ������ ��Ʈ�� Į���� ������ ������ �����Ұ�
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
    /// ��� ������ ����
    /// Ű(string) : � ���������� �ĺ��ϴ� Ű
    /// ��(LanguageData) : �� �� �����Ͱ� �������
    /// ex) Ű(string) : Apple / ��(LanguageData) : ����Ʈ�� "[0]���	[1]Apple	[2]���" �� ����Ǿ�����
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
        if (ELanguageKey.NONE == eLanguageKey) //���ÿ� ����� ��� Ű�� �����Ƿ� ���� ��� ������ �ش��ϴ� ���� �����Ѵ�.
        {
            Debug.Log("���ÿ� ����� ��� Ű ������ ����.");

            SystemLanguage eLang = Application.systemLanguage;
            switch (eLang)
            {
                case SystemLanguage.Korean:
                    eCurLanguage = ELanguageKey.Korean;
                    Debug.Log("���� ��� : �ѱ���");
                    break;

                case SystemLanguage.English:
                    eCurLanguage = ELanguageKey.English;
                    Debug.Log("���� ��� : English");
                    break;

                case SystemLanguage.Japanese:
                    eCurLanguage = ELanguageKey.Japanese;
                    Debug.Log("���� ��� : Japanese");
                    break;

                case SystemLanguage.French:
                    eCurLanguage = ELanguageKey.French;
                    Debug.Log("���� ��� : Franch");
                    break;

                default:
                    eCurLanguage = ELanguageKey.English;
                    Debug.Log("�������� �ʴ� ���");
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

    #region �ܺο��� ���� ������ �Լ���

    /// <summary>
    /// Ű�� �ش��ϴ� ��� ���� ��ȯ�Ѵ�.
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
    /// OnUpdateLocalText �� �̺�Ʈ �Լ��� ����ϴ� ���
    /// </summary>
    /// <param name="OnAction">����� �Լ�</param>
    public static void DoAddListner_UpdateLocalText(UnityAction OnAction)
    {
        OnUpdateLocalText.AddListener(OnAction);
    }

# endregion �ܺο��� ���� ������ �Լ���
}
