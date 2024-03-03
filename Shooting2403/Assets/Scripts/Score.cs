using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_Score;
    // Start is called before the first frame update
    void Start()
    {
        var score = PlayerPrefs.GetInt("Score");
        m_Score.text = $"{score:d3}点でした";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(0);
        }
    }
}
