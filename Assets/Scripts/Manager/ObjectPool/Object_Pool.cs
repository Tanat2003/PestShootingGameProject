using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Pool : MonoBehaviour
{
    [Header("Pool")]
    [SerializeField] private GameObject weaponPickup;
    [SerializeField] private GameObject ammoPickup;
    [SerializeField] private GameObject weaponUpgrade;
    public static Object_Pool instance;
  
    
    [SerializeField] private int poolSize = 10;
    

    

    //�纤��Ẻdictionary<�������ͧ������(key),��Ңͧ������> ����dictionary      =�����ҡѺ�������key�ͧ������ŧ仾��������Ң�����
    private Dictionary<GameObject,Queue<GameObject>> poolDictionary 
        = new Dictionary<GameObject, Queue<GameObject>>();

    private void Awake() //�����ʤ�û���������component������ź�͡��component���
        //Awake������Start
    {
        if(instance == null)
        {
            instance = this;
        }else
        {
            Destroy(gameObject);
        }
        
        
    }
    private void Start()
    {
        InitializeNewPool(weaponPickup);
        InitializeNewPool(ammoPickup);
        InitializeNewPool(weaponUpgrade);
    }



    //���¡�����͹Ѻ�����ѧ����Թobjtopool
    public void ReturnObject(GameObject objToReturn, float delay = .001f) 
        => StartCoroutine(DelayReturn(delay,objToReturn));
        
   
    private IEnumerator DelayReturn(float delay,GameObject objToReturn) //������Ҷ����ѧ����Թobjtopool
    {
        yield return new WaitForSeconds(delay);
        ReturnToPool(objToReturn);
    }
    private void ReturnToPool(GameObject objToReturn)
    {
        GameObject originalPrefab = objToReturn.GetComponent<PooledObject>().originalPrefab;
        objToReturn.SetActive(false);
        objToReturn.transform.parent = transform; //�����˹�obj�����obj���ʤ�Ի��������

        poolDictionary[originalPrefab].Enqueue(objToReturn); //����Թobj��dictionary�������Pool�����ҧ


    }//returnobj�����Pool
    private void InitializeNewPool(GameObject prefab) //���ҧPool����
    {
        poolDictionary[prefab] = new Queue<GameObject>(); //����Prefab���ŧ�Pool
        for (int i = 0; i <= poolSize; i++)
        {
            
            CreateNewObject(prefab); //���ҧobjŧ�Pool
        }
    }

    private void CreateNewObject(GameObject prefab)
    {
        GameObject newObj = Instantiate(prefab, transform);

        //����componentŧ��obj��������ҧ�����������component�ͧobj����纵���˹�original�ͧprefab
        //���ͷ����Ҩ������繤����dictionary
        newObj.AddComponent<PooledObject>().originalPrefab = prefab;
        newObj.SetActive(false);
        poolDictionary[prefab].Enqueue(newObj); //��������Dictionary
    }

    public GameObject GetObject(GameObject prefab,Transform target,bool vectorUp = false,bool axisMisnuX = false) //���ҧObj���������ҡPool
    {
        //�����Gameobj�����������յç�Ѻkey㹢ͧDictionary����
        //������������ҧPool���������
        if(poolDictionary.ContainsKey(prefab) == false)
        {
            InitializeNewPool(prefab);
        }
        if(poolDictionary[prefab].Count == 0) //����dictionary��������value���������ҧobj�pool����������value���dictionary
        {
            CreateNewObject(prefab);
        }
            
        GameObject objToGet = poolDictionary[prefab].Dequeue();
        if(vectorUp)
        {
            objToGet.transform.position = target.position +  new Vector3(0,0.6f,0);

        }else if(axisMisnuX)
        {
            objToGet.transform.position = target.position + new Vector3(-2, 0, 0);
        }else
        {
            objToGet.transform.position = target.position;
        }

        objToGet.transform.parent = null;//��������͡Gameobject objectpool

        var anim = objToGet.GetComponentInChildren<Animator>();
        if(anim != null) //�͹���ҷ��֧�ѵ���ҨҡPool���CullingMode ����� cullUpdateTranfrom �����ѹ����ѹ���ͧ���������Ŷ���ѵ���������Թ���С��ͧ
        {
            anim.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        }

        objToGet.SetActive(true);
        return objToGet;
    }



}
