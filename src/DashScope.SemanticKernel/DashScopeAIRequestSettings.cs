using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.SemanticKernel.AI;

namespace DashScope.SemanticKernel;

public class DashScopeAIRequestSettings : AIRequestSettings
{
    [JsonPropertyName("temperature")]
    public float? Temperature { get; set; }

    [JsonPropertyName("top_p")]
    public float? TopP { get; set; }
    
    private static readonly JsonSerializerOptions s_options = new()
    {
        WriteIndented = true,
        MaxDepth = 20,
        AllowTrailingCommas = true,
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip
    };
    
    public static DashScopeAIRequestSettings FromRequestSettings(AIRequestSettings? requestSettings, int? defaultMaxTokens = null)
    {
        if (requestSettings is null)
        {
            return new DashScopeAIRequestSettings();
        }

        if (requestSettings is DashScopeAIRequestSettings requestSettingDashScopeAIRequestSettings)
        {
            return requestSettingDashScopeAIRequestSettings;
        }

        var json = JsonSerializer.Serialize(requestSettings);
        var dashScopeAIRequestSettings = JsonSerializer.Deserialize<DashScopeAIRequestSettings>(json, s_options);

        if (dashScopeAIRequestSettings is not null)
        {
            return dashScopeAIRequestSettings;
        }

        throw new ArgumentException($"Invalid request settings, cannot convert to {nameof(DashScopeAIRequestSettings)}", nameof(requestSettings));
    }
    
}