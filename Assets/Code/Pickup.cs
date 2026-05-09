//using UnityEngine;
//using UnityEngine.SocialPlatforms.Impl;

//public class Pickup : MonoBehaviour
//{
//    public Player player;
//    public GameObject shotgundrop;

//    private void OnCollisionEnter(Collision collision)
//    {
//        if (collision.gameObject.tag == "Shotgun")
//        {
//            player.Gotgun2 = true;
//            Destroy(collision.gameObject);
//        }
//        if (player.hp < 10)
//        {
//            if (collision.gameObject.tag == "heal1")
//            {
//                player.hp += 2;
//                Destroy(collision.gameObject);
//            }
//            else if (collision.gameObject.tag == "heal2")
//            {
//                player.hp += 5;
//                Destroy(collision.gameObject);
//            }
//            else if (collision.gameObject.tag == "heal3")
//            {
//                player.hp += 8;
//                Destroy(collision.gameObject);
//            }
//            else
//            {

//            }
//        } 
        
//    }
//}
