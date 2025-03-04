using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

class Program
{
    static void Main(string[] args)
    {
        Log("Iniciando la aplicación...", "INFO");

        try
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

                    Log($"Intentando marcar {tipo} para el RUT: {rut}", "INFO");

                    // Realizar la solicitud HTTP para marcar entrada o salida
                    string response = MakeRequest(client, marcajeUrl);

                    // Verificar la respuesta
                    if (response == "ok")
                    {
                        Log($"{tipo} marcada exitosamente para el RUT: {rut}", "INFO");

                        // Realizar la segunda solicitud para obtener la información actualizada
                        string infoUrl = urlInformacion
                            .Replace("{i}", i.ToString())
                            .Replace("{rut}", rut);

                        string infoResponse = MakeRequest(client, infoUrl);

                        if (infoResponse != null)
                        {
                            Log($"Información actualizada para el RUT: {rut}", "INFO");
                            Log(infoResponse, "DEBUG"); // Mostrar la información devuelta
                        }
                    }
                    else if (response.Contains("marcaje en el mismo sentido"))
                    {
                        Log($"Advertencia: {response} para el RUT: {rut}", "WARNING");
                    }
                    else
                    {
                        Log($"Error al marcar {tipo} para el RUT: {rut}. Respuesta: {response}", "ERROR");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Log($"Ocurrió un error inesperado: {ex.Message}", "ERROR");
        }
        finally
        {
            Log("Aplicación finalizada.", "INFO");
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
                Log($"Error en la solicitud HTTP. Código de estado: {response.StatusCode}", "ERROR");
                return $"Error: {response.StatusCode}";
            }
        }
        catch (Exception ex)
        {
            Log($"Excepción en la solicitud HTTP: {ex.Message}", "ERROR");
            return $"Excepción: {ex.Message}";
        }
    }

    static void Log(string message, string level)
    {
        // Ruta del archivo de log
        string logFilePath = "log.txt";

        // Formato del mensaje: [Fecha y hora] [Nivel] Mensaje
        string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";

        // Escribir el mensaje en el archivo de log
        using (StreamWriter writer = new StreamWriter(logFilePath, true))
        {
            writer.WriteLine(logMessage);
        }

        // También mostrar el mensaje en la consola (opcional)
        Console.WriteLine(logMessage);
    }
}