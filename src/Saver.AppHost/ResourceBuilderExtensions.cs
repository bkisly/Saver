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
}