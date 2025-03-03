using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

class Program
{
    static void Main(string[] args)
    {
        var directory = Directory.GetCurrentDirectory();
        // Cargar la configuración desde appsettings.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(directory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Obtener las URLs
        string urlEntradaSalida = configuration["Urls:UrlEntradaSalida"];
        string urlInformacion = configuration["Urls:UrlInformación"];

        // Obtener la lista de RUTs
        var ruts = configuration.GetSection("Ruts").Get<List<string>>();

        // Crear un cliente HTTP
        using (HttpClient client = new HttpClient())
        {
            foreach (var rut in ruts)
            {
                // Obtener la hora actual
                DateTime now = DateTime.Now;
                string tipo = now.Hour < 12 ? "Entrada" : "Salida"; // Antes de las 12 PM es entrada, después es salida
                int i = now.Hour < 12 ? 1 : 0; // 1 para entrada, 0 para salida

                // Reemplazar el RUT y el parámetro i en la URL de marcaje
                string marcajeUrl = urlEntradaSalida
                    .Replace("{i}", i.ToString())
                    .Replace("{rut}", rut);

                // Realizar la solicitud HTTP para marcar entrada o salida
                string response = MakeRequest(client, marcajeUrl);

                // Verificar la respuesta
                if (response == "ok")
                {
                    Console.WriteLine($"{tipo} marcada exitosamente para el RUT: {rut}");

                    // Realizar la segunda solicitud para obtener la información actualizada
                    string infoUrl = urlInformacion
                        .Replace("{i}", i.ToString())
                        .Replace("{rut}", rut);

                    string infoResponse = MakeRequest(client, infoUrl);

                    if (infoResponse != null)
                    {
                        Console.WriteLine($"Información actualizada para el RUT: {rut}");
                        Console.WriteLine(infoResponse); // Mostrar la información devuelta
                    }
                }
                else if (response.Contains("Marcas en mismo <br> sentido"))
                {
                    Console.WriteLine($"Advertencia: {response} para el RUT: {rut}");
                }
                else
                {
                    Console.WriteLine($"Error al marcar {tipo} para el RUT: {rut}. Respuesta: {response}");
                }
            }
        }
    }


    static string MakeRequest(HttpClient client, string url)
    {
        try
        {
            HttpResponseMessage response = client.GetAsync(url).Result; // Llamada síncrona
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsStringAsync().Result; // Leer la respuesta como string
            }
            else
            {
                return $"Error: {response.StatusCode}";
            }
        }
        catch (Exception ex)
        {
            return $"Excepción: {ex.Message}";
        }
    }
}