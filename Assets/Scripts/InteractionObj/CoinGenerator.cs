using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
    [Header("listen to")]
    [SerializeField]
    private VoidEventChannel _OnSunRaise;

    [SerializeField]
    private Coin _CoinPrefab;


    private void Awake()
    {
        _OnSunRaise._OnEvent += GenNewCoin; 
    }
    private void OnDisable()
    {
        _OnSunRaise._OnEvent -= GenNewCoin;
    }
    public void GenNewCoin()
    {
        for (int i =0;  i < Random.Range(4,7); i++)
        {
            Vector3 spawnPos = transform.position + new Vector3(Random.Range(-5,5), 0, Random.Range(-5, 5) );
            Instantiate(_CoinPrefab, spawnPos, _CoinPrefab.transform.rotation) ; 
        }    
    }
}
