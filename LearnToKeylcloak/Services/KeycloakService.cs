using LearnToKeycloak.Dtos;
using LearnToKeylcloak.Dtos;
using LearnToKeylcloak.Options;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;

namespace LearnToKeylcloak.Services
{
    public class KeycloakService(IOptions<KeycloakConfiguration> options, IHttpClientFactory httpClientFactory)
    {

        public async Task<string> GetAccessToken(CancellationToken cancellationToken = default)
        {

            string endpoint = $"{options.Value.HostName}/realms/{options.Value.Realm}/protocol/openid-connect/token";

            var keyValuePairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", options.Value.ClientId),
                new KeyValuePair<string, string>("client_secret", options.Value.ClientSecret),
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
            };

            var response = await PostUrlEncodedFormAsync<GetAccesTokenResponseDto>(endpoint, keyValuePairs, false, cancellationToken);
            return response.AccessToken;
        }

        public async Task<T> GetAsync<T>(string endpoint, bool reqToken = false, CancellationToken cancellationToken = default)
        {
            var client = httpClientFactory.CreateClient();

            if (reqToken)
            {
                string token = await GetAccessToken();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }

            var response = await client.GetAsync(endpoint, cancellationToken);
            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };


            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
                string errorMessage;

                try
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var errorDto = JsonSerializer.Deserialize<BadRequestErrorResponseDto>(responseContent, jsonOptions);
                        errorMessage = errorDto?.ErrorMessage ?? "Bilinmeyen bir Bad Request hatası.";
                    }
                    else
                    {
                        var errorDto = JsonSerializer.Deserialize<ErrorResponseDto>(responseContent, jsonOptions);
                        errorMessage = errorDto?.ErrorDescription ?? $"Hata oluştu. Kod: {response.StatusCode}";
                    }
                }
                catch
                {

                    errorMessage = !string.IsNullOrEmpty(responseContent) ? responseContent : response.ReasonPhrase!;
                }


                throw new ApiException(errorMessage, response.StatusCode);
            }


            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return default!;
            }


            var successContent = await response.Content.ReadAsStringAsync(cancellationToken);


            if (typeof(T) == typeof(string))
            {
                return (T)(object)successContent;
            }

            var result = JsonSerializer.Deserialize<T>(successContent, jsonOptions);
            return result!;


        }
        //Exception açıklamalı versiyon
        public async Task<T> PostAsync<T>(string endpoint, object data, bool reqToken = false, CancellationToken cancellationToken = default)
        {

            string stringData = JsonSerializer.Serialize(data);
            var content = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");

            var client = httpClientFactory.CreateClient();

            if (reqToken)
            {
                string token = await GetAccessToken();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }

            var response = await client.PostAsync(endpoint, content, cancellationToken);

            // JSON Case-Sensitivity sorunlarını önlemek için ayar
            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            // 1. HATA YÖNETİMİ
            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
                string errorMessage;

                try
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var errorDto = JsonSerializer.Deserialize<BadRequestErrorResponseDto>(responseContent, jsonOptions);
                        errorMessage = errorDto?.ErrorMessage ?? "Bilinmeyen bir Bad Request hatası.";
                    }
                    else
                    {
                        var errorDto = JsonSerializer.Deserialize<ErrorResponseDto>(responseContent, jsonOptions);
                        errorMessage = errorDto?.ErrorDescription ?? $"Hata oluştu. Kod: {response.StatusCode}";
                    }
                }
                catch
                {
                    // JSON parse edilemezse ham mesajı veya genel bir mesajı kullan
                    errorMessage = !string.IsNullOrEmpty(responseContent) ? responseContent : response.ReasonPhrase!;
                }

                // Burada dynamic return yapmak yerine HATA FIRLATIYORUZ
                throw new ApiException(errorMessage, response.StatusCode);
            }

            // 2. BAŞARI YÖNETİMİ (204 No Content durumu)
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return default!;
            }

            // 3. BAŞARI YÖNETİMİ (Data varsa)
            var successContent = await response.Content.ReadAsStringAsync(cancellationToken);

            // Eğer T string ise direkt döndür, yoksa deserialize et
            if (typeof(T) == typeof(string))
            {
                return (T)(object)successContent;
            }

            var result = JsonSerializer.Deserialize<T>(successContent, jsonOptions);
            return result!;


        }

        public async Task<T> PostUrlEncodedFormAsync<T>(string endpoint, List<KeyValuePair<string, string>> data, bool reqToken = false, CancellationToken cancellationToken = default)
        {

            var client = httpClientFactory.CreateClient();

            if (reqToken)
            {
                string token = await GetAccessToken();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }

            var response = await client.PostAsync(endpoint, new FormUrlEncodedContent(data), cancellationToken);
            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };


            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
                string errorMessage;

                try
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var errorDto = JsonSerializer.Deserialize<BadRequestErrorResponseDto>(responseContent, jsonOptions);
                        errorMessage = errorDto?.ErrorMessage ?? "Bilinmeyen bir Bad Request hatası.";
                    }
                    else
                    {
                        var errorDto = JsonSerializer.Deserialize<ErrorResponseDto>(responseContent, jsonOptions);
                        errorMessage = errorDto?.ErrorDescription ?? $"Hata oluştu. Kod: {response.StatusCode}";
                    }
                }
                catch
                {

                    errorMessage = !string.IsNullOrEmpty(responseContent) ? responseContent : response.ReasonPhrase!;
                }


                throw new ApiException(errorMessage, response.StatusCode);
            }


            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return default!;
            }


            var successContent = await response.Content.ReadAsStringAsync(cancellationToken);


            if (typeof(T) == typeof(string))
            {
                return (T)(object)successContent;
            }

            var result = JsonSerializer.Deserialize<T>(successContent, jsonOptions);
            return result!;

        }

        public async Task<T> PutAsync<T>(string endpoint, object data, bool reqToken = false, CancellationToken cancellationToken = default)
        {

            string stringData = JsonSerializer.Serialize(data);
            var content = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");

            var client = httpClientFactory.CreateClient();

            if (reqToken)
            {
                string token = await GetAccessToken();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }

            var response = await client.PutAsync(endpoint, content, cancellationToken);

            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

   
            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
                string errorMessage;

                try
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var errorDto = JsonSerializer.Deserialize<BadRequestErrorResponseDto>(responseContent, jsonOptions);
                        errorMessage = errorDto?.ErrorMessage ?? "Bilinmeyen bir Bad Request hatası.";
                    }
                    else
                    {
                        var errorDto = JsonSerializer.Deserialize<ErrorResponseDto>(responseContent, jsonOptions);
                        errorMessage = errorDto?.ErrorDescription ?? $"Hata oluştu. Kod: {response.StatusCode}";
                    }
                }
                catch
                {
                    
                    errorMessage = !string.IsNullOrEmpty(responseContent) ? responseContent : response.ReasonPhrase!;
                }

       
                throw new ApiException(errorMessage, response.StatusCode);
            }

     
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return default!;
            }

   
            var successContent = await response.Content.ReadAsStringAsync(cancellationToken);

  
            if (typeof(T) == typeof(string))
            {
                return (T)(object)successContent;
            }

            var result = JsonSerializer.Deserialize<T>(successContent, jsonOptions);
            return result!;

        }

        public async Task<T> DeleteAsync<T>(string endpoint, bool reqToken = false, CancellationToken cancellationToken = default)
        {

            var client = httpClientFactory.CreateClient();

            if (reqToken)
            {
                string token = await GetAccessToken();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }

            var response = await client.DeleteAsync(endpoint, cancellationToken);
       
            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

          
            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
                string errorMessage;

                try
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var errorDto = JsonSerializer.Deserialize<BadRequestErrorResponseDto>(responseContent, jsonOptions);
                        errorMessage = errorDto?.ErrorMessage ?? "Bilinmeyen bir Bad Request hatası.";
                    }
                    else
                    {
                        var errorDto = JsonSerializer.Deserialize<ErrorResponseDto>(responseContent, jsonOptions);
                        errorMessage = errorDto?.ErrorDescription ?? $"Hata oluştu. Kod: {response.StatusCode}";
                    }
                }
                catch
                {
                    
                    errorMessage = !string.IsNullOrEmpty(responseContent) ? responseContent : response.ReasonPhrase!;
                }


                throw new ApiException(errorMessage, response.StatusCode);
            }


            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return default!;
            }

       
            var successContent = await response.Content.ReadAsStringAsync(cancellationToken);

   
            if (typeof(T) == typeof(string))
            {
                return (T)(object)successContent;
            }

            var result = JsonSerializer.Deserialize<T>(successContent, jsonOptions);
            return result!;
        }

        public async Task<T> DeleteAsync<T>(string endpoint, object data ,bool reqToken = false, CancellationToken cancellationToken = default)
        {

            var client = httpClientFactory.CreateClient();

            if (reqToken)
            {
                string token = await GetAccessToken();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(endpoint),
                Content = new StringContent(JsonSerializer.Serialize(data), System.Text.Encoding.UTF8, "application/json")
            };

            var response = await client.SendAsync(request, cancellationToken);

            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };


            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
                string errorMessage;

                try
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var errorDto = JsonSerializer.Deserialize<BadRequestErrorResponseDto>(responseContent, jsonOptions);
                        errorMessage = errorDto?.ErrorMessage ?? "Bilinmeyen bir Bad Request hatası.";
                    }
                    else
                    {
                        var errorDto = JsonSerializer.Deserialize<ErrorResponseDto>(responseContent, jsonOptions);
                        errorMessage = errorDto?.ErrorDescription ?? $"Hata oluştu. Kod: {response.StatusCode}";
                    }
                }
                catch
                {

                    errorMessage = !string.IsNullOrEmpty(responseContent) ? responseContent : response.ReasonPhrase!;
                }


                throw new ApiException(errorMessage, response.StatusCode);
            }


            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return default!;
            }


            var successContent = await response.Content.ReadAsStringAsync(cancellationToken);


            if (typeof(T) == typeof(string))
            {
                return (T)(object)successContent;
            }

            var result = JsonSerializer.Deserialize<T>(successContent, jsonOptions);
            return result!;
        }

    }

    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public ApiException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }

}


