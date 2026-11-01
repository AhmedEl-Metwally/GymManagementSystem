namespace GymManagementBLL.Filters
{
    public class FunctionLoggingFilter(ILogger<FunctionLoggingFilter> _logger) : IFunctionInvocationFilter
    {
        private const int MaxResultLength = 500;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public async Task OnFunctionInvocationAsync(FunctionInvocationContext context, Func<FunctionInvocationContext, Task> next)
        {
            var toolName = $"{context.Function.PluginName}-{context.Function.Name}";
            var hideResult = !AiToolPolicy.IsLogSafe(toolName);
            _logger.LogInformation("AI tool START [{Time}]: {Tool} ({Arguments})", DateTime.Now.ToString("HH:mm:ss.fff"), toolName, FormatArguments(context.Arguments));

            var stopWatch = Stopwatch.StartNew();

            try
            {
                await next(context);
                stopWatch.Stop();
                var result = hideResult ? "(redacted: personal data)" : Truncate(Serialize(context.Result));
                _logger.LogInformation("AI tool END [{Time}]: {Tool} took {Elapsed} ms. Result: {Result}", DateTime.Now.ToString("HH:mm:ss.fff"), toolName, stopWatch.ElapsedMilliseconds, result);
            }
            catch (Exception ex)
            {
                stopWatch.Stop();
                _logger.LogError(ex, "AI tool FAILED : {Tool} after {Elapsed} ms", toolName, stopWatch.ElapsedMilliseconds);
                throw;
            }
        }


        // Helper methods
        private static string FormatArguments(KernelArguments arguments)
            => string.Join(",", arguments.Select(A => AiToolPolicy.IsSensitiveArgument(A.Key) ? $"{A.Key}=***" : $"{A.Key}={A.Value}"));

        private static string Serialize(FunctionResult result)
        {
            var value = result.GetValue<object>();
            return value is null ? "null" : JsonSerializer.Serialize(value, JsonOptions);
        }

        private static string Truncate(string text) => text.Length <= MaxResultLength ? text : text[..MaxResultLength] + "....";
    }
}
