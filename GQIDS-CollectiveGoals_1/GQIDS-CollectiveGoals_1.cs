namespace GQIDS_CollectiveGoals_1
{
    using System;
    using System.Collections.Generic;

    using Skyline.DataMiner.Analytics.GenericInterface;
    using Skyline.DataMiner.Net.Messages;

    [GQIMetaData(Name = "GQIDS - Collective Goals")]
    public class MyDataSource : IGQIDataSource, IGQIInputArguments
    {
        // Input Arguments:
        private readonly GQIIntArgument _totalPointsArgument;
        private readonly GQIIntArgument _collectiveGoalArgument;

        // Data Source Columns:
        private readonly List<GQIColumn> _columns;

        // Input Values:
        private int _collectiveGoal;
        private int _totalPoints;

        public MyDataSource()
        {
            _totalPointsArgument = new GQIIntArgument("Total Points")
            {
                IsRequired = true,
            };

            _collectiveGoalArgument = new GQIIntArgument("Collective Goal")
            {
                IsRequired = true,
            };

            _columns = new List<GQIColumn>
            {
                new GQIIntColumn("Goal"),
                new GQIDoubleColumn("Completion Rate"),
            };
        }

        public GQIArgument[] GetInputArguments()
        {
            return new GQIArgument[]
            {
                _totalPointsArgument,
                _collectiveGoalArgument,
            };
        }

        public OnArgumentsProcessedOutputArgs OnArgumentsProcessed(OnArgumentsProcessedInputArgs args)
        {
            // Setting Values provided by user:
            _collectiveGoal = args.GetArgumentValue(_collectiveGoalArgument);
            _totalPoints = args.GetArgumentValue(_totalPointsArgument);
            return default;
        }

        public GQIColumn[] GetColumns()
        {
            return _columns.ToArray();
        }

        public GQIPage GetNextPage(GetNextPageInputArgs args)
        {
            // Completion Rate:
            double completionRate = Convert.ToDouble(_totalPoints * 100)/_collectiveGoal;
            double roundedToOneDecimal = Math.Round(completionRate, 1);

            var cells = new[]
            {
                // Goal:
                new GQICell
                {
                    Value = _collectiveGoal,
                    DisplayValue = _collectiveGoal.ToString("N0"),
                },

                // Completion Rate:
                new GQICell
                {
                    Value = roundedToOneDecimal,
                    DisplayValue = $"{roundedToOneDecimal} %",
                },
            };
            var row = new GQIRow(cells);

            return new GQIPage(new[] { row })
            {
                HasNextPage = false,
            };
        }
    }
}