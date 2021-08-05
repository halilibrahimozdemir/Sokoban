using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool Move(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) < 0.5)//Will always set one of the coordinates to 0  so avoids diagonal move
        {
            direction.x = 0;
        }
        else
        {
            direction.y = 0;
        } 
        direction.Normalize();
        if (Blocked(transform.position, direction))
        {
            return false;
        }
        else
        {
            transform.Translate(direction.x,0,direction.y);
            TestForOnHole();
            return true;
        }
    }

    private bool Blocked(Vector3 position, Vector2 direction)
    {
        Vector3 newPos = new Vector3(position.x+direction.x,position.y,position.z+direction.y);
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach (var wall in walls)
        {
            if (wall.transform.position.x == newPos.x && wall.transform.position.z == newPos.z)
            {
                return true;
            }
        }

        GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");
        foreach (var box in boxes)
        {
            if (box.transform.position.x == newPos.x && box.transform.position.z == newPos.z)
            {
                Box bx = box.GetComponent<Box>();
                if (!bx.amIFallen)
                {
                    if (bx && bx.Move(direction))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void TestForOnHole()
    {
        GameObject[] holes = GameObject.FindGameObjectsWithTag("Hole");
        GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");
        foreach (var hole in holes)
        {
            if (transform.position.x == hole.transform.position.x && transform.position.z == hole.transform.position.z)
            {
                foreach (var box in boxes)
                {
                    if(box.transform.position.x==hole.transform.position.x && box.transform.position.z==hole.transform.position.z) return;
                }
                transform.Translate(0,-1,0);
                GameManager.MyInstance.GameOver();
            }
        }
    }
    
}
