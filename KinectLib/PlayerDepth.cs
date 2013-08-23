using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace GintySoft.KinectLib
{
    public struct PlayerDepth
    {
        public bool PlayerTracked;
        public int Depth;
        public int Player;

        public PlayerDepth(short val)
        {
            Player = val & DepthImageFrame.PlayerIndexBitmask;
            Depth = val >> DepthImageFrame.PlayerIndexBitmaskWidth;
            if (Player == 0)//0 means no player is being tracked, 1 or 2 means it is
            {
                PlayerTracked = false;
            }
            else
            {
                PlayerTracked = true;
            }
        }
    }
}
