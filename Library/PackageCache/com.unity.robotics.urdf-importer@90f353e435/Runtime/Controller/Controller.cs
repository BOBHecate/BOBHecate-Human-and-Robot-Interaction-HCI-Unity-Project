using System;
using Unity.Robotics;
using UnityEngine;

// public class PlayerHealth : MonoBehaviour 
// { 
//     [SerializeField] AudioSource playerAudioSource;
//     float hp = 100;
//     public void PlayerHurt()
//     {
//         hp -= 20;
//     }
// }


namespace Unity.Robotics.UrdfImporter.Control
{
    public enum RotationDirection { None = 0, Positive = 1, Negative = -1 };
    public enum ControlType { PositionControl };
    

    // public class slider : using UnityEngine;
    
    // {
    //      public float SilderUI (float value){
    //         return value;
    //     }
        
    // }
   

   


    public class Controller : MonoBehaviour
    {
        private ArticulationBody[] articulationChain;
        // Stores original colors of the part being highlighted
        private Color[] prevColor;
        private int previousIndex;

        private FKRobot Fk;


        [InspectorReadOnly(hideInEditMode: true)]
        public string selectedJoint;
        // [HideInInspector]
        public int selectedIndex;

        public ControlType control = ControlType.PositionControl;

        public float stiffness = 10000f;
        public float damping = 100f;
        public float forceLimit = 1000f;

        public float slider = 0.00f;

        public bool rpress;

        public bool lpress;
        public float speed = 30f; // Units: degree/s
        public float torque = 100f; // Units: Nm or N
        public float acceleration = 10f;// Units: m/s^2 / degree/s^2

        public bool times = true; //flag for the times press the bottons

        public float JointAngle = 0.0f;

        public float Angle1;
        public float Angle2;
        public float Angle3;

        public float JointAngle3;

        public float JointAngle2;

        public float JointAngle1;

        public int Jointint;

        public Transform ObjectTar;

        public Transform Objectbase;

        public Transform endeffector;

        public Vector3 relativePosition;

        public Vector3 endeffector_coor;

        // public double endeffector_y;


        
        // public Transform articulationChain[4];

        

        [Tooltip("Color to highlight the currently selected join")]
        public Color highLightColor = new Color(1.0f, 0, 0, 1.0f);



        public void H (float value)
        {
            slider = (float)Math.Round(value,2);
            //speed = Math.Abs(slider)*60f;
            return;
        }

        public void press()
        {
            
            slider = 0;
            return;
        }

        public void pressright()
        {
           
                rpress = true;
                times = true;
            
            return ;
        }

        public void unpressright()
        {

            rpress = false;
            times = true;
            return ;
        }

        public void pressleft()
        {
         
                lpress = true;
                times = true;
                return ;
        }

        public void unpressleft()
        {
            lpress = false;
            times = true;
            return ;
        }

        void Start()
        {
            
            previousIndex = selectedIndex = 1;
            this.gameObject.AddComponent<FKRobot>();
            articulationChain = this.GetComponentsInChildren<ArticulationBody>(); // The code get all the articulationBody
            int defDyanmicVal = 10;
            foreach (ArticulationBody joint in articulationChain)
            {
                joint.gameObject.AddComponent<JointControl>();
                joint.jointFriction = defDyanmicVal;
                joint.angularDamping = defDyanmicVal;
                ArticulationDrive currentDrive = joint.xDrive;
                currentDrive.forceLimit = forceLimit;
                joint.xDrive = currentDrive;
               
            }
            DisplaySelectedJoint(selectedIndex);
            StoreJointColors(selectedIndex);
        }

        void SetSelectedJointIndex(int index)
        {
            if (articulationChain.Length > 0) 
            {
                selectedIndex = (index + articulationChain.Length) % articulationChain.Length;
            }
        }

        void InK(double x,double y,double z)
        {
            double L1 = 0.24;
            double L2 = 0.24;
            double Angle11;
            double Angle12;
            double AngleT = Math.Acos(Math.Sqrt(Math.Pow(x, 2)+Math.Pow(z, 2))/Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2)));
            double NX = Math.Sqrt(Math.Pow(x,2)+Math.Pow(z,2));

            Angle2 = (float)((180 / Math.PI)*Math.Acos(((Math.Pow(NX,2))+Math.Pow(y,2)-Math.Pow(L1,2)-Math.Pow(L2,2))/(-2*L1*L2)));

            Angle11 = (float)((180 / Math.PI)*Math.Acos((Math.Pow(L2,2)-Math.Pow(L1,2)-Math.Pow(NX,2)-Math.Pow(y,2))/(-2*Math.Sqrt(Math.Pow(NX,2)+Math.Pow(y,2))*L1)));
            
            Angle12 = (float)((180 / Math.PI)*Math.Atan(y/NX));
            
            Angle1 = (float)(Angle11 + Angle12);
        // // 计算表达式中的各个部分
        //     double numerator1 = Math.Pow(L1, 2) + Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2) - Math.Pow(L2, 2);
        //     double denominator1 = 2 * L1 * Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));

