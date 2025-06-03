using UnityEngine;

public class Mission_ObjectCarDeliveryZone : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        Car_Controller car = other.GetComponent<Car_Controller>();

        if (car != null)
        {
            if(car.GetComponent<Mission_ObjectCarToDeliver>())
            {
                car.GetComponent<Mission_ObjectCarToDeliver>().InvokeOnCarDelivery();
            }
            

        }

    }
}
