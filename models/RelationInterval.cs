using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Samples.Kinect.VisualizadorMultimodal.utils;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.models
{
    public class RelationInterval
    {
        public int RelationIntervalId { get; set; }
        public TimeSpan Duration { get; set; }
        public Person Person1 { get; set; }
        public Person Person2 { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int TotalInteraction { get; set; }
        [NotMapped]
        public string StartTimeFormatted {
            get
            {
                return StartTime.ToShortForm();
            }
        }

        [NotMapped]
        public string EndTimeFormatted
        {
            get
            {
                return EndTime.ToShortForm();
            }
        }

        [NotMapped]
        public string DurationFormatted {
            get
            {
                return Duration.ToShortForm();
            }
        }

        //public TimeSpan PromInteraction { get; set; } 
        //public int TotalInteraction { get; set; }
        //public int MinInteraction { get; set; }
        //public int MaxInteraction { get; set; }
        
        public RelationInterval()
        {

        }

        public RelationInterval(Person p1, Person p2, TimeSpan st, TimeSpan et)
        {
            Person1 = p1;
            Person2 = p2;
            StartTime = st;
            EndTime = et;
            Duration = et.Subtract(st);
            //TotalInteraction = totInt;
        }

        
    }

}
