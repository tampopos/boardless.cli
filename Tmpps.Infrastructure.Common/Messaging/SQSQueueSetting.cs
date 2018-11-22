using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Tmpps.Infrastructure.Common.Messaging
{
    public class SQSQueueSetting
    {
        public SQSQueueSetting(IConfiguration configuration)
        {
            this.QueueUrl = configuration.GetValue<string>(nameof(this.QueueUrl));
            this.FirstDelaySeconds = configuration.GetValue(nameof(this.FirstDelaySeconds), 0);
            this.DelayType = configuration.GetValue(nameof(this.DelayType), SQSDelayType.FirstTimeOnly);
            this.TargetMessageTypes = configuration.GetSection(nameof(this.TargetMessageTypes)).Get<string[]>() ?? Enumerable.Empty<string>();
            this.InstanceCount = configuration.GetValue(nameof(this.InstanceCount), 1);
            this.MaxReceiveCount = configuration.GetValue<int>(nameof(this.MaxReceiveCount));
            this.DeadLetterQueueUrl = configuration.GetValue<string>(nameof(this.DeadLetterQueueUrl));
        }
        public string QueueUrl { get; }
        public int InstanceCount { get; }
        public int FirstDelaySeconds { get; }
        public SQSDelayType DelayType { get; }
        public IEnumerable<string> TargetMessageTypes { get; }
        public int MaxReceiveCount { get; set; }
        public string DeadLetterQueueUrl { get; set; }
    }
}