// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;

// public class InventoryManager : MonoBehaviour
// {
//     public static InventoryManager Instance;
//     public List<BlindBox> blindBoxes = new List<BlindBox>();
//     public GameObject BlindBoxItems;

//     public TextMeshProUGUI blindBoxCountText;
//     public TextMeshProUGUI effectDisplayText;

//     private void Awake()
//     {
//         Instance = this;
//     }

//     private void Update() 
//     {
//         if (Input.GetKeyDown(KeyCode.Alpha1))
//         {
//             OpenBox(0);
//         }
//     }

//     public void AddBlindBox(BlindBox box)
//     {
//         blindBoxes.Add(box);
//         UpdateBlindBoxCount();

//         if (blindBoxes.Count > 0)
//         {
//             SoundManager.PlaySound(SoundType.HealthPickUp);
//             BlindBoxItems.SetActive(true); // bật panel nếu có ít nhất 1 hộp
//         }
//     }

//     public void OpenBox(int index)
//     {
//         if (index >= 0 && index < blindBoxes.Count)
//         {
//             SoundManager.PlaySound(SoundType.HealthPickUp);
//             string effect = blindBoxes[index].Open();
//             blindBoxes.RemoveAt(index);
//             UpdateBlindBoxCount();

//             if (effectDisplayText != null)
//             {
//                 StartCoroutine(ShowEffectThenReset(effect));
//             }

//             if (blindBoxes.Count == 0)
//             {
//                 StartCoroutine(DelaySetActiveFalse());
//             }
//         }
//     }

//     private IEnumerator ShowEffectThenReset(string effect)
//     {
//         effectDisplayText.text = effect;
//         yield return new WaitForSeconds(1.5f);
//         effectDisplayText.text = "Inventory";
//     }

//     private void UpdateBlindBoxCount()
//     {
//         if (blindBoxCountText != null)
//         {
//             blindBoxCountText.text = "X" + blindBoxes.Count.ToString();
//         }
//     }

//     private IEnumerator DelaySetActiveFalse()
//     {
//         yield return new WaitForSeconds(2f);
//         BlindBoxItems.SetActive(false);
//     }
// }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    // List chứa tất cả các UI nút
    public List<GameObject> uiSlots = new List<GameObject>(); // 6 nút UI
    public List<BlindBox> blindBoxes = new List<BlindBox>(); // Danh sách các BlindBox
    public int maxBlindBoxes = 6; // Số lượng tối đa BlindBox

    public TextMeshProUGUI effectDisplayText; // Hiển thị kết quả khi mở hộp

    private int currentBoxIndex = 0; // Chỉ số hộp đang mở

    private void Awake()
    {
        Instance = this;
        // Ban đầu tắt tất cả các UI slot
        foreach (var slot in uiSlots)
        {
            slot.SetActive(false);
        }
    }

    private void Update()
    {
        // Nhấn nút 1 để mở hộp tiếp theo
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OpenNextBox();
        }
    }

    public void AddBlindBox(BlindBox box)
    {
        if (blindBoxes.Count < maxBlindBoxes)
        {
            // Thêm BlindBox vào danh sách
            blindBoxes.Add(box);

            // Bật UI tương ứng với BlindBox mới nhặt
            int index = blindBoxes.Count - 1; // Lấy chỉ số hộp mới nhặt
            if (index < uiSlots.Count)
            {
                uiSlots[index].SetActive(true); // Bật UI của slot trống
            }

            // Nếu inventory đầy, thì không nhặt thêm BlindBox
            if (blindBoxes.Count >= maxBlindBoxes)
            {
                Debug.Log("Inventory is full. Cannot pick up more BlindBoxes.");
            }
        }
        else
        {
            Debug.Log("Inventory Full! Không thể nhặt thêm hộp.");
        }
    }

    // Mở hộp tiếp theo trong inventory
    public void OpenNextBox()
    {
        // Kiểm tra còn hộp chưa mở không
        if (blindBoxes.Count > 0)
        {
            // Lấy hộp tiếp theo trong danh sách
            BlindBox boxToOpen = blindBoxes[currentBoxIndex];

            // Mở BlindBox
            string effect = boxToOpen.Open();

            // Hiển thị effect khi mở hộp
            StartCoroutine(ShowEffectThenReset(effect));

            // Tắt UI của hộp vừa mở
            uiSlots[currentBoxIndex].SetActive(false);

            // Xóa BlindBox đã mở khỏi danh sách
            blindBoxes.RemoveAt(currentBoxIndex);

            // Cập nhật lại UI sau khi xóa BlindBox
            UpdateInventoryUI();

            // Không tăng chỉ số, vì hiện tại hộp sau sẽ tự động tiếp theo
        }
        else
        {
            Debug.Log("Không còn hộp nào để mở.");
        }
    }

    // Cập nhật lại UI sau khi thay đổi inventory
    private void UpdateInventoryUI()
    {
        // Bật lại tất cả UI cho các BlindBox còn lại
        for (int i = 0; i < blindBoxes.Count; i++)
        {
            uiSlots[i].SetActive(true);
        }

        // Ẩn các UI không còn BlindBox
        for (int i = blindBoxes.Count; i < uiSlots.Count; i++)
        {
            uiSlots[i].SetActive(false);
        }
    }

    private IEnumerator ShowEffectThenReset(string effect)
    {
        effectDisplayText.text = effect;
        yield return new WaitForSeconds(2f); // Hiển thị kết quả trong 2 giây
        effectDisplayText.text = "Inventory"; // Reset text về "Inventory"
    }
}
