using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using HomeTask1.Shared;

namespace HomeTask1.Users.WebApi;

public class Contracts
{
    public static class V1
    {
        /// <summary>
        /// Represents the model used to create a new subscription.
        /// </summary>
        public class CreateSubscription
        {
            /// <summary>
            /// Specifies the subscription type for a user or service. Valid values for this property include "Free", "Trial", and "Super".
            /// </summary>
            [Required]
            public string Type { get; set; }

            /// <summary>
            /// Specifies the start date of a subscription.
            /// </summary>

            [JsonConverter(typeof(DateTimeCustomConverter))]
            public DateTime StartDate { get; set; }

            /// <summary>
            /// Specifies the date and time when the subscription ends. The EndDate must be later than the StartDate.
            /// </summary>
            [JsonConverter(typeof(DateTimeCustomConverter))]
            public DateTime EndDate { get; set; }
        }

        /// <summary>
        /// Represents the model used to update an existing subscription.
        /// </summary>
        public class UpdateSubscription
        {
            /// <summary>
            /// Specifies the subscription type for a user or service. Valid values for this property include "Free", "Trial", and "Super".
            /// </summary>
            [Required]
            public string Type { get; set; }

            /// <summary>
            /// Specifies the start date of a subscription. This property specifies the starting date and time for the subscription.
            /// </summary>
            [JsonConverter(typeof(DateTimeCustomConverter))]
            public DateTime StartDate { get; set; }

            /// <summary>
            /// Specifies the date and time when the subscription ends. The EndDate must be later than the StartDate.
            /// </summary>
            [JsonConverter(typeof(DateTimeCustomConverter))]
            public DateTime EndDate { get; set; }
        }

        /// <summary>
        /// Represents the model used to create a new user.
        /// </summary>
        public class CreateUser
        {
            /// <summary>
            /// Specifies the name of the user being created or handled.
            /// </summary>
            [Required]
            public string Name { get; set; }

            /// <summary>
            /// Specifies the email address of a user.
            /// </summary>
            [Required]
            public string Email { get; set; }

            /// <summary>
            /// Specifies the unique identifier of a subscription assigned to a user.
            /// </summary>
            public int SubscriptionId { get; set; } 
        }

        /// <summary>
        /// Represents the model used to update an existing user.
        /// </summary>
        public class UpdateUser
        {
            /// <summary>
            /// Specifies the name of the user being updated.
            /// </summary>
            [Required]

            public string Name { get; set; }

            /// <summary>
            /// Specifies the email address of a user.
            /// </summary>
            [Required]

            public string Email { get; set; }

            /// <summary>
            /// Specifies the unique identifier of a subscription assigned to a user.
            /// </summary>
            public int SubscriptionId { get; set; }
        }
    }
}