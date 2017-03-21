using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.models
{
    public class Interaction
    {
        private static Interaction instance;
        public static Interaction Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Interaction();
                }
                return instance;
            }
        }
        

        public int InteractionId { get; set; }
        [NotMapped]
        public List<Relation> Relations { get; set; }
        public List<IntervalGroup> IntervalGroups { get; set; }
        public List<RelationInterval> RelationIntervals { get; set; }
        
        private Interaction() {
            Relations = new List<Relation>();
            IntervalGroups = new List<IntervalGroup>();
            RelationIntervals = new List<RelationInterval>();
        }

        private IntervalGroup GenerateIntervalGroup(Person p1, Person p2)
        {

            IntervalGroup intervalGroup = new IntervalGroup(p1, p2);

            List<Relation> relations = Interaction.Instance.Relations;
            TimeSpan startTime = TimeSpan.FromSeconds(0); // = null;
            TimeSpan endTime; // = null;
            TimeSpan nextTime;
            TimeSpan currentTime;
            //int totalInteraction = 1;

            //RelationInterval relation; // = new RelationInterval();
            double thresholdToClose = 3000; // 3 segundos

            for (int i = 0; i < relations.Count(); i++)
            {
                if (
                    (relations[i].Person1 == p1 && relations[i].Person2 == p2) ||
                    (relations[i].Person1 == p2 && relations[i].Person2 == p1)
                    )
                {
                    currentTime = relations[i].LocationTime;
                    if (i == 0)
                    {
                        startTime = currentTime;
                    }

                    if (i + 1 < relations.Count()) // existe siguiente?
                    {
                        nextTime = relations[i + 1].LocationTime;
                        if (nextTime.Subtract(currentTime) >= TimeSpan.FromMilliseconds(thresholdToClose))
                        {
                            endTime = currentTime;
                            RelationInterval relationInterval = new RelationInterval(p1, p2, startTime, endTime);
                            intervalGroup.RelationsIntervals.Add(relationInterval);
                            RelationIntervals.Add(relationInterval);
                            //totalInteraction++;
                            startTime = nextTime;
                        }
                    }
                    else // no existe siguiente
                    {
                        endTime = currentTime;
                        RelationInterval relationInterval = new RelationInterval(p1, p2, startTime, endTime);
                        intervalGroup.RelationsIntervals.Add(relationInterval);
                        RelationIntervals.Add(relationInterval);

                        //totalInteraction++;
                        break;
                    }
                }

            }
            return intervalGroup;

        }

        public void GenerateIntervalGroups()
        {
            List<Person> persons = Scene.Instance.Persons;
            for (int i = 0; i < Scene.Instance.Persons.Count(); i++)
                for (int j = i + 1; j < Scene.Instance.Persons.Count(); j++)
                    IntervalGroups.Add( GenerateIntervalGroup(persons[i], persons[j]) );
        }

    }
}
