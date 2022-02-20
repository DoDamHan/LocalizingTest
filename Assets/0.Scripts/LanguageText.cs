using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���� ������ ��� ���� ���� ���� �ؽ�Ʈ�� ����ϱ�
/// ex) �ѱ�� ���õ� ���¶�� "TItle" �� Ű���� �ش��ϴ� "���� ������ ����" �� ��µǾ�� ��.
/// </summary>
public class LanguageText : MonoBehaviour
{

    private Text _pText = null;


    [SerializeField]
    private string strKey = "";

    private void Awake()
    {
        StartCoroutine(nameof(OnCoroutine_LoadAllLangData));

        _pText = GetComponent<Text>();

        if (null == _pText)
        {
            Debug.LogError("�ؽ�Ʈ ������Ʈ�� �����ϴ�.");
        }

        LanguageManager.DoAddListner_UpdateLocalText(UpdateLocalText);
    }

    private IEnumerator OnCoroutine_LoadAllLangData()
    {
        while (!LanguageManager.bIsLoadAllLanguageData)
        {
            yield return null;
        }

        Debug.Log("��� ������ �ε� �Ϸ�");

        

//        _pText.text = LanguageManager.DoGetLocalText(strKey);
    }


    private void UpdateLocalText()
    {
        _pText.text = LanguageManager.DoGetLocalText(strKey);
    }
}
