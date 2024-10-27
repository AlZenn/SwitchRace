using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireGroundController_SC : MonoBehaviour
{
    public TireMovement_SC tiresc;
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            tiresc.isGrounded = true;
            //Debug.Log("Araba yere temas etti.");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            tiresc.isGrounded = false;
            //Debug.Log("Araba yere temas etmedi.");
        }
    }
}
