using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System;

public class CointCollection : MonoBehaviour
{
    [SerializeField] private GameObject coinFrefab;
    [SerializeField] private Transform coinParent;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Transform endPosition;
    [SerializeField] private float duration = 2f;
    [SerializeField] private int coinAmont;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;
    List<GameObject> coins = new List<GameObject>();
    private Tween coinReactTween;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Button]
    public async void ConitBehavior()
    {   
        //Reset
        for (int i = 0; i< coins.Count; i++)
        {
            Destroy(coins[i]);
        }
        coins.Clear();
        // Spawn Coint to speciphic location with random value
        for (int i = 0; i < coinAmont; i++)
        {
            GameObject coinInstance = Instantiate(coinFrefab, coinParent);
            float xPosition = spawnPosition.position.x + UnityEngine.Random.Range(minX, maxX);
            float yPosition = spawnPosition.position.y + UnityEngine.Random.Range(minY, maxY);
            coinInstance.transform.position = new Vector3(xPosition, yPosition);
            coins.Add(coinInstance);
        }
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        //Move all the coint to the Coint Layout
        await MoveCoinsTask();
        // animate reaction when collecting coin
    }

    private async UniTask MoveCoinsTask()
    {
        List<UniTask> moveCoinTask = new List<UniTask>();
        for (int i = 0; i < coins.Count; i++)
        {
            moveCoinTask.Add(MoveCoinTask(i));
            await UniTask.Delay(TimeSpan.FromSeconds(0.05f));
        }
    }

    private async UniTask MoveCoinTask(int i)
    {
        await coins[i].transform.DOMove(endPosition.position, duration).ToUniTask();
        ReactToCollectionCoin();
    }

    [Button]
    private async UniTask ReactToCollectionCoin()
    {
        if(coinReactTween == null)
        {
            coinReactTween = endPosition.DOPunchScale(new Vector3(.5f, .5f, 0f), 0.3f).SetEase(Ease.InOutElastic);
            await coinReactTween.ToUniTask();
            coinReactTween = null;
        }
            
    }

   
}
