using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;
using Newtonsoft.Json;

namespace QuantConnect.Tests
{
    [TestFixture, Category("TravisExclude")]
    public class FkuTestRunner
    {
        [Test, TestCaseSource(nameof(GetRegressionTestParameters))]
        public void AlgorithmStatisticsRegression(AlgorithmStatisticsTestParameters parameters)
        {
            QuantConnect.Configuration.Config.Set("quandl-auth-token", "WyAazVXnq7ATy_fefTqm");
            QuantConnect.Configuration.Config.Set("forward-console-messages", "false");

            if (parameters.Algorithm == "OptionChainConsistencyRegressionAlgorithm")
            {
                // special arrangement for consistency test - we check if limits work fine
                QuantConnect.Configuration.Config.Set("symbol-minute-limit", "100");
                QuantConnect.Configuration.Config.Set("symbol-second-limit", "100");
                QuantConnect.Configuration.Config.Set("symbol-tick-limit", "100");
            }

            if (parameters.Algorithm == "BasicTemplateIntrinioEconomicData")
            {
                var intrinioCredentials = new Dictionary<string, string>
                {
                    {"intrinio-username", "121078c02c20a09aa5d9c541087e7fa4"},
                    {"intrinio-password", "65be35238b14de4cd0afc0edf364efc3" }
                };
                QuantConnect.Configuration.Config.Set("parameters", JsonConvert.SerializeObject(intrinioCredentials));
            }

            AlgorithmRunner.RunLocalBacktest(parameters.Algorithm, parameters.Statistics, parameters.AlphaStatistics, parameters.Language);
        }

        private static TestCaseData[] GetRegressionTestParameters()
        {
         
            var fkuButtomReversalStatistics = new Dictionary<string, string>
            {
                {"Total Trades", "1"},
                {"Average Win", "0%"},
                {"Average Loss", "0%"},
                {"Compounding Annual Return", "257.026%"},
                {"Drawdown", "2.200%"},
                {"Expectancy", "0"},
                {"Net Profit", "1.640%"},
                {"Sharpe Ratio", "4.391"},
                {"Loss Rate", "0%"},
                {"Win Rate", "0%"},
                {"Profit-Loss Ratio", "0"},
                {"Alpha", "-0.01"},
                {"Beta", "76.612"},
                {"Annual Standard Deviation", "0.19"},
                {"Annual Variance", "0.036"},
                {"Information Ratio", "4.334"},
                {"Tracking Error", "0.19"},
                {"Treynor Ratio", "0.011"},
                {"Total Fees", "$1.00"}
            };

            var volumeWeightedAveragePriceExecutionModelRegressionAlgorithmStatistics = new Dictionary<string, string>
            {
                {"Total Trades", "61"},
                {"Average Win", "0.10%"},
                {"Average Loss", "0%"},
                {"Compounding Annual Return", "585.503%"},
                {"Drawdown", "0.600%"},
                {"Expectancy", "0"},
                {"Net Profit", "2.492%"},
                {"Sharpe Ratio", "9.136"},
                {"Loss Rate", "0%"},
                {"Win Rate", "100%"},
                {"Profit-Loss Ratio", "0"},
                {"Alpha", "0"},
                {"Beta", "113.313"},
                {"Annual Standard Deviation", "0.137"},
                {"Annual Variance", "0.019"},
                {"Information Ratio", "9.063"},
                {"Tracking Error", "0.137"},
                {"Treynor Ratio", "0.011"},
                {"Total Fees", "$96.79"},
                {"Total Insights Generated", "5"},
                {"Total Insights Closed", "3"},
                {"Total Insights Analysis Completed", "0"},
                {"Long Insight Count", "3"},
                {"Short Insight Count", "2"},
                {"Long/Short Ratio", "150.0%"},
                {"Estimated Monthly Alpha Value", "$54250.3481"},
                {"Total Accumulated Estimated Alpha Value", "$8740.3339"},
                {"Mean Population Estimated Insight Value", "$2913.4446"},
                {"Mean Population Direction", "0%"},
                {"Mean Population Magnitude", "0%"},
                {"Rolling Averaged Population Direction", "0%"},
                {"Rolling Averaged Population Magnitude", "0%"},
            };

            var standardDeviationExecutionModelRegressionAlgorithmStatistics = new Dictionary<string, string>
            {
                {"Total Trades", "63"},
                {"Average Win", "0.06%"},
                {"Average Loss", "0%"},
                {"Compounding Annual Return", "793.499%"},
                {"Drawdown", "0.400%"},
                {"Expectancy", "0"},
                {"Net Profit", "2.840%"},
                {"Sharpe Ratio", "10.781"},
                {"Loss Rate", "0%"},
                {"Win Rate", "100%"},
                {"Profit-Loss Ratio", "0"},
                {"Alpha", "0"},
                {"Beta", "128.815"},
                {"Annual Standard Deviation", "0.132"},
                {"Annual Variance", "0.017"},
                {"Information Ratio", "10.71"},
                {"Tracking Error", "0.132"},
                {"Treynor Ratio", "0.011"},
                {"Total Fees", "$76.61"},
                {"Total Insights Generated", "5"},
                {"Total Insights Closed", "3"},
                {"Total Insights Analysis Completed", "0"},
                {"Long Insight Count", "3"},
                {"Short Insight Count", "2"},
                {"Long/Short Ratio", "150.0%"},
                {"Estimated Monthly Alpha Value", "$54250.3481"},
                {"Total Accumulated Estimated Alpha Value", "$8740.3339"},
                {"Mean Population Estimated Insight Value", "$2913.4446"},
                {"Mean Population Direction", "0%"},
                {"Mean Population Magnitude", "0%"},
                {"Rolling Averaged Population Direction", "0%"},
                {"Rolling Averaged Population Magnitude", "0%"},
            };

            return new List<AlgorithmStatisticsTestParameters>
            {
                // CSharp
                new AlgorithmStatisticsTestParameters("FkuButtomReversalAlgorithm", fkuButtomReversalStatistics, Language.CSharp),
                // new AlgorithmStatisticsTestParameters("VolumeWeightedAveragePriceExecutionModelRegressionAlgorithm", volumeWeightedAveragePriceExecutionModelRegressionAlgorithmStatistics, Language.CSharp),
                // new AlgorithmStatisticsTestParameters("StandardDeviationExecutionModelRegressionAlgorithm", standardDeviationExecutionModelRegressionAlgorithmStatistics, Language.CSharp)
            }.Select(x => new TestCaseData(x).SetName(x.Language + "/" + x.Algorithm)).ToArray();
        }

        public class AlgorithmStatisticsTestParameters
        {
            public readonly string Algorithm;
            public readonly Dictionary<string, string> Statistics;
            public readonly AlphaRuntimeStatistics AlphaStatistics;
            public readonly Language Language;

            public AlgorithmStatisticsTestParameters(string algorithm, Dictionary<string, string> statistics, Language language)
            {
                Algorithm = algorithm;
                Statistics = statistics;
                Language = language;
            }
        }
    }

}
