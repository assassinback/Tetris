using UnityEngine;

public class Shape : MonoBehaviour
{
    public bool canRotate = true;

    public Vector3 queueOffset;
    public Vector2 startPos;
    public Vector2 direction;   
    private void Move(Vector3 moveDirection) => transform.position += moveDirection;

    public void MoveLeft() => Move(new Vector3(-1, 0, 0));

    public void MoveRight() => Move(new Vector3(1, 0, 0));

    public void MoveUp() => Move(new Vector3(0, 1, 0));
    private Touch touch;
    //public void MoveDown()
    //{
    //    if (Input.touchCount > 0)
    //    {
    //        touch = Input.GetTouch(0);

    //        if (touch.phase == TouchPhase.Moved)
    //        {
    //            Debug.Log("here");
    //            transform.position = Vector2.up * -1;
    //            //rb.velocity = (touch.deltaPosition.y > 0)
    //            //    ? Vector2.up * -1
    //            //    : -Vector2.up * -1;
    //        }
    //    }
    //}
    public void MoveDown() => Move(new Vector3(0, -1, 0));

    public void RotateRight()
    {
        if (canRotate)
        {
            transform.Rotate(0, 0, -90);
        }
    }
    
    public void RotateLeft()
    {
        if (canRotate)
        {
            transform.Rotate(0, 0, 90);
        }
    }

    public void RotateClockwise(bool rotateClockwise)
    {
        if (rotateClockwise)
        {
            RotateRight();
        }
        else
        {
            RotateLeft();
        }
    }
}
