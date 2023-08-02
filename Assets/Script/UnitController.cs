using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;


    public class UnitController : MonoBehaviourSingleton<UnitController>
    {
        //Add all units in the scene to this array
        public List<Unit> allUnits;
        //The selection square we draw when we drag the mouse to select units
        public RectTransform selectionSquareTrans;
        //To test the square's corners
        public Transform sphere1;
        public Transform sphere2;
        public Transform sphere3;
        public Transform sphere4;
        //The materials
        public Material normalMaterial;
        public Material highlightMaterial;
        public Material selectedMaterial;
   
        //All currently selected units
        [System.NonSerialized]
        public List<Unit> selectedUnits = new List<Unit>();

        //We have hovered above this unit, so we can deselect it next update
        //and dont have to loop through all units
        GameObject highlightThisUnit;

        //To determine if we are clicking with left mouse or holding down left mouse
        float delay = 0.3f;
        float clickTime = 0f;
        //The start and end coordinates of the square we are making
        Vector3 squareStartPos;
        Vector3 squareEndPos;
        //If it was possible to create a square
        bool hasCreatedSquare;
        //The selection squares 4 corner positions
        Vector3 TL, TR, BL, BR;
        private bool showRealPosOfRectangle;

        void Start()
        {
            //Deactivate the square selection image
            selectionSquareTrans.gameObject.SetActive(false);
        }
            
        void Update()
        {
            //Select one or several units by clicking or draging the mouse
            SelectUnits();

            //Highlight by hovering with mouse above a unit which is not selected
            HighlightUnit();
        }

        //Select units with click or by draging the mouse
        void SelectUnits()
        {
            //Are we clicking with left mouse or holding down left mouse
            bool isClicking = false;
            bool isHoldingDown = false;

            //Click the mouse button
            if (Input.GetMouseButtonDown(0))
            {
                clickTime = Time.time;

                //We dont yet know if we are drawing a square, but we need the first coordinate in case we do draw a square
                RaycastHit hit;
                //Fire ray from camera
             
                    //The corner position of the square
                    squareStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    
            }
            //Release the mouse button
            if (Input.GetMouseButtonUp(0))
            {
                if (Time.time - clickTime <= delay)
                {
                    isClicking = true;
                }

                //Select all units within the square if we have created a square
            

                    //Deactivate the square selection image
                    selectionSquareTrans.gameObject.SetActive(false);

                    //Clear the list with selected unit
                    selectedUnits.Clear();

                    //Select the units
                    
                    for (int i = 0; i < allUnits.Count; i++)
                    {
                        Unit currentUnit = allUnits[i];

                //Is this unit within the square
                if (currentUnit == null) return;
                        if (IsWithinPolygon(currentUnit.transform.position))
                        {
                            
                            currentUnit.GetComponent<SpriteRenderer>().material = selectedMaterial;

                            selectedUnits.Add(currentUnit);
                        }
                        //Otherwise deselect the unit if it's not in the square
                        else
                        {
                            currentUnit.GetComponent<SpriteRenderer>().material = normalMaterial;
                        }
                    }
                

            }
            //Holding down the mouse button
            if (Input.GetMouseButton(0))
            {
                if (Time.time - clickTime > delay)
                {
                    isHoldingDown = true;
                }
            }

            //Select one unit with left mouse and deselect all units with left mouse by clicking on what's not a unit
            if (isClicking)
            {
                //Deselect all units
                for (int i = 0; i < selectedUnits.Count; i++)
                {
                    selectedUnits[i].GetComponent<SpriteRenderer>().material = normalMaterial;
                }

                //Clear the list with selected units
                selectedUnits.Clear();

                //Try to select a new unit
                RaycastHit hit;
                //Fire ray from camera
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 200f))
                {
                    //Did we hit a friendly unit?
                    if (hit.collider.CompareTag("Friendly"))
                    {
                        GameObject activeUnit = hit.collider.gameObject;
                        //Set this unit to selected
                        activeUnit.GetComponent<SpriteRenderer>().material = selectedMaterial;
                        //Add it to the list of selected units, which is now just 1 unit
                        selectedUnits.Add(activeUnit.GetComponent<Unit>());
                    }
                }
            }

            //Drag the mouse to select all units within the square
            if (isHoldingDown)
            {
                //Activate the square selection image
                if (!selectionSquareTrans.gameObject.activeInHierarchy)
                {
                    selectionSquareTrans.gameObject.SetActive(true);
                }

                //Get the latest coordinate of the square
                squareEndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                //Display the selection with a GUI image
                DisplaySquare();

                //Highlight the units within the selection square, but don't select the units
             
                    for (int i = 0; i < allUnits.Count; i++)
                    {
                        Unit currentUnit = allUnits[i];

                //Is this unit within the square
                if (currentUnit == null) return;
                        if (IsWithinPolygon(currentUnit.transform.position))
                        {
                            currentUnit.GetComponent<SpriteRenderer>().material = highlightMaterial;
                        }
                        //Otherwise deactivate
                        else
                        {
                            currentUnit.GetComponent<SpriteRenderer>().material = normalMaterial;
                        }
                    }
                
            }
        }

        //Highlight a unit when mouse is above it
        void HighlightUnit()
        {
            //Change material on the latest unit we highlighted
            if (highlightThisUnit != null)
            {
                //But make sure the unit we want to change material on is not selected
                bool isSelected = false;
                for (int i = 0; i < selectedUnits.Count; i++)
                {
                    if (selectedUnits[i] == highlightThisUnit)
                    {
                        isSelected = true;
                        break;
                    }
                }

                if (!isSelected)
                {
                    highlightThisUnit.GetComponent<SpriteRenderer>().material = normalMaterial;
                }

                highlightThisUnit = null;
            }

            //Fire a ray from the mouse position to get the unit we want to highlight
            RaycastHit hit;
            //Fire ray from camera
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 200f))
            {
                //Did we hit a friendly unit?
                if (hit.collider.CompareTag("Friendly"))
                {
                    //Get the object we hit
                    GameObject currentObj = hit.collider.gameObject;

                    //Highlight this unit if it's not selected
                    bool isSelected = false;
                    for (int i = 0; i < selectedUnits.Count; i++)
                    {
                        if (selectedUnits[i] == currentObj)
                        {
                            isSelected = true;
                            break;
                        }
                    }

                    if (!isSelected)
                    {
                        highlightThisUnit = currentObj;

                        highlightThisUnit.GetComponent<SpriteRenderer>().material = highlightMaterial;
                    }
                }
            }
        }

        //Is a unit within a polygon determined by 4 corners
        bool IsWithinPolygon(Vector3 unitPos)
        {
            bool isWithinPolygon = false;

            //The polygon forms 2 triangles, so we need to check if a point is within any of the triangles
            //Triangle 1: TL - BL - TR
            if (IsWithinTriangle(unitPos, TL, BL, TR))
            {
                return true;
            }

            //Triangle 2: TR - BL - BR
            if (IsWithinTriangle(unitPos, TR, BL, BR))
            {
                return true;
            }

            return isWithinPolygon;
        }

        //Is a point within a triangle
        //From http://totologic.blogspot.se/2014/01/accurate-point-in-triangle-test.html
        bool IsWithinTriangle(Vector3 p, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            bool isWithinTriangle = false;

            //Need to set z -> y because of other coordinate system
            float denominator = ((p2.y - p3.y) * (p1.x - p3.x) + (p3.x - p2.x) * (p1.y - p3.y));

            float a = ((p2.y - p3.y) * (p.x - p3.x) + (p3.x - p2.x) * (p.y - p3.y)) / denominator;
            float b = ((p3.y - p1.y) * (p.x - p3.x) + (p1.x - p3.x) * (p.y - p3.y)) / denominator;
            float c = 1 - a - b;

            //The point is within the triangle if 0 <= a <= 1 and 0 <= b <= 1 and 0 <= c <= 1
            if (a >= 0f && a <= 1f && b >= 0f && b <= 1f && c >= 0f && c <= 1f)
            {
                isWithinTriangle = true;
            }

            return isWithinTriangle;
        }

        //Display the selection with a GUI square
        void DisplaySquare()
        {
            //The start position of the square is in 3d space, or the first coordinate will move
            //as we move the camera which is not what we want
            Vector3 squareStartScreen = Camera.main.WorldToScreenPoint(squareStartPos);

           
            Vector3 squareEndScreen = Camera.main.WorldToScreenPoint(squareEndPos);
            squareStartScreen.z = 0f;
            squareEndScreen.z = 0;
            //Get the middle position of the square
            Vector3 middle = (squareStartScreen + squareEndScreen) / 2f;

            //Set the middle position of the GUI square
            selectionSquareTrans.position = middle;

            //Change the size of the square
            float sizeX = Mathf.Abs(squareStartScreen.x - squareEndScreen.x);
            float sizeY = Mathf.Abs(squareStartScreen.y - squareEndScreen.y);

            //Set the size of the square
            selectionSquareTrans.sizeDelta = new Vector2(sizeX, sizeY);

            //The problem is that the corners in the 2d square is not the same as in 3d space
            //To get corners, we have to fire a ray from the screen
            //We have 2 of the corner positions, but we don't know which,  
            //so we can figure it out or fire 4 raycasts
            TL = Camera.main.ScreenToWorldPoint(new Vector3(middle.x - sizeX / 2f, middle.y + sizeY / 2f, 0f));
            TR = Camera.main.ScreenToWorldPoint(new Vector3(middle.x + sizeX / 2f, middle.y + sizeY / 2f, 0f));
            BL = Camera.main.ScreenToWorldPoint(new Vector3(middle.x - sizeX / 2f, middle.y - sizeY / 2f, 0f));
            BR = Camera.main.ScreenToWorldPoint(new Vector3(middle.x + sizeX / 2f, middle.y - sizeY / 2f, 0f));

            if (showRealPosOfRectangle)
            {
                sphere1.position = (Vector2)TL;
                sphere2.position = (Vector2)TR;
                sphere3.position = (Vector2)BL;
                sphere4.position = (Vector2)BR;
            }
        


        }
    }
