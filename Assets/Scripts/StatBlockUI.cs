using UnityEngine;
using UnityEngine.UIElements;

// Attach this script to a GameObject that has a UIDocument component.
// It finds three SliderInt elements in the UI and lets you:
//  - Up/Down to change which slider is selected
//  - Left/Right to decrease/increase the selected slider's value
public class StatSliderController : MonoBehaviour
{
    // Optional: assign in inspector. If left null, the script will try to auto-get the UIDocument on this GameObject.
    public UIDocument uiDocument;

    // Names of the three sliders in the UXML (change to match your UXML element names)
    [SerializeField] string[] sliderNames = new string[] { "Health", "Speed", "Jump" };

    // Runtime references to the sliders
    private SliderInt[] sliders;

    // Which slider index is currently selected (0..2)
    private int selectedIndex = 0;

    // CSS class name used to highlight selected slider in the UI
    private const string selectedClass = "selected";

    void Awake()
    {
        // If the inspector didn't set a UIDocument, try to get one from the same GameObject.
        if (uiDocument == null)
            uiDocument = GetComponent<UIDocument>();

        if (uiDocument == null)
        {
            Debug.LogError("StatSliderController: No UIDocument found. Attach the script to the GameObject with the UIDocument or assign one in inspector.");
            enabled = false;
            return;
        }

        // Prepare the sliders array
        int count = Mathf.Min(3, sliderNames.Length);
        sliders = new SliderInt[count];

        // Grab each SliderInt by name from the UI's rootVisualElement
        var root = uiDocument.rootVisualElement;
        for (int i = 0; i < count; i++)
        {
            var s = root.Q<SliderInt>(sliderNames[i]);
            if (s == null)
            {
                Debug.LogWarning($"StatSliderController: Could not find SliderInt named '{sliderNames[i]}' in the UI.");
                continue;
            }

            sliders[i] = s;

            // Optional: ensure each slider is whole-number friendly (SliderInt already is) and set defaults
            // You can change the lowValue/highValue here if you like:
            // s.lowValue = 0;
            // s.highValue = 100;

            // If you want to visually show the numeric value too, you could connect an IntegerField or label here.
        }

        // Make sure the first slider is highlighted
        UpdateHighlight();
    }

    void Update()
    {
        // Switch selected slider with Up/Down arrows (detect key down once)
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SelectPrevious();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SelectNext();
        }

        // Adjust selected slider's value with Left/Right arrows
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            AdjustSelected(-1);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            AdjustSelected(+1);
    }

    void SelectPrevious()
    {
        int old = selectedIndex;
        selectedIndex = Mathf.Max(0, selectedIndex - 1);
        if (selectedIndex != old)
            UpdateHighlight();
    }

    void SelectNext()
    {
        int old = selectedIndex;
        selectedIndex = Mathf.Min(sliders.Length - 1, selectedIndex + 1);
        if (selectedIndex != old)
            UpdateHighlight();
    }

    // delta is +1 or -1 (or any integer step)
    void AdjustSelected(int delta)
    {
        if (sliders == null || sliders.Length == 0) return;
        var s = sliders[selectedIndex];
        if (s == null) return;

        int newValue = s.value + delta;

        // Clamp the value between the slider's low and high values
        newValue = Mathf.Clamp(newValue, s.lowValue, s.highValue);

        s.value = newValue;
    }

    // Visually mark which slider is selected by toggling a USS class
    void UpdateHighlight()
    {
        var root = uiDocument.rootVisualElement;
        for (int i = 0; i < sliders.Length; i++)
        {
            var s = sliders[i];
            if (s == null) continue;
            if (i == selectedIndex)
                s.AddToClassList(selectedClass);
            else
                s.RemoveFromClassList(selectedClass);
        }
    }
}
