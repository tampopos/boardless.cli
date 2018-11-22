using System.Collections.Generic;

namespace Tmpps.Infrastructure.Common.Messaging.Interfaces
{
    public interface ISQSConfig
    {
        string AwsAccessKeyId { get; }
        string AwsSecretAccessKey { get; }
        string ServiceURL { get; }
        SQSQueueSetting DefaultSQSQueueSetting { get; }
        IEnumerable<SQSQueueSetting> SQSQueueSettings { get; }
        IDictionary<string, SQSQueueSetting> SQSQueueSettingDictionary { get; }
        int MaxConcurrencyReceive { get; }
    }
}