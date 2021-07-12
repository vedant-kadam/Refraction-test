using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof( LineRenderer))]
public class Reflect : MonoBehaviour
{
   
    public int reflection;
    public float maxLength;

    public LineRenderer lr;


    public float speed;

    public Vector3 wwater;

    private Vector3 moveDir;
    public Transform target;
    public Vector3 adjust;
    public float adjustRefreaction;
   
   

    private void Update()
    {
        RayReflections();
        moveDir.x = Input.GetAxis("Horizontal");
        moveDir.z = Input.GetAxis("Vertical");
        moveDir.y = 0;

        transform.Rotate(Vector3.up * moveDir.x * speed);
        

    }

    private void RayReflections()
    {
         Ray ray;//stores the ray
        RaycastHit hit; //hit info

        //cast a ray from the source
        ray = new Ray(transform.position, transform.forward);

        //set the initial point i.e the point of origin 
        lr.positionCount = 1;
        lr.SetPosition(0, transform.position);
        

        //max length we want are ray to be active for
        float remaingLenght = maxLength;
        for (int i = 0; i < reflection; i++)
        {
            if (Physics.Raycast(ray.origin, ray.direction, out hit, remaingLenght))
            {
               //if we hhit something set the new point of lr
                NewHitPoint(hit.point);//

                //ray for reflection
                 ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));

                if (hit.collider.CompareTag("water") )//game object where we want to add refraction
                {
                    adjust.x = hit.collider.bounds.size.x + adjustRefreaction;
                    NewHitPoint(hit.point - adjust);//adjust the value to see what refrection we want

                   
                    //ray for refraction
                    ray = new Ray(hit.point, Vector3.Reflect(ray.direction, ray.direction+wwater));



                }
                else  
                if (hit.collider.CompareTag("noReflection"))
                {
                    break;
                }
                
                
            }
            else
            {
                //if we are not hitting any Gameobject we simply draw the ray
                Vector3 newPoint = ray.origin + ray.direction * remaingLenght;
                NewHitPoint(newPoint);
            }
        }
    }


    //a function to  add new vertex to the lr
    private void NewHitPoint(Vector3 hitPoint)
    {
        lr.positionCount += 1;
        lr.SetPosition(lr.positionCount - 1, hitPoint);
    }
}
