using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    public float LerpSpeed = 5;
    public float ScaleSpeed = 3;
    public Transform child;
    public float scaleChange = 0.1f;
    public AnimationCurve animationCurve;
    public Animation anim;



   
    float currentLerpTime;
    float currentScaleTime;

    float perc = 1;
    float scalePerc;

    Vector3 startPos;
    Vector3 endPos;


    Vector3 startScale;
    Vector3 endScale;


    int bounds = 5;
    int maxZ , maxBackTrack = 10;
    // Start is called before the first frame update
    void Start()
    {
        endPos = gameObject.transform.position;
        endScale = gameObject.transform.localScale;
        maxZ = maxBackTrack;

        anim = gameObject.GetComponentInChildren<Animation>();
        child = gameObject.transform.Find("PenguinBaseMesh");
    }

    bool firstInput;
    public bool JustJump;
    // Update is called once per frame
    void Update()
    {
        bool up = (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && gameObject.transform.position == endPos ;
        bool down = (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && gameObject.transform.position == endPos;
        bool left = (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && gameObject.transform.position == endPos;
        bool right = (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && gameObject.transform.position == endPos;
        startPos = gameObject.transform.position;
        if (up || down || left || right)
        {
            if (perc == 1)
            {
                anim.Play("Jump");
                currentLerpTime = 0;
                JustJump = firstInput = true;

            }
            if (gameObject.transform.position == endPos)
            {
                var x = right ? 1 : left ? -1 : 0;
                var z = up ? 1 : down ? -1 : 0;
                var newPos = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
                if (newPos.z >= maxZ - maxBackTrack && Mathf.Abs(newPos.x) < bounds)
                {
                    endPos = newPos;
                    maxZ = Mathf.Max(maxZ, (int)endPos.z);
                }

            }
        }
        if (firstInput)
        {
            currentLerpTime += Time.deltaTime * LerpSpeed;
            perc = currentLerpTime;
            gameObject.transform.position = Vector3.Lerp(startPos, endPos, animationCurve.Evaluate(perc));
            if (perc > 0.8)
                perc = 1;
            if (Mathf.Round(perc) == 1)
            {
                JustJump = false;
            }
            
        }
    }
}
