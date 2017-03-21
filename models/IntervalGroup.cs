using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.models
{
    public class IntervalGroup
    {

        public int IntervalGroupId { get; set; }
        public TimeSpan PromedioInteraction { get; set; }
        public Person Person1 { get; set; }
        public Person Person2 { get; set; }
        public List<RelationInterval> RelationsIntervals { get; set; }

        [NotMapped]
        public int TotalInteraction
        {
            get
            {
                return RelationsIntervals.Count();
            }
        }
        [NotMapped]
        private TimeSpan totalDuration = TimeSpan.FromSeconds(0);

        public IntervalGroup()
        {

        }

        public IntervalGroup(Person p1, Person p2)
        {
            PromedioInteraction = TimeSpan.FromSeconds(0);
            Person1 = p1;
            Person2 = p2;
            RelationsIntervals = new List<RelationInterval>();
        }
        
        public TimeSpan TotalDuration
        {
            get
            {
                if (totalDuration == TimeSpan.FromSeconds(0) )
                {
                    totalDuration = calculateTotalDuration();
                }
                return totalDuration;
            }
        }

        private TimeSpan calculateTotalDuration()
        {
            TimeSpan sum = TimeSpan.FromSeconds(0);
            foreach (var relationInterval in RelationsIntervals)
            {
                sum += relationInterval.Duration;
            }

            return sum;
        }
        
        //public void addRelationInterval(RelationInterval relationInterval)
        //{
        //    RelationsIntervals.Add(relationInterval);
        //}
    }
}
