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
