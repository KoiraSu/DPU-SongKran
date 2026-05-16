using System.Collections;
using UnityEngine;

public class Skill3 : MonoBehaviour
{
    public IEnumerator Attack()
    {

        // พักหลังยิง
        yield return new WaitForSeconds(1f);
    }
}
