﻿using System.Collections.Immutable;
using X.Extensions.Serilog.Sinks.Telegram.Batch;
using X.Extensions.Serilog.Sinks.Telegram.Batch.Contracts;
using X.Extensions.Serilog.Sinks.Telegram.Batch.Rules;

namespace X.Extensions.Serilog.Sinks.Telegram.Configuration;

/// <summary>
/// Represents a configuration for managing the batch emitting rules.
/// </summary>
public class BatchEmittingRulesConfiguration
{
    private readonly TimeSpan _rulesCheckPeriod = TelegramSinkDefaults.RulesCheckPeriod;

    /// <summary>
    /// Gets or initializes the check period for batch emit rules.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when an invalid timespan (less than or equal to zero) is provided.</exception>
    public TimeSpan RuleCheckPeriod
    {
        get => _rulesCheckPeriod;
        init
        {
            if (value <= TimeSpan.Zero)
            {
                throw new ArgumentException(
                    "Invalid batch emit rules check period! It must be greater than TimeSpan.Zero!");
            }

            _rulesCheckPeriod = value;
        }
    }

    /// <summary>
    /// Gets or initializes the batch processing rules to be applied.
    /// </summary>
    public IImmutableList<IRule> BatchProcessingRules { get; init; } = ImmutableList<IRule>.Empty;

    /// <summary>
    /// Gets or initializes the async batch processing rules to be applied.
    /// </summary>
    public IImmutableList<IRuleAsync> AsyncBatchProcessingRules { get; init; } = ImmutableList<IRuleAsync>.Empty;

    /// <summary>
    /// Gets the execution hooks that are extracted from the batch processing rules.
    /// </summary>
    // TODO: add an option to add hooks separately from rules.
    public IImmutableList<IPostExecutionHook> BatchPostExecutionHooks
        => BatchProcessingRules
            .Where(rule => rule is IPostExecutionHook)
            .Cast<IPostExecutionHook>()
            .ToImmutableList();
}