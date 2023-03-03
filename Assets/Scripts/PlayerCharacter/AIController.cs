using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine.AI;
using UnityEditor.SceneTemplate;

[RequireComponent(typeof(CharacterController))]
public class AIController : MonoBehaviour  
{

    [Header("listen to")]
    [SerializeField]
    private VoidEventChannel _OnSunDown; 
    [SerializeField]
    private VoidEventChannel _OnSunRaise;
    [SerializeField]
    private VoidEventChannel _OnPickUpCoin;


    private int _energy = 5; 

    [SerializeField]
    private float _moveSpeed = 0;


    [SerializeField]
    private FSM _fsm = new FSM();

    [SerializeField]
    NavMeshAgent _agent;

    private CharacterController _characterController;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>(); 
        _characterController = GetComponent<CharacterController>();  
        _fsm = new FSM();
        _fsm.Init(gameObject);

        _OnSunRaise._OnEvent += () => _fsm.ChangeState(typeof(GatherCoinState));
        _OnSunDown._OnEvent += () => _fsm.ChangeState(typeof(ReturnHome));
        _OnPickUpCoin._OnEvent += DecreaseEnergy; 

    }


    public void RestingAnimation()
    {
        StartCoroutine(RestingCO()); 
    } 

    private IEnumerator RestingCO()
    {
        yield return new WaitForSeconds(2);
        _energy += 5; 
        ArriveAtHome();
    }
    private void DecreaseEnergy()
    {
        _energy -= 1; 
        if (_energy <= 0) {
            _fsm.ChangeState(typeof(Resting));
        }
    } 

    public void ArriveAtHome()
    {
        _OnSunRaise.RaiseEvent(); 
    } 
    private void Update()
    {
        _fsm.StateUpdate();   
    }

    public void UpdateMovement(float x, float z)
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0;
        right.y = 0;


        Vector3 motion = forward * z + right * x;

  
        _characterController.Move(motion.normalized * _moveSpeed * Time.deltaTime ) ;
        
        return; 

    }

    public bool AI_GO_POS(Transform pos)
    {
        NavMeshPath path = new NavMeshPath();

        if (_agent.CalculatePath(pos.transform.position,path) ){
            Vector3 dir = path.corners[1] - transform.position;
            dir.y = 0;
            transform.Translate(dir.normalized * Time.deltaTime * _moveSpeed);
            if (dir.magnitude < 0.1f)
            {
                if (path.corners.Length <= 2)
                    return true;
            }
        }
       
        return false ;
    }
 

    public List<Coin> GetCoinsRef()
    {
        List<Coin> c = new List<Coin>();
        foreach (Coin coin in FindObjectsOfType<Coin>())
        {
            c.Add(coin);
        }
        return c;
    }

}