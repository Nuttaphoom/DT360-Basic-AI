using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;

public class FSM 
{
    private State _currentState = null;  
    private Dictionary<System.Type,State> _validStates = new Dictionary<System.Type,State>(); 

    private GameObject _subject;  
    public void Init(GameObject sub)
    {
        _subject = sub ;   
        _validStates.Add(typeof(ReturnHome) , new ReturnHome(this));
        _validStates.Add(typeof(GatherCoinState), new GatherCoinState(this));
        _validStates.Add(typeof(Resting), new Resting(this));

        _currentState = _validStates[typeof(ReturnHome)] ;
        _currentState.StateEnter();  
    }

    public void ChangeState(System.Type stateType) 
    {
        Debug.Log("change state to " + stateType);
        if (_validStates.ContainsKey(stateType) ) 
        {
            _currentState.StateExit();  
            _currentState = _validStates[stateType]; 
            _currentState.StateEnter();  
        } 
    }

    public void StateUpdate()
    {

        _currentState.StateUpdate();  
    }

    public GameObject GetSubject => _subject;



}

public abstract class State
{
    private FSM _fsm; 
    public State(FSM fsm)
    {
        _fsm = fsm ;  
    }

    public abstract void StateEnter();
    public abstract void StateExit();
    public abstract void StateUpdate();

    protected FSM GetFsm => _fsm     ; 


}

public class ReturnHome : State
{
    Transform _homeTF;
    public ReturnHome(FSM fsm) : base(fsm)
    {
    }

    public override void StateEnter()
    {
        _homeTF = GameObject.FindGameObjectWithTag("Gate").transform; 
    }

    public override void StateExit()
    {
    
    }

    public override void StateUpdate()
    {
        
            bool met = GetFsm.GetSubject.GetComponent<AIController>().AI_GO_POS(_homeTF);
            if (met)
            {
                GetFsm.GetSubject.GetComponent<AIController>().ArriveAtHome();
            }
        

    }
}

public class Resting : State
{
    Transform _homeTF;
    bool athome = false;

    public Resting(FSM fsm) : base(fsm)
    {
    }

    public override void StateEnter()
    {
        _homeTF = GameObject.FindGameObjectWithTag("Gate").transform;
    }

    public override void StateExit()
    {
        athome = false;
    }

    public override void StateUpdate()
    {
        if (!athome)
        {
            bool met = GetFsm.GetSubject.GetComponent<AIController>().AI_GO_POS(_homeTF);
            if (met)
            {
                athome = true;

                GetFsm.GetSubject.GetComponent<AIController>().RestingAnimation();
            }
        }
    }
}

public class GatherCoinState : State
{
    List<Coin> _coins ;
    int _closestCoin = 0;  
    public GatherCoinState(FSM fsm) : base(fsm)
    {
    
    }

    public override void StateEnter()
    {
        _coins = GetFsm.GetSubject.GetComponent<AIController>().GetCoinsRef(); 

    }

    public override void StateExit()
    {

    }
    public override void StateUpdate()
    {
        if (_coins.Count > 0)
        {
            bool met = GetFsm.GetSubject.GetComponent<AIController>().AI_GO_POS(_coins[_coins.Count - 1].transform);
            if (met)
            {
                _coins[_coins.Count - 1].PickUp(); 
                _coins.Remove(_coins[_coins.Count - 1]);
            }
        }
         
    }
}
