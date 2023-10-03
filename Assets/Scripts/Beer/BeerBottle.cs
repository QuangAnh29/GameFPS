using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerBottle : MonoBehaviour
{
    public List<Rigidbody> allParts = new List<Rigidbody>();

    public void Shatter()
    {
        foreach (Rigidbody part in allParts)
        {
            part.isKinematic = false;
        }

        foreach (Rigidbody part in allParts)
        {
            if (part != null) // Kiểm tra xem mảnh vỡ còn tồn tại trước khi xóa
            {
                Destroy(part.gameObject, 3f);
            }
        }
    }
}
