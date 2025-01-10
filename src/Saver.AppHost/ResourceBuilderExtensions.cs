using Microsoft.Extensions.Configuration;

namespace Saver.AppHost;

public static class ResourceBuilderExtensions
{
    public static IResourceBuilder<ProjectResource> WithIdentityEnvironment(
        this IResourceBuilder<ProjectResource> builder, EndpointReference identityEndpoint, string publicKey)
    {
        builder.WithEnvironment("Identity__Issuer", identityEndpoint);
        builder.WithEnvironment("Identity__PublicKey", publicKey);
        return builder;
    }

    public static IResourceBuilder<ProjectResource> WithOpenAiEnvironment(
        this IResourceBuilder<ProjectResource> builder, IConfiguration sharedConfiguration)
    {
        var openAiSection = sharedConfiguration.GetSection("OpenAI");
        var apiKey = openAiSection.GetValue<string>("ApiKey") ??
                     throw new InvalidOperationException("OpenAI:ApiKey not found in config!");

        var engine = openAiSection.GetValue<string>("Engine") ?? "gpt-4o-mini";

        builder.WithEnvironment("OpenAI__ApiKey", apiKey);
        builder.WithEnvironment("OpenAI__Engine", engine);

        return builder;
    }
}