using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "BlindBox", menuName = "Items/BlindBox")]
public class BlindBox : ScriptableObject
{
    public string Open()
    {
        int rng = Random.Range(0, 100);
        string result = "";

        bool wasBuffed = false;

        if (rng < 40)
        {
            int heal = Random.Range(10, 20);
            PlayerHealth.Instance.Heal(heal);
            result = $"Hồi máu +{heal}";
            wasBuffed = true;
        }
        else if (rng < 70)
        {
            int armor = Random.Range(5, 15);
            PlayerHealth.Instance.AddArmor(armor);
            result = $"Tăng giáp +{armor}";
            wasBuffed = true;
        }
        else if (rng < 90)
        {
            PlayerController.Instance.moveSpeed += 0.5f;
            result = "Tăng tốc độ di chuyển!";
            wasBuffed = true;
        }
        else
        {
            int damage = Random.Range(5, 15);
            PlayerHealth.Instance.TakeDamage(damage);
            result = $"Debuff - mất {damage} máu";
            wasBuffed = false; // không buff thì khỏi debuff thêm
        }

        // Nếu đã buff thì thêm 1 debuff phụ nữa
        if (wasBuffed)
        {
            int debuffType = Random.Range(0, 2);
            string debuffText = "";

            switch (debuffType)
            {
                case 0:
                    int damage = Random.Range(5, 10);
                    PlayerHealth.Instance.TakeDamage(damage);
                    debuffText = $" mất {damage} máu.";
                    break;
                case 1:
                    PlayerController.Instance.moveSpeed = Mathf.Max(1f, PlayerController.Instance.moveSpeed - 0.2f);
                    debuffText = " Tốc độ giảm nhẹ.";
                    break;
            }

            result += debuffText;
        }

        return result;
    }

}


