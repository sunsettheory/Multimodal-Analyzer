using KinectEx;
using KinectEx.DVR;
using KinectEx.Smoothing;
using Microsoft.Kinect;
using Microsoft.Samples.Kinect.VisualizadorMultimodal.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.core
{
    public class Monitor
    {
        KinectSensor _sensor = null;
        BodyFrameReader _bodyReader = null;
        ColorFrameReader _colorReader = null;
        
        ColorFrameBitmap _colorBitmap;
        List<CustomBody> _bodies = null;

        bool _bodyFrameEnable = true;
        bool _colorFrameEnable = true;

        public Monitor()
        {
            Open();
        }

        //public void Enable()
        //{
        //    _bodyFrameEnable = true;
        //    _colorFrameEnable = true;
        //}

        //public void Disable()
        //{
        //    _bodyFrameEnable = false;
        //    _colorFrameEnable = false;
        //}

        public bool IsOpen
        {
            get
            {
                return _colorReader != null;
            }
        }

        public void Open()
        {
            _colorBitmap = new ColorFrameBitmap();
            //DepthFrameBitmap _depthBitmap = new DepthFrameBitmap();
            //InfraredFrameBitmap _infraredBitmap = new InfraredFrameBitmap();

            this._sensor = Kinect.Sensor;
            _bodies = new List<CustomBody>();

            _bodyReader = _sensor.BodyFrameSource.OpenReader();
            
            _bodyReader.FrameArrived += _bodyReader_FrameArrived;

            _colorReader = _sensor.ColorFrameSource.OpenReader();
            _colorReader.FrameArrived += _colorReader_FrameArrived;

            MainWindow.Instance().colorImageControl.Source = _colorBitmap.Bitmap;
        }

        public void Close()
        {
            _bodies = new List<CustomBody>();

            _bodyReader.FrameArrived -= _bodyReader_FrameArrived;
            _colorReader.FrameArrived -= _colorReader_FrameArrived;

            _bodyReader.Dispose();
            _colorReader.Dispose();

            _bodyReader = null;
            _colorReader = null;

            MainWindow.Instance().colorImageControl.Source = null;
            MainWindow.Instance().bodyImageControl.Source = null;

            _colorBitmap = null;

            this._sensor.Close();
        }


        private void _bodyReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            if (!_bodyFrameEnable) return;
            IEnumerable<IBody> bodies = null; // to make the GetBitmap call a little cleaner
            using (var frame = e.FrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    frame.GetAndRefreshBodyData(_bodies);
                    bodies = _bodies;
                }
            }

            if (bodies != null)
            {
                
                int i = 0;
                foreach (IBody body1 in bodies)
                {
                    if (Scene.Instance != null && Kinect.Instance.Recorder.IsRecording)
                    {
                        if (body1.TrackingId != 0)
                        {
                            Person person1 = Scene.Instance.Persons.FirstOrDefault(p => p.TrackingId == (long)body1.TrackingId);
                            if (person1 == null)
                            {
                                person1 = new Person(
                                        body1.TrackingId,
                                        Scene.Instance.Persons.Count
                                    );
                                Scene.Instance.Persons.Add(person1);
                            }
                            
                            IJoint center = body1.Joints[JointType.SpineShoulder];
                            for (int j = i + 1; j < bodies.Count(); j++)
                            {
                                IBody body2 = bodies.ElementAt(j);

                                if (body2.TrackingId != 0)
                                {
                                    Person person2 = Scene.Instance.Persons.FirstOrDefault(p => p.TrackingId == (long)body2.TrackingId);
                                    if (person2 == null)
                                    {
                                        person2 = new Person(
                                                body2.TrackingId,
                                                Scene.Instance.Persons.Count
                                            );
                                        Scene.Instance.Persons.Add(person2);
                                    }
                                    IJoint center2 = body2.Joints[JointType.SpineShoulder];
                                    //double d = Relacion.distance();
                                    Relation rel = new Relation(person1, person2, center, center2, Kinect.Instance.Recorder.getCurrentLocation());
                                    if( rel.HasInteraction() ) Interaction.Instance.Relations.Add(rel);
                                }
   
                            }
                        }

                        
                        //if the current body TrackingId changed, update the corresponding gesture detector with the new value
                        if (body1.TrackingId != Kinect.Instance.gestureDetectorList[i].TrackingId)
                        {
                            Kinect.Instance.gestureDetectorList[i].TrackingId = body1.TrackingId;

                            // if the current body is tracked, unpause its detector to get VisualGestureBuilderFrameArrived events
                            // if the current body is not tracked, pause its detector so we don't waste resources trying to get invalid gesture results
                            Kinect.Instance.gestureDetectorList[i].IsPaused = body1.TrackingId == 0;
                        }


                    }
                    i++;
                }

                bodies.MapDepthPositions();
                //bodies.MapColorPositions();
                MainWindow.Instance().bodyImageControl.Source = bodies.GetBitmap(Colors.LightGreen, Colors.Yellow);
                //OutputImage = bodies.GetBitmap(Colors.LightGreen, Colors.Yellow);
            }
            else
            {
                MainWindow.Instance().bodyImageControl.Source = null;
                //OutputImage = null;
            }
        }

        private void _colorReader_FrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
            if (!_colorFrameEnable) return;

            _colorBitmap.Update(e.FrameReference);
            MainWindow.Instance().colorImageControl.Source = _colorBitmap.Bitmap;
        }

    }
}
