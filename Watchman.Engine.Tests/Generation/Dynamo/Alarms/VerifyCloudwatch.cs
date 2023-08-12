using Amazon.CloudWatch;
using Amazon.CloudWatch.Model;
using NSubstitute;
using Watchman.Engine.Alarms;

namespace Watchman.Engine.Tests.Generation.Dynamo.Alarms
{
    public static class VerifyCloudwatch
    {
        public static void AlarmFinderFindsThreshold(IAlarmFinder alarmFinder,
            double threshold, int period, string action)
        {
            alarmFinder.Setup(x => x.FindAlarmByName(Arg.Any<string>()))
                .Returns(new MetricAlarm
                {
                    Threshold = threshold,
                    EvaluationPeriods = 1,
                    Period = period,
                    AlarmActions = new List<string> { action },
                    OKActions = new List<string> { action }
                });
        }

        public static void PutMetricAlarmWasCalledOnce(IAmazonCloudWatch cloudWatch)
        {
            cloudWatch.Verify(x => x.PutMetricAlarmAsync(
                Arg.Any<PutMetricAlarmRequest>(), Arg.Any<CancellationToken>()),
                Times.Once);
        }

        public static void PutMetricAlarmWasNotCalled(IAmazonCloudWatch cloudWatch)
        {
            cloudWatch.Verify(x => x.PutMetricAlarmAsync(
                Arg.Any<PutMetricAlarmRequest>(), Arg.Any<CancellationToken>()),
                Times.Never);
        }

    }
}
