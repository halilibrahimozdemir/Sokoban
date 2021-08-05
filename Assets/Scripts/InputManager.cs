using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public GameObject playerObject;
    private Vector3 truePos;
    public float gridSize;

    private bool m_ReadyForInput;
    public Player m_Player;
    public bool isSettingReady;

    private void Start()
    {
        m_Player = FindObjectOfType<Player>();
        playerObject = m_Player.gameObject;
    }

    private void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveInput.Normalize();
        if (moveInput.sqrMagnitude > 0.5) //Button pressed or held
        {
            if (m_ReadyForInput)
            {
                m_ReadyForInput = false;
                m_Player.Move(moveInput);
                //m_NextButton.SetActive(IsLevelComplete());
            }
            else
            {
                if (!isSettingReady)
                {
                    StartCoroutine(SetReadyAfterTime(0.25f)); 
                }
            }
        }    
    }

    void LateUpdate()
    {
        truePos.x = Mathf.Floor(playerObject.transform.position.x / gridSize) * gridSize;
        truePos.y = Mathf.Floor(playerObject.transform.position.y / gridSize) * gridSize;
        truePos.z = Mathf.Floor(playerObject.transform.position.z / gridSize) * gridSize;

        playerObject.transform.position = truePos;
    }

    private IEnumerator SetReadyAfterTime(float t)
    {
        isSettingReady = true;
        yield return new WaitForSeconds(t);
        m_ReadyForInput = true;
        isSettingReady = false;
    }
}
