// using System.Collections;
// using UnityEngine;

// [CreateAssetMenu(fileName = "BlindBox", menuName = "Items/BlindBox")]
// public class BlindBox : ScriptableObject
// {
//     public string Open()
//     {
//         int rng = Random.Range(0, 100);
//         string result = "";

//         bool wasBuffed = false;

//         if (rng < 40)
//         {
//             int heal = Random.Range(10, 20);
//             PlayerHealth.Instance.Heal(heal);
//             result = $"Hồi máu +{heal}";
//             wasBuffed = true;
//         }
//         else if (rng < 70)
//         {
//             int armor = Random.Range(5, 15);
//             PlayerHealth.Instance.AddArmor(armor);
//             result = $"Tăng giáp +{armor}";
//             wasBuffed = true;
//         }
//         else if (rng < 90)
//         {
//             PlayerController.Instance.moveSpeed += 0.5f;
//             result = "Tăng tốc độ di chuyển!";
//             wasBuffed = true;
//         }
//         else
//         {
//             int damage = Random.Range(5, 15);
//             PlayerHealth.Instance.TakeDamage(damage);
//             result = $"Debuff - mất {damage} máu";
//             wasBuffed = false; // không buff thì khỏi debuff thêm
//         }

//         // Nếu đã buff thì thêm 1 debuff phụ nữa
//         if (wasBuffed)
//         {
//             int debuffType = Random.Range(0, 2);
//             string debuffText = "";

//             switch (debuffType)
//             {
//                 case 0:
//                     int damage = Random.Range(5, 10);
//                     PlayerHealth.Instance.TakeDamage(damage);
//                     debuffText = $" mất {damage} máu.";
//                     break;
//                 case 1:
//                     PlayerController.Instance.moveSpeed = Mathf.Max(1f, PlayerController.Instance.moveSpeed - 0.2f);
//                     debuffText = " Tốc độ giảm nhẹ.";
//                     break;
//             }

//             result += debuffText;
//         }

//         return result;
//     }

// }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlindBox", menuName = "Items/BlindBox")]
public class BlindBox : ScriptableObject
{
    public string Open()
    {
        int rng = Random.Range(0, 100);
        string result = "";

        bool wasBuffed = false;
        List<int> availableDebuffs = new List<int> { 0, 1, 2 }; // 0: Damage, 1: Reduce Armor, 2: Reduce Speed

        if (rng < 40)
        {
            int heal = Random.Range(10, 20);
            PlayerHealth.Instance.Heal(heal);
            result = $"Hồi máu +{heal}";
            wasBuffed = true;

            // Remove "Damage" debuff to avoid conflict
            availableDebuffs.Remove(0);
        }
        else if (rng < 70)
        {
            int armor = Random.Range(5, 15);
            PlayerHealth.Instance.AddArmor(armor);
            result = $"Tăng giáp +{armor}";
            wasBuffed = true;

            // Remove "Reduce Armor" debuff to avoid conflict
            availableDebuffs.Remove(1);
        }
        else if (rng < 90)
        {
            PlayerController.Instance.moveSpeed += 0.5f;
            result = "Tăng tốc độ di chuyển!";
            wasBuffed = true;

            // Remove "Reduce Speed" debuff to avoid conflict
            availableDebuffs.Remove(2);
        }
        else
        {
            int damage = Random.Range(5, 15);
            PlayerHealth.Instance.TakeDamage(damage);
            result = $"Debuff - mất {damage} máu";
            wasBuffed = false;
        }

        // Nếu đã buff thì thêm 1 debuff phụ nữa, chọn ngẫu nhiên trong các debuff không bị loại
        if (wasBuffed && availableDebuffs.Count > 0)
        {
            int debuffType = availableDebuffs[Random.Range(0, availableDebuffs.Count)];
            string debuffText = "";

            switch (debuffType)
            {
                case 0:
                    int damage = Random.Range(5, 10);
                    PlayerHealth.Instance.TakeDamage(damage);
                    debuffText = $" mất {damage} máu.";
                    break;
                case 1:
                    int armorLoss = Random.Range(3, 7);
                    PlayerHealth.Instance.ReduceArmor(armorLoss); // Giả sử có hàm ReduceArmor
                    debuffText = $" Mất {armorLoss} giáp.";
                    break;
                case 2:
                    PlayerController.Instance.moveSpeed = Mathf.Max(1f, PlayerController.Instance.moveSpeed - 0.2f);
                    debuffText = " Tốc độ giảm nhẹ.";
                    break;
            }

            result += debuffText;
        }

        return result;
    }
}
