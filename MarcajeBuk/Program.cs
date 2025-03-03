using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

class Program
{
    static async Task Main(string[] args)
    {
        // Cargar la configuración desde appsettings.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Obtener las URLs
        string urlEntrada = configuration["Urls:UrlEntrada"];
        string urlSalida = configuration["Urls:UrlSalida"];

        // Obtener la lista de RUTs
        var ruts = configuration.GetSection("Ruts").Get<List<string>>();

        // Crear un cliente HTTP
        using (HttpClient client = new HttpClient())
        {
            foreach (var rut in ruts)
            {
                // Marcar entrada
                string entradaUrl = urlEntrada.Replace("{rut}", rut);
                await MakeRequest(client, entradaUrl, "Entrada", rut);

                // Marcar salida
                string salidaUrl = urlSalida.Replace("{rut}", rut);
                await MakeRequest(client, salidaUrl, "Salida", rut);
            }
        }
    }

    static async Task MakeRequest(HttpClient client, string url, string tipo, string rut)
    {
        try
        {
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"{tipo} marcada exitosamente para el RUT: {rut}");
            }
            else
            {
                Console.WriteLine($"Error al marcar {tipo} para el RUT: {rut}. Código de estado: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine