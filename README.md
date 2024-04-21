# Unity Project of HRI
## Overview
This project aims to incorporate human-robot interaction (HRI) technologies into robotic systems by creating virtual worlds using the Unity Engine. The primary objective is to empower users to use a robotic arm using movements detected by a Leap Motion device. This method utilises the advanced hand tracking features of the Leap Motion controller to translate user hand movements into the virtual environment. It then transforms these movements to drive a robotic arm that is created in Unity. The research employs an inverse kinematics technique to translate hand motions into robot arm configurations, guaranteeing precise tracking and coordination between user gestures and robot motions. The study's findings indicate that human-robot interaction has extensive potential for improving the intuitiveness and effectiveness of interactions inside robotic systems. Subsequent efforts will concentrate on enhancing the control algorithm and expanding the system's capabilities to encompass more intricate tasks and accommodate additional interaction modalities.
## Unity install
If you are adding the URDF-Importer, ensure you are using a 2020.2.0+ version of Unity Editor.
https://unity.com/releases/editor/whats-new/2020.2.0

## Unity Robotics packages
This project is base on Unity-Robotics-Hub, please follow the instruction from https://github.com/Unity-Technologies/Unity-Robotics-Hub 
to install the URDF-Importer .

After install the URDF-Importer, niryo one robotic arm in the scene should be avaliable.

## Ultraleap Package
Import the Ultraleap Package by following the Guidance of https://docs.ultraleap.com/xr-and-tabletop/xr/unity/getting-started/index.html.

After set up the ultraleap package, headtracking in the scene should be avaliable.

## Robotic arm adjustment
Robotic arm can be adjusted through script Contoller.cs.

Alterations in the physical characteristics of robotic arm by changing this part of code:
        public float stiffness = 10000f;
        public float damping = 100f;
        public float forceLimit = 1000f;

        public float slider = 0.00f;

        public bool rpress;

        public bool lpress;
        public float speed = 30f; // Units: degree/s
        public float torque = 100f; // Units: Nm or N
        public float acceleration = 10f;// Units: m/s^2 / degree/s^2

The Update () method is an inherent function in Unity that is responsible for executing the full script, including tasks like data uploading. This feature is utilised to regulate the motion of the robotic arm.
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

The Update () function includes the Updatedirection1(), Updatedirection2(), and Updatedirection3() functions, which are utilised to rotate the joints 1, 2, and 3 of the robot arm, accordingly. Due to the similarity in structure and content, only one of these three functions is displayed.
        private void UpdateDirection1()
        {
            
            JointControl current = articulationChain[1].GetComponent<JointControl>();
            float moveDirection = JointAngle3; //slider control the joint
            if (moveDirection > JointAngle)
            {
                current.direction = RotationDirection.Positive;
                
            }
            else if (moveDirection < JointAngle)
            {
                current.direction = RotationDirection.Negative;
            }
            else
            {
                current.direction = RotationDirection.None;
            }
        }

The Ink () function is incorporated within the update() function. This function computes the angle of each joint of the robotic arm using the coordinates provided by the target hand in the virtual scene. The parameters consist of the values for the x, y, and z coordinates. After referencing it in the Update() function, the joint angle of the robotic arm will be updated directly.
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
            Angle3 = (float)((180 / Math.PI) *Math.Atan(z/x));
        }
