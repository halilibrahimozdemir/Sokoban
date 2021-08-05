using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Box : MonoBehaviour
{
    public bool amIDone=false;
    public bool amIFallen;
    public bool countDown;
    public int count;
    public RectTransform countDownObj;
    public Text countText;
    public int moves;

    private void Start()
    {
        if(countDown) countText.text = count.ToString();
    }

    private void Update()
    {
        if (countDown)
        {
            countDownObj.gameObject.SetActive(true);
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            countDownObj.transform.position = screenPoint;
        }
    }

    public bool Move(Vector2 direction)
    {
        if (countDown && count <= 0) return false;
        if (BoxBlocked(transform.position, direction))
        {
            return false;
        }
        else
        {
            transform.Translate(direction.x,0,direction.y);
            moves++;
            if (countDown)
            {
                count--;
                countText.text = count.ToString(); 
                if (count<=0 && !amIDone)
                {
                    Debug.Log("game over");
                }
            }
            TestForOnRightArea();
            TestForOnHole();
            return true;
        }
    }

    bool BoxBlocked(Vector3 position, Vector2 direction) //Boxes blocked by other boxes or walls
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
            if (box.transform.position==newPos)
            {
                return true;
            }
        }
        return false;
    }

    private void TestForOnRightArea()
    {
        GameObject[] areas = GameObject.FindGameObjectsWithTag("Area");
        foreach (var area in areas)
        {
            if (area.GetComponent<Renderer>().material.color.CompareRGB(GetComponent<Renderer>().material.color))
            {
                if (transform.position == area.transform.position)
                {
                    area.GetComponent<Area>().amIDone = true;
                    amIDone = true;
                }
            }
        }
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
                    if (box !=gameObject)
                    {
                        if(box.transform.position.x==hole.transform.position.x && box.transform.position.z==hole.transform.position.z) return; 
                    }
                }
                transform.position = hole.transform.position + new Vector3(0, 0.1f, 0);
                amIFallen = true;
            }
        }
    }
}
