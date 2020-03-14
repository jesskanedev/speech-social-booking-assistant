using System;

namespace CoreBot.MockData.Models
{
    /// <summary>
    /// Represents an activity.
    /// </summary>
    public class Activity
    {
        /// <summary>
        /// The activity ID.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The activity name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The activity location.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// The activity details.
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// The activity organiser.
        /// </summary>
        public string Organiser { get; set; }

        /// <summary>
        /// The activity time.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The activity price.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// The activity image URL.
        /// </summary>
        public string ImageUrl { get; set; }
    }
}
