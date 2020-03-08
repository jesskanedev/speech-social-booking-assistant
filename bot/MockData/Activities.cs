using System;
using System.Collections.Generic;
using System.Linq;
using CoreBot.MockData.Models;

namespace CoreBot.MockData
{
    public static class Activities
    {
        public const string AquariumActivityId = "Aquarium";
        public const string DinnerActivityId = "Dinner";
        public const string BusTourActivityId = "BusTour";
        public const string ChoirShowActivityId = "Choir";

        public static IEnumerable<Activity> PortalActivities = new List<Activity>
        {
            new Activity
            {
                Id = AquariumActivityId,
                Name = "Day trip to the aquarium",
                Location = "Kelly Tarlton's, 23 Tamaki Drive, Auckland",
                Details = "We will meet at the front desk at 9am to board the bus, ready to leave by 9.30. Lunch will be supplied.",
                Organiser = "Tammy McNamara",
                Date = DateTime.Now.AddDays(3),
                Price = 70.00M
            },
            new Activity
            {
                Id = DinnerActivityId,
                Name = "Dinner event at the restaurant",
                Location = "Restaurant",
                Details = "We will be holding a special dinner event in the restaurant. Come dressed in your finest 20's attire!",
                Organiser = "Jules Anderson",
                Date = DateTime.Now.AddDays(12),
                Price = 34.99M
            },
            new Activity
            {
                Id = BusTourActivityId,
                Name = "Auckland Explorer bus tour",
                Location = "Throughout Auckland City",
                Details = "Let's get out and about on a special bus tour around Auckland city. We will be stopping at Cornwall Park for a picnic lunch. Please meet at the front desk at 8am sharp to board the bus.",
                Organiser = "Jules Anderson",
                Date = DateTime.Now.AddDays(1),
                Price = 0.00M
            },
            new Activity
            {
                Id = ChoirShowActivityId,
                Name = "Choir Club Annual Charity Performance",
                Location = "Restaurant",
                Details = "The Choir Club will be holding their annual charity fundraiser event in the restaurant after dinner. Free entry, but please feel free to bring a donation!",
                Organiser = "Henry Greene",
                Date = DateTime.Now,
                Price = 0.00M
            }
        };

        public static IEnumerable<string> GetActivityNamesByTimeEntityFilter(string timeEntity)
        {
            List<string> activityNames = new List<string>();

            // Today
            if (timeEntity.Contains("today", StringComparison.InvariantCultureIgnoreCase))
            {
                var filteredActivities = PortalActivities.Where(a => a.Date.Date == DateTime.Today);
                activityNames.AddRange(filteredActivities.Select(a => a.Name));
            }

            // Tomorrow
            if (timeEntity.Contains("tomorrow", StringComparison.InvariantCultureIgnoreCase))
            {
                var filteredActivities = PortalActivities.Where(a => a.Date.Date == DateTime.Today.AddDays(1));
                activityNames.AddRange(filteredActivities.Select(a => a.Name));
            }

            // Next few days
            if (timeEntity.Contains("next few days", StringComparison.InvariantCultureIgnoreCase) ||
                timeEntity.Contains("next couple of days", StringComparison.InvariantCultureIgnoreCase) ||
                timeEntity.Contains("next couple days", StringComparison.InvariantCultureIgnoreCase))
            {
                var filteredActivities = PortalActivities.Where(a => a.Date.Date < DateTime.Today.AddDays(5));
                activityNames.AddRange(filteredActivities.Select(a => a.Name));
            }

            // This week
            if (timeEntity.Contains("this week", StringComparison.InvariantCultureIgnoreCase) ||
                timeEntity.Contains("this coming week", StringComparison.InvariantCultureIgnoreCase) ||
                timeEntity.Contains("coming week", StringComparison.InvariantCultureIgnoreCase) ||
                timeEntity.Contains("the next week", StringComparison.InvariantCultureIgnoreCase) ||
                timeEntity.Contains("upcoming week", StringComparison.InvariantCultureIgnoreCase) ||
                timeEntity.Contains("following week", StringComparison.InvariantCultureIgnoreCase) ||
                timeEntity.Contains("week coming up", StringComparison.InvariantCultureIgnoreCase))
            {
                var filteredActivities = PortalActivities.Where(a => a.Date.Date < DateTime.Today.AddDays(7));
                activityNames.AddRange(filteredActivities.Select(a => a.Name));
            }

            // Next few weeks
            if (timeEntity.Contains("next few weeks", StringComparison.InvariantCultureIgnoreCase) ||
                timeEntity.Contains("next couple of weeks", StringComparison.InvariantCultureIgnoreCase) ||
                timeEntity.Contains("next couple weeks", StringComparison.InvariantCultureIgnoreCase) ||
                timeEntity.Contains("fortnight", StringComparison.InvariantCultureIgnoreCase))
            {
                var filteredActivities = PortalActivities.Where(a => a.Date.Date < DateTime.Today.AddDays(21));
                activityNames.AddRange(filteredActivities.Select(a => a.Name));
            }

            // This month
            if (timeEntity.Contains("this month", StringComparison.InvariantCultureIgnoreCase) ||
                timeEntity.Contains("this coming month", StringComparison.InvariantCultureIgnoreCase) ||
                timeEntity.Contains("coming month", StringComparison.InvariantCultureIgnoreCase) ||
                timeEntity.Contains("the next month", StringComparison.InvariantCultureIgnoreCase) ||
                timeEntity.Contains("upcoming month", StringComparison.InvariantCultureIgnoreCase) ||
                timeEntity.Contains("following month", StringComparison.InvariantCultureIgnoreCase) ||
                timeEntity.Contains("month coming up", StringComparison.InvariantCultureIgnoreCase))
            {
                var filteredActivities = PortalActivities.Where(a => a.Date.Date < DateTime.Today.AddDays(31));
                activityNames.AddRange(filteredActivities.Select(a => a.Name));
            }

            return activityNames;
        }

        public static string BuildActivitiesString(List<string> activityNames)
        {
            var activitiesEnquiryText = activityNames.Count == 1 ? "We have" : "There are some interesting events coming up! We have";

            for (int i = 0; i < activityNames.Count; i++)
            {
                var activityName = activityNames[i];

                if (i == 0 && activityNames.Count == 1)
                {
                    // only item
                    activitiesEnquiryText += $" the {activityName}.";
                }

//                else if (i == 0)
//                {
//                    // first item
//                    activitiesEnquiryText += $" the {activityName}, ";
//                }

                else if ((i + 1) == activityNames.Count)
                {
                    // last item
                    activitiesEnquiryText += $" and the {activityName}.";
                }

                else
                {
                    // every other item
                    activitiesEnquiryText += $" the {activityName},";
                }
            }

            activitiesEnquiryText += activityNames.Count == 1 ? " Let me know if you would like to be booked on this." : " Let me know if you would like to be booked on any of these.";
            return activitiesEnquiryText;
        }
    }
}
