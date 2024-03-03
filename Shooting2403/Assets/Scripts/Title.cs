using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    [SerializeField]
    private Button m_StartButton;
    // Start is called before the first frame update
    void Start()
    {
        // ボタン押したらスタート
        m_StartButton?.onClick.AddListener(OnPressedStart);
    }

    private void OnPressedStart()
    {
        m_StartButton.interactable = false;
        ChangeSceneAsync().Forget();
    }

    private async UniTask ChangeSceneAsync()
    {
        await SceneManager.LoadSceneAsync((int)SceneIndex.Main);
    }
}
