using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ObstableRock : ObstacleSpawner
{
    public void Start()
    {
        transform.localScale = Vector3.zero;
    }
    public override void Spawn()
    {
        transform.DOScale(Vector3.one, .245f);
        transform.DOLocalMoveY(0, .5f).SetEase(Ease.InExpo).OnComplete(() =>
        {
            if (this.isActiveAndEnabled)
            {
                //StartCoroutine(CameraShake(.25f, .4f));
                StartCoroutine(PlayRockSound(0.125f));
            }
        });
    }

    public IEnumerator PlayRockSound(float delay)
    {
        yield return new WaitForSeconds(delay);
        SoundManager.self.PlayAudio("rock");
    }
}
