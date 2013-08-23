using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;
using GintySoft.KinectLib;
using Coding4Fun.Kinect.Wpf;

namespace GintySoft.Kinect
{
    public class SkeletalViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        private Canvas m_canvas;
        private KinectLib.Kinect m_kinect;

        public Canvas Canvas
        {
            get
            {
                return m_canvas;
            }
            set
            {
                m_canvas = value;
                
                OnPropertyChanged("Canvas");
            }
        }

        public SkeletalViewModel(KinectLib.Kinect kinect)
        {
            m_kinect = kinect;

            m_kinect.NewSkeletonFrame += new KinectLib.Kinect.SkeletonFrameReadyDelegate(m_kinect_NewSkeletonFrame);
        }

        private void m_kinect_NewSkeletonFrame(Skel skel)
        {
            //KinectSDK TODO: this shouldn't be needed, but if power is removed from the Kinect, you may still get an event here, but skeletonFrame will be null.
            if (skel == null)
            {
                return;
            }

            int iSkeleton = 0;
            Brush[] brushes = new Brush[6];
            brushes[0] = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            brushes[1] = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            brushes[2] = new SolidColorBrush(Color.FromRgb(64, 255, 255));
            brushes[3] = new SolidColorBrush(Color.FromRgb(255, 255, 64));
            brushes[4] = new SolidColorBrush(Color.FromRgb(255, 64, 255));
            brushes[5] = new SolidColorBrush(Color.FromRgb(128, 128, 255));

            Canvas skeletonCanvas = new Canvas();

            skeletonCanvas.Children.Clear();
            skeletonCanvas.Height = 300;
            skeletonCanvas.Width = 400;
            skeletonCanvas.Background = new SolidColorBrush(Color.FromRgb(138, 168, 255));

        
            foreach (Skeleton data in skel.SkeletonData)
            {
                if (SkeletonTrackingState.Tracked == data.TrackingState)
                {
                    // Draw bones
                    Brush brush = brushes[iSkeleton % brushes.Length];
                    skeletonCanvas.Children.Add(getBodySegment(skel,data, brush, skeletonCanvas, JointType.HipCenter, JointType.Spine, JointType.ShoulderCenter, JointType.Head));
                    skeletonCanvas.Children.Add(getBodySegment(skel,data, brush, skeletonCanvas, JointType.ShoulderCenter, JointType.ShoulderLeft, JointType.ElbowLeft, JointType.WristLeft, JointType.HandLeft));
                    skeletonCanvas.Children.Add(getBodySegment(skel,data, brush, skeletonCanvas, JointType.ShoulderCenter, JointType.ShoulderRight, JointType.ElbowRight, JointType.WristRight, JointType.HandRight));
                    skeletonCanvas.Children.Add(getBodySegment(skel,data, brush, skeletonCanvas, JointType.HipCenter, JointType.HipLeft, JointType.KneeLeft, JointType.AnkleLeft, JointType.FootLeft));
                    skeletonCanvas.Children.Add(getBodySegment(skel,data, brush, skeletonCanvas, JointType.HipCenter, JointType.HipRight, JointType.KneeRight, JointType.AnkleRight, JointType.FootRight));

                    // Draw joints
                    foreach (Joint joint in data.Joints)
                    {
                        Point jointPos = getDisplayPosition(joint, skeletonCanvas);
                        Line jointLine = new Line();
                        jointLine.X1 = jointPos.X - 3;
                        jointLine.X2 = jointLine.X1 + 6;
                        jointLine.Y1 = jointLine.Y2 = jointPos.Y;
                        jointLine.Stroke = jointColors[joint.JointType];
                        jointLine.StrokeThickness = 6;
                        skeletonCanvas.Children.Add(jointLine);
                    }
                }
                iSkeleton++;
            } // for each skeleton

           
            this.Canvas = skeletonCanvas;
        }

        private Polyline getBodySegment(Skel skel, Skeleton data, Brush brush, Canvas canvas, params JointType[] ids)
        {
            
            PointCollection points = new PointCollection(ids.Length);
            for (int i = 0; i < ids.Length; ++i)
            { 
                Joint j = skel.findJoint(data, ids[i]);
                points.Add(getDisplayPosition(j, canvas));
            }
         
            Polyline polyline = new Polyline();
            polyline.Points = points;
            polyline.Stroke = brush;
            polyline.StrokeThickness = 5;
            return polyline;
        }

        private Point getDisplayPosition(Joint joint, Canvas skeletonCanvas)
        {
            ColorImagePoint cpc = this.m_kinect.KinectDevice.MapSkeletonPointToColor(joint.Position, m_kinect.KinectDevice.ColorStream.Format );
            double canvasScaledX = skeletonCanvas.Width * cpc.X / 640.0;
            double canvasScaledY = skeletonCanvas.Height * cpc.Y / 480.0;
            Point p = new Point(canvasScaledX, canvasScaledY);
            return p;
        }

        private static Dictionary<JointType, Brush> jointColors = new Dictionary<JointType, Brush>() { 
            {JointType.HipCenter, new SolidColorBrush(Color.FromRgb(169, 176, 155))},
            {JointType.Spine, new SolidColorBrush(Color.FromRgb(169, 176, 155))},
            {JointType.ShoulderCenter, new SolidColorBrush(Color.FromRgb(168, 230, 29))},
            {JointType.Head, new SolidColorBrush(Color.FromRgb(200, 0,   0))},
            {JointType.ShoulderLeft, new SolidColorBrush(Color.FromRgb(79,  84,  33))},
            {JointType.ElbowLeft, new SolidColorBrush(Color.FromRgb(84,  33,  42))},
            {JointType.WristLeft, new SolidColorBrush(Color.FromRgb(255, 126, 0))},
            {JointType.HandLeft, new SolidColorBrush(Color.FromRgb(215,  86, 0))},
            {JointType.ShoulderRight, new SolidColorBrush(Color.FromRgb(33,  79,  84))},
            {JointType.ElbowRight, new SolidColorBrush(Color.FromRgb(33,  33,  84))},
            {JointType.WristRight, new SolidColorBrush(Color.FromRgb(77,  109, 243))},
            {JointType.HandRight, new SolidColorBrush(Color.FromRgb(37,   69, 243))},
            {JointType.HipLeft, new SolidColorBrush(Color.FromRgb(77,  109, 243))},
            {JointType.KneeLeft, new SolidColorBrush(Color.FromRgb(69,  33,  84))},
            {JointType.AnkleLeft, new SolidColorBrush(Color.FromRgb(229, 170, 122))},
            {JointType.FootLeft, new SolidColorBrush(Color.FromRgb(255, 126, 0))},
            {JointType.HipRight, new SolidColorBrush(Color.FromRgb(181, 165, 213))},
            {JointType.KneeRight, new SolidColorBrush(Color.FromRgb(71, 222,  76))},
            {JointType.AnkleRight, new SolidColorBrush(Color.FromRgb(245, 228, 156))},
            {JointType.FootRight, new SolidColorBrush(Color.FromRgb(77,  109, 243))}
        };
    }
}
