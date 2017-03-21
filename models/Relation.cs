using KinectEx;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.models
{
    public class Relation
    {
        public int RelationId { get; set; }
        public double Distance { get; set; }
        public Person Person1 { get; set; }
        public Person Person2 { get; set; }
        public TimeSpan LocationTime { get; set; }

        [NotMapped]
        private static double distanceThreshold = 120; // cm

        
        public Relation()
        {

        }
        public Relation(Person person1, Person person2, IJoint center1, IJoint center2, TimeSpan LocationTime)
        {
            Person1 = person1;
            Person2 = person2;
            this.Distance = distance(center1, center2);
            this.LocationTime = LocationTime;
        }

        public bool HasInteraction()
        {
            return Distance < distanceThreshold;
        }

        public static double DistanceThreshold
        {
            get
            {
                return distanceThreshold;
            }
            set
            {
                distanceThreshold = value;
            }
        }

        private double distance(IJoint center1, IJoint center2)
        {
            float Xi = center1.Position.X;
            float Yi = center1.Position.Y;
            float Zi = center1.Position.Z;

            float Xj = center2.Position.X;
            float Yj = center2.Position.Y;
            float Zj = center2.Position.Z;

            double potencia = Math.Pow(Xj - Xi, 2) + Math.Pow(Yj - Yi, 2) + Math.Pow(Zj - Zi, 2);
            double result = Math.Sqrt(potencia);
            return result * 100;
        }
    }
}
