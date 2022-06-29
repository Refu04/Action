using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

public class FallFloor : MonoBehaviour
{
    [SerializeField]
    private int waitTime;
    private void Start()
    {
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            var cts = this.GetCancellationTokenOnDestroy();
            Fall(cts).Forget();
        }
    }

    private async UniTask Fall(CancellationToken token)
    {
        await UniTask.Delay(waitTime);
        gameObject.SetActive(false);
        await UniTask.Delay(3000);
        gameObject.SetActive(true);
    }
}
