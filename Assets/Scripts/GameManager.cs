using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

enum LevelState
{
    Play,
    Finishing,
    Finished
}

public class GameManager : MonoBehaviour
{
    public Transform player;
    public Transform[] path;
    public ObjectCollection objCollection;
    public Transform stairs;
    public Transform finishStairs;
    public Text scoreText;
    public Text timerText;
    LevelState levelState = LevelState.Play;
    public float playerSpeed = 5.0f;
    private float distThreshold = 1.0f;
    private float timer = 0f;
    private float stairSpawnTime = 0;
    private float finishStairSpawnTime = 0;
    private int currentPoint = 0;
    private int score = 0;
    // Update is called once per frame

    private void Start()
    {
        stairSpawnTime = stairs.localScale.y / (playerSpeed / 2f);
    }
    void Update()
    {
        scoreText.text = objCollection.TotalMoney.ToString();
        //Level end
        if (levelState == LevelState.Finished)
        {
            timer -= Time.deltaTime;
            timerText.text = timer.ToString("0");
            if (timer <= 0f)
            {
                currentPoint = 0;
                Scene activeScene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(activeScene.name);
            }
        }
        else
        {
            float dist = Vector3.Distance(path[currentPoint].position, player.position);
            player.position = Vector3.MoveTowards(player.position, path[currentPoint].position, playerSpeed * Time.deltaTime);
            if (dist <= distThreshold)
            {
                //before the last point
                if (currentPoint == path.Length - 2)
                {
                    levelState = LevelState.Finishing;
                    playerSpeed *= 2;
                    timer = 0;
                    float finishFloor = objCollection.TotalMoney / (objCollection.BanknoteValue * 2);
                    if (finishFloor > 20) finishFloor = 20;
                    float reachedScoreX = finishFloor * finishStairs.localScale.x + finishStairs.parent.position.x - (finishStairs.localScale.x / 2);
                    float reachedScoreY = finishFloor * (finishStairs.localScale.y) + (player.localScale.y / 2);
                    path[currentPoint + 1].position = new Vector3(reachedScoreX, reachedScoreY);
                    float finishStairDistance = Vector3.Distance(path[currentPoint].position, path[currentPoint + 1].position);
                    finishStairSpawnTime = finishStairDistance / playerSpeed / finishFloor;
                }

                currentPoint++;

                if (currentPoint >= path.Length)
                {
                    levelState = LevelState.Finished;
                    timer = 2f;
                }
            }
        }

        if (levelState == LevelState.Finishing)
        {
            SpawnStairs(finishStairSpawnTime);
            score += objCollection.BanknoteValue * 2;
            scoreText.text = score.ToString();
        }

        if (Input.GetMouseButton(0) && objCollection.TotalMoney > 0 && levelState == LevelState.Play)
        {
            SpawnStairs(stairSpawnTime);
            score -= objCollection.BanknoteValue;
            scoreText.text = score.ToString();
        }

        //reached

    }

    private void RiseObject(Transform obj)
    {
        Vector3 destPos = new Vector3(obj.position.x, obj.position.y + stairs.localScale.y, obj.position.z);
        obj.position = Vector3.MoveTowards(obj.position, destPos, playerSpeed * Time.deltaTime);
    }

    private void SpawnStairs(float spawnTime)
    {
        Vector3 stairSpawnPos = new Vector3(player.position.x, player.position.y - player.localScale.y, player.position.z);
        timer += Time.deltaTime;
        if (timer >= spawnTime)
        {
            timer = 0;
            RiseObject(player);
            var newStairs = Instantiate(stairs, stairSpawnPos, Quaternion.identity);
            Destroy(newStairs.gameObject, 2);
            objCollection.RemoveMoneyFromMoneyStack(2);
        }
    }
}
