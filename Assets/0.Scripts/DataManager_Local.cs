using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ��⿡ ����� �������� �����ϴ� �Ŵ���
/// </summary>
public class DataManager_Local : Singleton<DataManager_Local>
{
    private SaveData _pSaveData = new SaveData();

    private void Start()
    {
        //_pSaveData.DoLoad_LangKey();
    }

    public static void DoSave_LangKey(ELanguageKey eLangKey)
    {
        Instance._pSaveData.DoSave_LangKey(eLangKey);
    }

    public static ELanguageKey DoLoad_LangKey()
    {
        return Instance._pSaveData.DoLoad_LangKey();
    }
}
