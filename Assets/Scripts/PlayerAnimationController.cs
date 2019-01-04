using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        bool up = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
        bool down = Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);
        bool left = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);
        bool right = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
        if(up)
            gameObject.transform.rotation = Quaternion.Euler(0,0, 0);
        else if (down)
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (left)
            gameObject.transform.rotation = Quaternion.Euler(0, -90, 0);
        else if (right)
            gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
    }
}
