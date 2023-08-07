using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System;
using TMPro;
public class CointCollection : MonoBehaviour
{
    [SerializeField] private GameObject coinFrefab;
    [SerializeField] private Transform coinParent;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Transform endPosition;
    [SerializeField] private TextMeshProUGUI cointText;
    [SerializeField] private float duration = 2f;
    [SerializeField] private int coinAmont;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;
    List<GameObject> coins = new List<GameObject>();
    private Tween coinReactTween;
    private int coint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetCoin(int value)
    {
        coint = value;
        cointText.text = coint.ToString("0000000");
    }

    [Button]
    public async void ConitBehavior()
    {
        //Reset
        coint = 0;
        for (int i = 0; i< coins.Count; i++)
        {
            Destroy(coins[i]);
        }
        coins.Clear();
        // Spawn Coint to speciphic location with random value
        List<UniTask> spawnCoinTaskList = new List<UniTask>();
        for (int i = 0; i < coinAmont; i++)
        {
            GameObject coinInstance = Instantiate(coinFrefab, coinParent);
            float xPosition = spawnPosition.position.x + UnityEngine.Random.Range(minX, maxX);
            float yPosition = spawnPosition.position.y + UnityEngine.Random.Range(minY, maxY);
             coinInstance.transform.position = new Vector3(xPosition, yPosition);
            spawnCoinTaskList.Add( coinInstance.transform.DOPunchRotation(new Vector3(0, 30, 0), UnityEngine.Random.Range(0, 1f)).SetEase(Ease.InOutElastic ).ToUniTask());
            coins.Add(coinInstance);
            await UniTask.Delay(TimeSpan.FromSeconds(0.01f));
        }
        await UniTask.WhenAll(spawnCoinTaskList);
        //await UniTask.Delay(TimeSpan.FromSeconds(1f));
        //Move all the coint to the Coint Layout
        await MoveCoinsTask();
        // animate reaction when collecting coin
    }

    private async UniTask MoveCoinsTask()
    {
        List<UniTask> moveCoinTask = new List<UniTask>();
        for (int i = coins.Count -1 ; i >= 0; i--)
        {
            moveCoinTask.Add(MoveCoinTask(coins[i]));
            await UniTask.Delay(TimeSpan.FromSeconds(0.05f));   
        }
    }

    private async UniTask MoveCoinTask(GameObject coinInstance)
    {
        await coinInstance.transform.DOMove(endPosition.position, duration).SetEase(Ease.InBack).ToUniTask();
        GameObject temp = coinInstance;
        coins.Remove(coinInstance);
        Destroy(temp);
        ReactToCollectionCoin();
        SetCoin(coint+1);
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
