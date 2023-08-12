using Amazon.CloudWatch.Model;
using NSubstitute;
using NUnit.Framework;
using Watchman.Configuration;

namespace Watchman.Engine.Tests.Generation.Dynamo.AlarmGeneratorTests
{
    public class NoTablesAreMatchedInTheConfig
    {
        [Test]
        public async Task AnSnsTopicIsCreatedForTriggeringTheAlert()
        {
            var mockery = new DynamoAlarmGeneratorMockery();
            var generator = mockery.AlarmGenerator;
            ConfigureTables(mockery);

            await generator.GenerateAlarmsFor(Config(), RunMode.GenerateAlarms);

            mockery.SnsTopicCreator.Verify(
                x => x.EnsureSnsTopic("TestGroup", false),
                Times.Once);
        }

        [Test]
        public async Task NoAlarmsAreCreated()
        {
            var mockery = new DynamoAlarmGeneratorMockery();
            var generator = mockery.AlarmGenerator;
            ConfigureTables(mockery);

            await generator.GenerateAlarmsFor(Config(), RunMode.GenerateAlarms);

            mockery.Cloudwatch.Verify(x =>
                x.PutMetricAlarmAsync(Arg.Any<PutMetricAlarmRequest>(), Arg.Any<CancellationToken>()), Times.Never);
        }

        private static void ConfigureTables(DynamoAlarmGeneratorMockery mockery)
        {
            mockery.ValidSnsTopic();
            mockery.GivenAListOfTables(new string[0]);
        }

        private static WatchmanConfiguration Config()
        {
            var dynamoConfig = new DynamoDb
            {
                Tables = new List<Table>
                {
                    new Table { Pattern = "nomatchwillbefoundforthis" }
                }
            };

            return AlertingGroupData.WrapDynamo(dynamoConfig);
        }
    }
}
