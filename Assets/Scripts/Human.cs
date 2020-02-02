using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
   public float delay = 3;

   // Start is called before the first frame update
   void Start()
   {
      StartCoroutine(fade());
   }

   IEnumerator fade()
   {
      yield return new WaitForSeconds(delay);
      StartCoroutine(fout());
   }

   IEnumerator fout()
   {
      SpriteRenderer sr = GetComponent<SpriteRenderer>();
      Color newColor = sr.color;

      for (float f = 1f; f >= 0; f -= 0.1f)
      {
         newColor.a = f;
         sr.color = newColor;
         yield return new WaitForSeconds(0.1f);
      }

      Destroy(gameObject);
   }
}
