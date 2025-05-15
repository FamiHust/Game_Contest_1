using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    // List chứa tất cả các UI nút (6 nút tương ứng với 6 hộp)
    public List<GameObject> uiSlots = new List<GameObject>();
    public List<BlindBox> blindBoxes = new List<BlindBox>();
    public int maxBlindBoxes = 6;

    public TextMeshProUGUI effectDisplayText;

    private void Awake()
    {
        Instance = this;

        // Tắt tất cả UI và clear sự kiện click
        for (int i = 0; i < uiSlots.Count; i++)
        {
            uiSlots[i].SetActive(false);
            Button btn = uiSlots[i].GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();
            }
        }
    }

    private void Update()
    {
        // Giữ lại phần này nếu muốn debug bằng phím 1
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OpenNextBox();
        }
    }

    public void AddBlindBox(BlindBox box)
    {
        if (blindBoxes.Count < maxBlindBoxes)
        {
            blindBoxes.Add(box);
            int index = blindBoxes.Count - 1;

            if (index < uiSlots.Count)
            {
                SoundManager.PlaySound(SoundType.HealthPickUp);
                uiSlots[index].SetActive(true);

                // Gán sự kiện click cho button tương ứng
                Button btn = uiSlots[index].GetComponent<Button>();
                if (btn != null)
                {
                    int capturedIndex = index; // bắt đúng chỉ số
                    btn.onClick.RemoveAllListeners(); // tránh trùng lặp
                    btn.onClick.AddListener(() => OpenBoxAtIndex(capturedIndex));
                }
            }

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

    public void OpenNextBox()
    {
        if (blindBoxes.Count > 0)
        {
            SoundManager.PlaySound(SoundType.HealthPickUp);
            BlindBox boxToOpen = blindBoxes[0];
            string effect = boxToOpen.Open();

            StartCoroutine(ShowEffectThenReset(effect));

            uiSlots[0].SetActive(false);
            blindBoxes.RemoveAt(0);

            UpdateInventoryUI();
        }
        else
        {
            Debug.Log("Không còn hộp nào để mở.");
        }
    }

    // Hàm mới: mở hộp tại chỉ số cụ thể
    public void OpenBoxAtIndex(int index)
    {
        if (index >= 0 && index < blindBoxes.Count)
        {
            SoundManager.PlaySound(SoundType.HealthPickUp);
            BlindBox boxToOpen = blindBoxes[index];
            string effect = boxToOpen.Open();

            StartCoroutine(ShowEffectThenReset(effect));

            uiSlots[index].SetActive(false);
            blindBoxes.RemoveAt(index);

            UpdateInventoryUI();
        }
        else
        {
            Debug.Log("Không có hộp ở vị trí này.");
        }
    }

    private void UpdateInventoryUI()
    {
        for (int i = 0; i < uiSlots.Count; i++)
        {
            if (i < blindBoxes.Count)
            {
                uiSlots[i].SetActive(true);

                Button btn = uiSlots[i].GetComponent<Button>();
                if (btn != null)
                {
                    int capturedIndex = i;
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(() => OpenBoxAtIndex(capturedIndex));
                }
            }
            else
            {
                uiSlots[i].SetActive(false);
            }
        }
    }

    private IEnumerator ShowEffectThenReset(string effect)
    {
        effectDisplayText.text = effect;
        yield return new WaitForSeconds(2f);
        effectDisplayText.text = "Inventory";
    }
}
