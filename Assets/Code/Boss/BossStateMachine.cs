using System.Collections;
using UnityEngine;

public class BossStateMachine : MonoBehaviour
{
    public enum BossState
    {
        Moving,
        Attacking
    }

    public BossState currentState;

    [Header("References")]
    public BossFly fly;

    public WaterBall waterBall;
    public Tongue tongue;
    public DashAttack dash;

    private bool attacking;

    void Start()
    {
        currentState = BossState.Moving;

        StartCoroutine(StateLoop());
    }

    IEnumerator StateLoop()
    {
        while (true)
        {
            switch (currentState)
            {
                case BossState.Moving:

                    yield return new WaitForSeconds(3f);

                    currentState = BossState.Attacking;
                    break;

                case BossState.Attacking:

                    if (!attacking)
                    {
                        attacking = true;

                        fly.canMove = false;

                        int random = Random.Range(0, 3);

                        switch (random)
                        {
                            case 0:
                                yield return StartCoroutine(waterBall.Attack());
                                break;

                            case 1:
                                yield return StartCoroutine(tongue.Attack());
                                break;

                            case 2:
                                yield return StartCoroutine(dash.Attack());
                                break;
                        }

                        fly.canMove = true;

                        attacking = false;

                        currentState = BossState.Moving;
                    }

                    break;
            }

            yield return null;
        }
    }
}