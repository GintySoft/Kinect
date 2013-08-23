using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace GintySoft.KinectLib
{
    public class Skel
    {
        public SkeletonFrame m_skeletonFrame = null;
        private Skeleton[] m_data = null;
        public SkeletonFrame Skeleton 
        { 
            get
            {
                return m_skeletonFrame;
            }
        }
        public Skeleton[] SkeletonData
        {
            get
            {
                if (m_data == null)
                {
                    m_data = new Skeleton[this.Skeleton.SkeletonArrayLength];
                    this.Skeleton.CopySkeletonDataTo(m_data);
                }
                return m_data;
            }
        }

        public Skel(SkeletonFrame frame)
        {
            this.m_skeletonFrame = frame;
        }

        public Joint findJoint(Skeleton skel, JointType type)
        {
            foreach (Joint j in skel.Joints)
            {
                if (type == j.JointType)
                {
                    return j;
                }
            }
            throw new Exception("Joint not found");
        }
    }
}