        // // 计算最终结果
        //     Angle1 = (float)((180 / Math.PI) *Math.Acos(numerator1 / denominator1)+AngleT);

        //     double numerator2 = Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2) - Math.Pow(L2, 2)- Math.Pow(L1, 2);
        //     double denominator2 = 2 * L1 * L2;

        // // 计算最终结果
        //     Angle2 = (float)((180 / Math.PI) *Math.Acos(numerator2 / denominator2));
            Angle3 = (float)((180 / Math.PI) *Math.Atan(z/x));
        }
        //原来的code
        // void Update()
        // {
        //     bool SelectionInput1 = Input.GetKeyDown("right");
        //     bool SelectionInput2 = Input.GetKeyDown("left");

        //     SetSelectedJointIndex(selectedIndex); // to make sure it is in the valid range
        //     UpdateDirection(selectedIndex);

        //     if (SelectionInput2)
        //     {
        //         SetSelectedJointIndex(selectedIndex - 1);
        //         Highlight(selectedIndex);
        //     }
        //     else if (SelectionInput1)
        //     {
        //         SetSelectedJointIndex(selectedIndex + 1);
        //         Highlight(selectedIndex);
        //     }

        //     UpdateDirection(selectedIndex);
        // }

        //    void Update()
        // {
        //     //bool SelectionInput1 = Input.GetKeyDown("right");
        //     bool SelectionInput1 = rpress;
        //     //bool SelectionInput2 = Input.GetKeyDown("left");
        //     bool SelectionInput2 = lpress;
        //     JointAngle = (float)Math.Round(articulationChain[selectedIndex].jointPosition[0],2);
        //     SetSelectedJointIndex(selectedIndex); // to make sure it is in the valid range
        //     UpdateDirection(selectedIndex);
        //     if (times){
        //         if (SelectionInput2)
        //         {
        //             SetSelectedJointIndex(selectedIndex - 1);
        //             Highlight(selectedIndex);
        //         }
        //         else if (SelectionInput1)
        //         {
        //             SetSelectedJointIndex(selectedIndex + 1);
        //             Highlight(selectedIndex);
        //         }
        //         times = false;
        //         }
            
        //     // Highlight(selectedIndex);
        //     UpdateDirection(selectedIndex);
            
        // } 
        //Button control the jointindex and slider control the JointAngle

       void Update()
        {
             relativePosition = Objectbase.InverseTransformPoint(ObjectTar.position);
            // relativePosition = new Vector3(-0.163f,-0.148f,0.19f);
            endeffector_coor = Objectbase.InverseTransformPoint(endeffector.position);
            // endeffector_y = endeffector_coor.y;
            double x = relativePosition.x;
            double y = relativePosition.y;
            double z = relativePosition.z;
            float distanceToTarget = relativePosition.magnitude;
            // if (distanceToTarget > 0.24 + 0.2452)
            // {
            //     Debug.LogWarning("目标位置超出机械臂的工作范围！");
            //     return;
            // }
            InK(x,z,y);
            JointAngle3 = (float)(-Angle3);
            JointAngle = (float)Math.Round(articulationChain[1].jointPosition[0],2);
            UpdateDirection1();

            Angle1 = -(90 - Angle1);
            JointAngle1 = (float)(Angle1/57.37771);
            JointAngle = (float)Math.Round(articulationChain[2].jointPosition[0],2);
            UpdateDirection2();
            
            Angle2 = -(90 - Angle2);
            JointAngle2 = (float)(Angle2/57.37771);
            JointAngle = (float)Math.Round(articulationChain[3].jointPosition[0],2);
            UpdateDirection3();
         
        }
        

        /// <summary>
        /// Highlights the color of the robot by changing the color of the part to a color set by the user in the inspector window
        /// </summary>
        /// <param name="selectedIndex">Index of the link selected in the Articulation Chain</param>
        private void Highlight(int selectedIndex)
        {
            if (selectedIndex == previousIndex || selectedIndex < 0 || selectedIndex >= articulationChain.Length) 
            {
                return;
            }

            // reset colors for the previously selected joint
            ResetJointColors(previousIndex);

            // store colors for the current selected joint
            StoreJointColors(selectedIndex);

            DisplaySelectedJoint(selectedIndex);
            Renderer[] rendererList = articulationChain[selectedIndex].transform.GetChild(0).GetComponentsInChildren<Renderer>();

            // set the color of the selected join meshes to the highlight color
            foreach (var mesh in rendererList)
            {
                MaterialExtensions.SetMaterialColor(mesh.material, highLightColor);
            }
        }

        void DisplaySelectedJoint(int selectedIndex)
        {
            if (selectedIndex < 0 || selectedIndex >= articulationChain.Length) 
            {
                return;
            }
            selectedJoint = articulationChain[selectedIndex].name + " (" + selectedIndex + ")";
        }

    

         

        /// <summary>
        /// Sets the direction of movement of the joint on every update
        /// </summary>
        // /// <param name="jointIndex">Index of the link selected in the Articulation Chain</param>
        // private void UpdateDirection(int jointIndex)
        // {
        //     if (jointIndex < 0 || jointIndex >= articulationChain.Length) //ArticulationChain.Length is the number of joints
        //     {
        //         return;
        //     }

        //     //float moveDirection = Input.GetAxis("Vertical"); /原code
        //     float moveDirection = slider; //slider control the joint
            
        //     //Current joint control
        //     JointControl current = articulationChain[jointIndex].GetComponent<JointControl>();
            
        //     //Stop the rotation of the previous joint and reset the previous joint index
        //     if (previousIndex != jointIndex)
        //     {
        //         JointControl previous = articulationChain[previousIndex].GetComponent<JointControl>();
        //         previous.direction = RotationDirection.None;
        //         previousIndex = jointIndex;
        //     } 

        //     if (current.controltype != control) 
        //     {
        //         UpdateControlType(current);
        //     }

        //     if (moveDirection > JointAngle)
        //     {
        //         current.direction = RotationDirection.Positive;
                
        //     }
        //     else if (moveDirection < JointAngle)
        //     {
        //         current.direction = RotationDirection.Negative;
        //         // current.direction = -1;
        //     }
        //     else
        //     {
        //         current.direction = RotationDirection.None;
              
        //         // current.direction = 0;
        //     }
        // }

        //Each Joint Controlling 
        private void UpdateDirection1()
        {
            
            JointControl current = articulationChain[1].GetComponent<JointControl>();

            //float moveDirection = Input.GetAxis("Vertical"); /原code
            float moveDirection = JointAngle3; //slider control the joint
            // float moveDirection = 0; //slider control the joint
            if (moveDirection > JointAngle)
            {
                current.direction = RotationDirection.Positive;
                
            }
            else if (moveDirection < JointAngle)
            {
                current.direction = RotationDirection.Negative;
                // current.direction = -1;
            }
            else
            {
                current.direction = RotationDirection.None;
              
                // current.direction = 0;
            }
        }

         private void UpdateDirection2()
        {
            
            JointControl current = articulationChain[2].GetComponent<JointControl>();

            //float moveDirection = Input.GetAxis("Vertical"); /原code
            float moveDirection = JointAngle1; //slider control the joint
           
            if (moveDirection > JointAngle)
            {
                current.direction = RotationDirection.Positive;
                
            }
            else if (moveDirection < JointAngle)
            {
                current.direction = RotationDirection.Negative;
                // current.direction = -1;
            }
            else
            {
                current.direction = RotationDirection.None;
              
                // current.direction = 0;
            }
        }

         private void UpdateDirection3()
        {
            
            JointControl current = articulationChain[3].GetComponent<JointControl>();

            //float moveDirection = Input.GetAxis("Vertical"); /原code
            float moveDirection = JointAngle2; //slider control the joint
            // float moveDirection = 0; //slider control the joint
            if (moveDirection > JointAngle)
            {
                current.direction = RotationDirection.Positive;
                
            }
            else if (moveDirection < JointAngle)
            {
                current.direction = RotationDirection.Negative;
                // current.direction = -1;
            }
            else
            {
                current.direction = RotationDirection.None;
              
                // current.direction = 0;
            }
        }

        /// <summary>
        /// Stores original color of the part being highlighted
        /// </summary>
        /// <param name="index">Index of the part in the Articulation chain</param>
        private void StoreJointColors(int index)
        {
            Renderer[] materialLists = articulationChain[index].transform.GetChild(0).GetComponentsInChildren<Renderer>();
            prevColor = new Color[materialLists.Length];
            for (int counter = 0; counter < materialLists.Length; counter++)
            {
                prevColor[counter] = MaterialExtensions.GetMaterialColor(materialLists[counter]);
            }
        }

        /// <summary>
        /// Resets original color of the part being highlighted
        /// </summary>
        /// <param name="index">Index of the part in the Articulation chain</param>
        private void ResetJointColors(int index)
        {
            Renderer[] previousRendererList = articulationChain[index].transform.GetChild(0).GetComponentsInChildren<Renderer>();
            for (int counter = 0; counter < previousRendererList.Length; counter++)
            {
                MaterialExtensions.SetMaterialColor(previousRendererList[counter].material, prevColor[counter]);
            }
        }

        public void UpdateControlType(JointControl joint)
        {
            joint.controltype = control;
            if (control == ControlType.PositionControl)
            {
                ArticulationDrive drive = joint.joint.xDrive;
                drive.stiffness = stiffness;
                drive.damping = damping;
                joint.joint.xDrive = drive;
            }
        }
       
        public void OnGUI()
    {
        GUIStyle centeredStyle = GUI.skin.GetStyle("Label");
        centeredStyle.alignment = TextAnchor.UpperCenter;
        centeredStyle.fontSize = 40; // 设置字体大小为20

        GUI.Label(new Rect(Screen.width / 2 - 200, 10, 400, 20), "Press left/right arrow keys to select a robot joint.");
        GUI.Label(new Rect(Screen.width / 2 - 200, 30, 400, 20), "Press up/down arrow keys to move " + selectedJoint + ".", centeredStyle);
    }

        
    }
}
