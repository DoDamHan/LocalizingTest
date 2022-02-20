using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 현재 선택한 언어 설정 값에 따라 텍스트를 출력하기
/// ex) 한국어가 선택된 상태라면 "TItle" 이 키값에 해당하는 "아직 정하지 않음" 가 출력되어야 함.
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
            Debug.LogError("텍스트 컴포넌트가 없습니다.");
        }

        LanguageManager.DoAddListner_UpdateLocalText(UpdateLocalText);
    }

    private IEnumerator OnCoroutine_LoadAllLangData()
    {
        while (!LanguageManager.bIsLoadAllLanguageData)
        {
            yield return null;
        }

        Debug.Log("모든 데이터 로드 완료");

        

//        _pText.text = LanguageManager.DoGetLocalText(strKey);
    }


    private void UpdateLocalText()
    {
        _pText.text = LanguageManager.DoGetLocalText(strKey);
    }
}
