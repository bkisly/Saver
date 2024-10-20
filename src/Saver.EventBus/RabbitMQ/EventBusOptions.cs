﻿namespace Saver.EventBus.RabbitMQ;

public class EventBusOptions
{
    public string SubscriptionClientName { get; set; } = string.Empty;
    public int RetryCount { get; set; } = 10;
}