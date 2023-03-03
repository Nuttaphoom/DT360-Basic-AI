using UnityEditor;
using UnityEngine;


public class CoinCollector : MonoBehaviour
{
    [Header("Broadcast to")]  
    [SerializeField]
    private VoidEventChannel _OnSunDown;
    [SerializeField]
    private VoidEventChannel _OnPickUpCoin ;

    private int _number_of_coin = 0; 

    public void IncreaseCoin()
    {
        _number_of_coin += 1 ;

        if (FindObjectsOfType<Coin>().Length <= 1)
        {
            _OnSunDown.RaiseEvent(); 
        }

        _OnPickUpCoin.RaiseEvent(); 
    }


}