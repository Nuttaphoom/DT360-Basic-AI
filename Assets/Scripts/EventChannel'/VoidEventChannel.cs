using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "VoidEventCH" , menuName = "ScriptableObject/EventChannel/VoidEventCH")]
public class VoidEventChannel : ScriptableObject
{
    public UnityAction _OnEvent; 
    
    public void RaiseEvent()
    {
        _OnEvent?.Invoke(); 
    }
}