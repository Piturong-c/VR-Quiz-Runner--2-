using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ObstacleTree : ObstacleSpawner
{
   public bool isRight = true;
   
   public override void Spawn()
   {
      if (isRight)
         {
            
            //transform.DOMove(Vector3(transform.position.x, transform.position.y, transform.position.z+10));
            //transform.DOLocalRotate(Vector3.zero, 1f).SetEase(Ease.OutBounce);
           // transform.Translate(1 * Time.deltaTime,0,0); //ขยับวัตถุ
//            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y+10);
            //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y-10);
            transform.Translate(5 * Time.deltaTime, 5 * Time.deltaTime, 5 * Time.deltaTime);
         }
         else
         {
            //transform.DOMove(new Vector3(10,0,10));
            //transform.DOLocalRotate(new Vector3(0, 180, 0), 1f).SetEase(Ease.OutBounce);
            //transform.DOMoveZ(new Vector3(0, 0, 50),1f).SetEase(Ease.InBack);
            //transform.DOMoveZ(new Vector3(0, 0, 50),1f).SetEase(Ease.InBack);
            //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y-10);
            transform.Translate(5 * Time.deltaTime, 5 * Time.deltaTime, 5 * Time.deltaTime);
         }
      if (this.isActiveAndEnabled)
      {
         
            StartCoroutine(PlayTreeSound(0.45f));
         
      }
      
   }
   
   public IEnumerator PlayTreeSound(float delay)
   {
      yield return new WaitForSeconds(delay);
      SoundManager.self.PlayAudio("tree");
   }
}
