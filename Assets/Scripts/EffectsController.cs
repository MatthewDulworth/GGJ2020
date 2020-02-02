using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Used to handle effects like screen shake, camera flashes, controller rumbles, and anything else we dont want
 * to clutter other scripts with 
 */
public class EffectsController : MonoBehaviour
{

    //Camera Stuff
    //Camera shake will only do anything if shaking is true
    private bool shaking;
        // Transform of the camera to shake. Grabs the gameObject's transform
        // if null.
        private Transform camTransform;

        // How long the object should shake for.
        public float shakeDuration;

        // Amplitude of the shake. A larger value shakes the camera harder.
        public static float shakeIntensity;
        public float decay = 1.0f;

        public Vector3 originalPos;

    //Screen Flash
    //Will only do caluclations for screen flash if screenFlashing is true
    private bool screenFlashing;
        //What the screen flash looks like. Must have a spriterender
        public GameObject screenFlash;
        //To hold a specific instance of the screen flash after it is spawned
        private GameObject screenFlashInstance;

        //How quickly will the flash expand
        public float flashSpeed;
        //How long will the flash stay before starting to fade  
        public float flashDuration;
        //How fast the flash will fade
        public float fadeSpeed;
        //How large the flash will expand before stopping
        public float maxFlashSize;
        //To keep track of elapsed time
        private float flashTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void FixedUpdate()
    {
        //Camera shake
        if (shaking)
        {
            if (shakeDuration > 0)
            {
                camTransform.localPosition = originalPos + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0) * shakeIntensity;

                shakeDuration -= Time.deltaTime * decay;
            }
            else
            {
                shakeDuration = 0f;
                camTransform.localPosition = originalPos;
                shaking = false;
            }
        }
        //Screen flash
        if (screenFlashing)
        {
            Transform flashTransform = screenFlashInstance.transform;
            //Decrement timer
            flashTimer -= Time.deltaTime;

            if (flashTransform.localScale.x < maxFlashSize || flashTransform.localScale.y < maxFlashSize)
            {
                 //Expand flash
                flashTransform.localScale = flashTransform.localScale * flashSpeed;
            }
            if (flashTimer <= 0)
            {
                //Start to make when flash is done
                Color newColor = screenFlashInstance.GetComponent<Image>().color;
                newColor.a -= Time.deltaTime * fadeSpeed;
                screenFlashInstance.GetComponent<Image>().color = newColor;
                if(newColor.a <= 0)
                {
                    screenFlashing = false;
                    Destroy(screenFlashInstance);
                }
            }
        }
    }
    /*
     * Shakes camera for inShakeDuration seconds with inShakeIntensity scale.
     */
    public void CameraShake(float inShakeDuration, float inShakeIntensity)
    {
        shaking = true;
        camTransform = FindObjectOfType<Camera>().transform;
        shakeDuration = inShakeDuration;
        shakeIntensity = inShakeIntensity;
    }
    public void ScreenFlash(Vector2 position)
    {
        screenFlashInstance = Instantiate(screenFlash, GameObject.Find("Main Canvas").transform);
        screenFlashInstance.GetComponent<RectTransform>().anchoredPosition = screenFlashInstance.GetComponent<RectTransform>().anchoredPosition + position;
        screenFlashing = true;
        flashTimer = flashDuration;
    }

    /*
     * Recursively decreases the opacity of any sprite renderers or image components on a gameobject
     */
    public void RecursiveOpacityDecreaser(float decrementAmount, float opacityMinimum, GameObject obj)
    {
        //Possible because there may not be these components
        SpriteRenderer posSpriteRend = obj.GetComponent<SpriteRenderer>();
        Image posImage = obj.GetComponent<Image>();

        if (posSpriteRend != null)
        {
            Color newColor = posSpriteRend.color;
            newColor.a -= decrementAmount;
            if (newColor.a >= opacityMinimum)
            {
                posSpriteRend.color = newColor;
            }
            else
            {
                newColor.a = opacityMinimum;
                posSpriteRend.color = newColor;
            }
        }
        if (posImage != null)
        {
            Color newColor = posImage.color;
            newColor.a -= decrementAmount;
            if (newColor.a >= opacityMinimum)
            {
                posImage.color = newColor;
            }
            else
            {
                newColor.a = opacityMinimum;
                posImage.color = newColor;
            }
        }
        foreach (Transform e in obj.transform)
        {
            RecursiveOpacityDecreaser(decrementAmount, opacityMinimum, e.gameObject);
        }
    }
}
