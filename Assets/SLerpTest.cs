using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SLerpTest : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Transform target;

    void Start()
    {
        StartCoroutine(Move());
    }

    // Update is called once per frame
    void Update()
    {
    }


    private IEnumerator Move()
    {
        var time = 2f;
        var passedTime = 0f;
        var startPos = transform.position;

        while (true)
        {
            var pos = Vector3.Slerp(startPos, target.position, passedTime / time);
            passedTime += Time.deltaTime;

            transform.position = pos;

            yield return null;

            if (passedTime >= 2f)
            {
                yield break;
            }
        }
    }
}