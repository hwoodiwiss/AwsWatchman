using NSubstitute;
using Watchman.AwsResources;
using Watchman.AwsResources.Services.Sqs;
using Watchman.Engine.Generation.Sqs;

namespace Watchman.Engine.Tests.Generation.Sqs
{
    public static class VerifyQueues
    {
        public static void ReturnsQueues(IResourceSource<QueueData> queueSource, List<string queueNames)
        {
            queueSource.Setup(x => x.GetResourceNamesAsync())
                .Returns(queueNames);
        }

        public static void EnsureLengthAlarm(IQueueAlarmCreator alarmCreator,
            string queueName, bool isDryRun)
        {
            alarmCreator.Verify(x => x.EnsureLengthAlarm(queueName,
                Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>(), isDryRun), Times.Once);
        }

        public static void EnsureLengthAlarm(IQueueAlarmCreator alarmCreator,
            string queueName, int threshold, bool isDryRun)
        {
            alarmCreator.Verify(x => x.EnsureLengthAlarm(queueName,
                threshold, Arg.Any<string>(), Arg.Any<string>(), isDryRun), Times.Once);
        }

        public static void EnsureOldestMessageAlarm(IQueueAlarmCreator alarmCreator,
          string queueName, int threshold, bool isDryRun)
        {
            alarmCreator.Verify(x => x.EnsureOldestMessageAlarm(queueName,
                threshold, Arg.Any<string>(), Arg.Any<string>(), isDryRun), Times.Once);
        }

        public static void NoLengthAlarm(IQueueAlarmCreator alarmCreator, string queueName)
        {
            alarmCreator.Verify(x => x.EnsureLengthAlarm(queueName,
                Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>()), Times.Never);
        }

        public static void NoOldestMessageAlarm(IQueueAlarmCreator alarmCreator, string queueName)
        {
            alarmCreator.Verify(x => x.EnsureOldestMessageAlarm(queueName,
                Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>()), Times.Never);
        }
    }
}
