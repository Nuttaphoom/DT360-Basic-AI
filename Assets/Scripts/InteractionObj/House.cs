using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class House : MonoBehaviour
{
    [Header("Listen to")]
    [SerializeField]
    private VoidEventChannel _OnSunDown;

    [Header("broadcast to")]
    [SerializeField]
    private VoidEventChannel _OnSunRaise;
    CoinGenerator _coinGenerator ;
    bool _night = false;
 
    private void Awake()
    {
        _coinGenerator = FindObjectOfType<CoinGenerator>();
        _OnSunDown._OnEvent += SunDown; 
    }

    private void Start()
    {
        _OnSunRaise.RaiseEvent();
    }

    private void OnDisable()
    {
        _OnSunDown._OnEvent -= SunDown;
    }

    private void SunDown()
    {
        _night = true;  
    }
 
}