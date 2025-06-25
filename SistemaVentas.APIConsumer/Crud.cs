using Newtonsoft.Json;
using System.Text;
using SistemaVentas.Model;

namespace SistemaVentas.APIConsumer
{
    public static class Crud<T>
    {
        public static string EndPoint { get; set; }

        // Método para obtener todos los registros
        public static List<T> GetAll()
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(EndPoint).Result;
                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<List<T>>(json);
                }
                else
                {
                    throw new Exception($"Error: {response.StatusCode}");
                }
            }
        }

        // Método para obtener un registro por su ID
        public static T GetById(int id)
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync($"{EndPoint}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<T>(json);
                }
                else
                {
                    throw new Exception($"Error: {response.StatusCode}");
                }
            }
        }

        // Método para crear un nuevo registro
        public static T Create(T item)
        {
            using (var client = new HttpClient())
            {
                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(item),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = client.PostAsync(EndPoint, jsonContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<T>(json);
                }
                else
                {
                    var errorContent = response.Content.ReadAsStringAsync().Result;
                    throw new Exception($"Error: {response.StatusCode} - {errorContent}");
                }
            }
        }

        // Método para actualizar un registro existente
        public static bool Update(int id, T item)
        {
            using (var client = new HttpClient())
            {
                var response = client.PutAsync(
                        $"{EndPoint}/{id}",
                        new StringContent(
                            JsonConvert.SerializeObject(item),
                            Encoding.UTF8,
                            "application/json"
                        )
                    ).Result;

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var errorMessage = response.Content.ReadAsStringAsync().Result;
                    throw new Exception($"Error: {response.StatusCode} - {errorMessage}");
                }
            }
        }

        // Método para eliminar un registro
        public static bool Delete(int id)
        {
            using (var client = new HttpClient())
            {
                var response = client.DeleteAsync($"{EndPoint}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var errorMessage = response.Content.ReadAsStringAsync().Result;
                    throw new Exception($"Error: {response.StatusCode} - {errorMessage}");
                }
            }
        }
    }
}
