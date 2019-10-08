﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    DiskFactory diskFactory;
    SSDirector ssDirector;
    Color[] colors = { Color.black, Color.blue, Color.cyan, Color.gray, Color.green, Color.magenta, Color.red, Color.white, Color.yellow };
    float count = 0;
    Ruler ruler;
    public Text Score;
    private int score;

    private void Awake()
    {
        ssDirector = SSDirector.getInstance();
        ssDirector.setFPS(30);
        ssDirector.sceneController = this;
        ssDirector.sceneController.LoadResources();
        score = 0;
    }

    private void LoadResources()
    {
        diskFactory = Singleton<DiskFactory>.instance;
        diskFactory.initFactory();
    }

    private void Update()
    {
        count += Time.deltaTime;
        if(count >= 1f)
        {
            if(diskFactory.isPrepared())
            {
                ruler = GetRuler();
                diskFactory.getDisk(ruler);
            }
            count = 0;
        }

        diskFactory.freeDisk();

        if(Input.GetButtonDown("Fire1"))
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                score += hit.transform.gameObject.GetComponent<DiskData>().ruler.score;
                hit.transform.gameObject.GetComponent<DiskData>().gameObject.GetComponent<Transform>().position = new Vector3(0, -10, 0);
                print("Score: " + score);
                Score.text = "Score: " + score.ToString();
            }
        }
    }

    private void FixedUpdate()
    {
        diskFactory.runDisk();
    }

    private Ruler GetRuler()
    {
        float size = Random.Range(1, 4);
        Color color = colors[Random.Range(0, 9)];
        Vector3 position = new Vector3(
            Random.Range(-4f, 4f),
            -1,
            Random.Range(-2f, 2f));
        Vector3 direction = new Vector3(
            Random.Range(-100f, 100f),
            Random.Range(0f, 100f)+60,
            Random.Range(0f, 100f));
        direction.Normalize();
        direction = position - direction;
        float speed = Random.Range(1, 4);
        return new Ruler(size, color, position, direction, speed);
    }
}
