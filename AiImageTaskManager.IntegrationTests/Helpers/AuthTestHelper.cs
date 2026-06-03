using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace AiImageTaskManager.IntegrationTests.Helpers;

public static class AuthTestHelper
{
    public static async Task AuthenticateAsync(HttpClient client)
    {
        var email = $"test-{Guid.NewGuid()}@example.com";

        var registerRequest = new
        {
            email,
            displayName = "Integration Test User",
            password = "P@ssw0rd123"
        };

        var response = await client.PostAsJsonAsync("/api/auth/register", registerRequest);

        response.EnsureSuccessStatusCode();

        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseForTest>();

        if (authResponse == null || string.IsNullOrWhiteSpace(authResponse.Token))
        {
            throw new InvalidOperationException("Failed to get JWT token.");
        }

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", authResponse.Token);
    }

    private class AuthResponseForTest
    {
        public string Token { get; set; } = string.Empty;
    }
}