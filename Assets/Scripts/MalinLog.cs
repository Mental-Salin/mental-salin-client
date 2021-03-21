using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MalinLog : MonoBehaviour
{
    private GameObject _text;
    
    private static MalinLog _instance = null;

    public static MalinLog Get()
    {
        return _instance;
    } 

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        _instance = this;
        _text = GetComponentInChildren<Text>().gameObject;
        
    }
    
    public float disappearedTime = 1f;
    private float timer = 0;
    // Update is called once per frame
    void Update()
    {
        if (_text.activeSelf)
        {
            timer += Time.deltaTime;
            if (timer > disappearedTime)
                _text.SetActive(false);
        }
  
    }

    public void ShowLog(string str, float timeSec = 2f)
    {
        _text.GetComponent<Text>().text = str;
        timer = 0;
        disappearedTime = timeSec;
        _text.SetActive(true);
        Debug.Log($"[Malin] {str}");
    }
}
