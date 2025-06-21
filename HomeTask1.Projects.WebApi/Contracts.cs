using System.ComponentModel.DataAnnotations;

namespace HomeTask1.Projects.WebApi;

public class Contracts
{
    public static class V1
    {
        /// <summary>
        /// Represents a request to create a user setting.
        /// </summary>
        public class CreateUserSetting
        {
            /// <summary>
            /// Specifies the unique identifier of a user.
            /// </summary>
            public int UserId { get; set; }

            /// <summary>
            /// Specifies the language preference for the user. Valid values for this property include "English" and "Spanish".
            /// </summary>
            [Required]
            public string Language { get; set; }

            /// <summary>
            /// Specifies the visual theme setting for a user. Valid values for this property include "Light" and "Dark".
            /// </summary>
            [Required]
            public string Theme { get; set; }
        }

        /// <summary>
        /// Represents a request to update user settings.
        /// </summary>
        public class UpdateUserSetting
        {
            /// <summary>
            /// Specifies the language preference of the user.  Valid values for this property include "English" and "Spanish".
            /// </summary>
            [Required]
            public string Language { get; set; }

            /// <summary>
            /// Specifies the visual theme setting for a user. Valid values for this property include "Light" and "Dark".
            /// </summary>
            [Required]
            public string Theme { get; set; }
        }

        /// <summary>
        /// Represents a request for creating a project.
        /// </summary>
        public class CreateProject
        {
            /// <summary>
            /// Specifies the unique identifier of the user associated with the project.
            /// </summary>
            public int UserId { get; set; }

            /// <summary>
            /// Specifies the name of the project.
            /// </summary>
            [Required]
            public string Name { get; set; }

            /// <summary>
            /// Specifies the collection of charts included in a project.
            /// </summary>
            public List<CreateChart> Charts { get; set; } = new();
        }

        /// <summary>
        /// Represents a chart configuration within a project.
        /// </summary>
        public class CreateChart
        {
            /// <summary>
            /// Specifies the symbol used in a chart.
            /// </summary>
            [Required]
            public string Symbol { get; set; }

            /// <summary>
            /// Specifies the timeframe associated with a chart.
            /// </summary>
            [Required]
            public string Timeframe { get; set; }

            /// <summary>
            /// Specifies a collection of indicators associated with a chart.
            /// </summary>
            public List<CreateIndicator> Indicators { get; set; } = new();
        }

        /// <summary>
        /// Represents an indicator used in a chart within a project.
        /// </summary>
        public class CreateIndicator
        {
            /// <summary>
            /// Specifies the name of the indicator.
            /// </summary>
            [Required]
            public string Name { get; set; }

            /// <summary>
            /// Specifies the parameters for the indicator.
            /// </summary>
            [Required]
            public string Parameters { get; set; }
        }

        /// <summary>
        /// Represents the data required to update an existing project.
        /// </summary>
        public class UpdateProject
        {
            /// <summary>
            /// Specifies the unique identifier of the user associated with the project.
            /// </summary>
            public int UserId { get; set; }

            /// <summary>
            /// Specifies the name of the project.
            /// </summary>
            [Required]
            public string Name { get; set; }

            /// <summary>
            /// Specifies the collection of charts included in a project.
            /// </summary>
            public List<UpdateChart> Charts { get; set; } = new(); 
        }

        /// <summary>
        /// Represents a chart that can be updated within a project.
        /// </summary>
        public class UpdateChart
        {
            /// <summary>
            /// Specifies the symbol used in a chart.
            /// </summary>
            [Required]
            public string Symbol { get; set; }

            /// <summary>
            /// Specifies the timeframe associated with a chart.
            /// </summary>
            [Required]
            public string Timeframe { get; set; }

            /// <summary>
            /// Specifies a collection of indicators associated with a chart.
            /// </summary>
            public List<UpdateIndicator> Indicators { get; set; } = new();
        }

        /// <summary>
        /// Represents a request to update an indicator associated with a chart.
        /// </summary>
        public class UpdateIndicator
        {
            /// <summary>
            /// Specifies the name of the indicator.
            /// </summary>
            [Required]
            public string Name { get; set; }

            /// <summary>
            /// Specifies the parameters for the indicator.
            /// </summary>
            [Required]
            public string Parameters { get; set; }
        }
    }
}

