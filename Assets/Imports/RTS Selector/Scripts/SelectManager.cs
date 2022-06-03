/*************************************************************
 * Created by Sun Dryd Studios                               *
 * For the Unity Asset Store                                 *
 * This asset falls under the "Creative Commons License"     *
 * For support email sundrysdtudios@gmail.com                *
 *************************************************************/

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SelectManager : MonoBehaviour
{
    [Tooltip("The camera used for highlighting")]
    public Camera selectCam;
    [Tooltip("The rectangle to modify for selection")]
    public RectTransform SelectingBoxRect;

    private Rect SelectingRect;
    private Vector3 SelectingStart;

    [Tooltip("Changes the minimum square before selecting characters. Needed for single click select")]
    public float minBoxSizeBeforeSelect = 10f;
    public float selectUnderMouseTimer = 0.1f;
    private float selectTimer = 0f;

    public List<SelectableCharacter> selectableChars = new List<SelectableCharacter>();
    private List<SelectableCharacter> selectedArmy = new List<SelectableCharacter>();

    private void Start()
    {
        if (!SelectingBoxRect)
            SelectingBoxRect = GetComponent<RectTransform>();
    }

    void Update()
    {

        if (SelectingBoxRect == null)
        {
            Debug.LogError("There is no Rect Transform to use for selection!");
            return;
        }

        if (Input.GetKeyDown(KeyCode.C))
            ReSelect();

        //The input for triggering selecting. This can be changed
        if (Input.GetMouseButtonDown(0))
        {

            //Sets up the screen box
            var cord = GetNormalizedCursorCoordinates();
            SelectingStart = new Vector3(cord.Item1, cord.Item2, 0);

            SelectingBoxRect.anchoredPosition = new Vector2(cord.Item1, cord.Item2);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            selectTimer = 0f;
        }

        if (Input.GetMouseButton(0))
        {
            SelectingArmy();
            selectTimer += Time.deltaTime;

            //Only check if there is a character under the mouse for a fixed time
            if (selectTimer <= selectUnderMouseTimer)
            {
                CheckIfUnderMouse();
            }
        }
        else
        {
            SelectingBoxRect.sizeDelta = new Vector2(0, 0);
        }
    }

    private (float, float) GetNormalizedCursorCoordinates()
    {
        var cursCoord = Input.mousePosition;
        var cursorCoord = (cursCoord.x, cursCoord.y);
        return GetNormalizedCoordinates(cursorCoord);
    }

    private (float, float) GetNormalizedCoordinates((float, float) coord)
    {
        return ((coord.Item1 / Screen.width) * 1920,
                        (coord.Item2 / Screen.height) * 1080);
    }

    //Resets what is currently being selected
    void ReSelect()
    {
        for (int i = 0; i <= (selectedArmy.Count - 1); i++)
        {
            selectedArmy[i].TurnOffSelector();
            selectedArmy.Remove(selectedArmy[i]);
        }
    }

    //Does the calculation for mouse dragging on screen
    //Moves the UI pivot based on the direction the mouse is going relative to where it started
    //Update: Made this a bit more legible
    void SelectingArmy()
    {
        Vector2 _pivot = Vector2.zero;
        Vector3 _sizeDelta = Vector3.zero;
        Rect _rect = Rect.zero;

        //Controls x's of the pivot, sizeDelta, and rect
        if (-(SelectingStart.x - (Input.mousePosition.x / Screen.width) * 1920) > 0)
        {
            _sizeDelta.x = -(SelectingStart.x - (Input.mousePosition.x / Screen.width) * 1920);
            _rect.x = SelectingStart.x;
        }
        else
        {
            _pivot.x = 1;
            _sizeDelta.x = (SelectingStart.x - (Input.mousePosition.x / Screen.width) * 1920);
            _rect.x = SelectingStart.x - SelectingBoxRect.sizeDelta.x;
        }

        //Controls y's of the pivot, sizeDelta, and rect
        if (SelectingStart.y - (Input.mousePosition.y / Screen.height) * 1080 > 0)
        {
            _pivot.y = 1;
            _sizeDelta.y = SelectingStart.y - (Input.mousePosition.y / Screen.height) * 1080;
            _rect.y = SelectingStart.y - SelectingBoxRect.sizeDelta.y;
        }
        else
        {
            _sizeDelta.y = -(SelectingStart.y - (Input.mousePosition.y / Screen.height) * 1080);
            _rect.y = SelectingStart.y;
        }

        //Sets pivot if of UI element
        if (SelectingBoxRect.pivot != _pivot)
            SelectingBoxRect.pivot = _pivot;

        //Sets the size
        SelectingBoxRect.sizeDelta = _sizeDelta;

        //Finished the Rect set up then set rect
        _rect.height = SelectingBoxRect.sizeDelta.x;
        _rect.width = SelectingBoxRect.sizeDelta.y;
        SelectingRect = _rect;

        //Only does a select check if the box is bigger than the minimum size.
        //While checking it messes with single click
        if (_rect.height > minBoxSizeBeforeSelect && _rect.width > minBoxSizeBeforeSelect)
        {
            CheckForSelectedCharacters();
        }
    }

    //Checks if the correct characters can be selected and then "selects" them
    void CheckForSelectedCharacters()
    {
        var tselectableChars = Resources.AllObjects.Where(x => x != null)
                            .Select(x => x.transform.gameObject.GetComponentInChildren<SelectableCharacter>());

        foreach (var soldier in tselectableChars.Where(x => x != null))
        {
            var t = selectCam.WorldToScreenPoint(soldier.transform.position);
            var cord = GetNormalizedCoordinates((t.x, t.y));
            var screenPos = new Vector3(cord.Item1, cord.Item2);

            if (SelectingRect.Contains(screenPos))
            {
                if (!selectedArmy.Contains(soldier))
                    selectedArmy.Add(soldier);

                soldier.TurnOnSelector();
            }
            else
            {
                soldier.TurnOffSelector();

                if (selectedArmy.Contains(soldier))
                    selectedArmy.Remove(soldier);
            }
        }
    }

    //Checks if there is a character under the mouse that is on the Selectable list
    void CheckIfUnderMouse()
    {
        RaycastHit hit;
        var cord = GetNormalizedCursorCoordinates();
        var vector = new Vector3(cord.Item1, cord.Item2, 0);
        var ray = selectCam.ScreenPointToRay(vector);

        //Raycast from mouse and select character if its hit!
        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (hit.transform != null)
            {
                var selectChar = hit.transform.gameObject.GetComponentInChildren<SelectableCharacter>();
                if (selectChar != null)
                {
                    selectedArmy.Add(selectChar);
                    selectChar.TurnOnSelector();
                }
            }
        }
    }
}
