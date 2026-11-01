namespace GymManagementPL.Extensions
{
    public static class GymAssistantServiceCollectionExtensions
    {
        public static IServiceCollection AddGymAssistant(this IServiceCollection services,IConfiguration configuration)
        {
            var aiSettings = configuration.GetSection(AiSettings.SectionName).Get<AiSettings>() ?? throw new InvalidOperationException("Missing 'AI' section in configuration.");
            services.AddKernel().AddOpenAIChatCompletion(modelId: aiSettings.ModelId, endpoint: new Uri(aiSettings.EndPoint), apiKey: aiSettings.ApiKey);
            services.AddScoped<PlansPlugin>();
            services.AddScoped<TrainersPlugin>();
            services.AddScoped<SessionsPlugin>();
            services.AddScoped<MembersPlugins>();
            services.AddMemoryCache();

            return services;
        }
    }
}
