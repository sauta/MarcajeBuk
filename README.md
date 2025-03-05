Proyecto de Marcaje Automático con C#
Este proyecto es una aplicación de consola en C# que automatiza el proceso de marcaje de entrada y salida para una lista de RUTs, utilizando URLs específicas. Además, incluye un sistema de logs básico para registrar eventos importantes.

Tabla de Contenidos
Requisitos

Configuración

Estructura del Proyecto

Ejecución

Sistema de Logs

Personalización

Contribución

Requisitos
.NET 8.0

Un archivo appsettings.json con las URLs y la lista de RUTs.

Conexión a Internet para realizar las solicitudes HTTP.

Configuración
Archivo appsettings.json
El archivo appsettings.json contiene las URLs para el marcaje de entrada/salida y la lista de RUTs. Asegúrate de que tenga la siguiente estructura:

json
Copy
{
  "Urls": {
    "UrlEntradaSalida": "https://app.ctrlit.cl/ctrl/dial/registrarweb/RQgM9fO9cA?i={i}&lat=&lng=&r={rut}",
    "UrlInformación": "https://app.ctrlit.cl/ctrl/dial/infotrab/RQgM9fO9cA?i={i}&r={rut}"
  },
  "Ruts": [
    "18863583-0"
  ]
}
UrlEntradaSalida: URL para marcar entrada (i=1) o salida (i=0).

UrlInformación: URL para obtener información actualizada después del marcaje.

Ruts: Lista de RUTs para los cuales se realizará el marcaje.

Estructura del Proyecto
El proyecto tiene la siguiente estructura:

Copy
ProyectoMarcaje/
│
├── appsettings.json          # Archivo de configuración con URLs y RUTs
├── Program.cs                # Código principal de la aplicación
├── log.txt                   # Archivo de logs (se crea automáticamente)
└── README.md                 # Este archivo
Ejecución
Clona o descarga el proyecto:

bash
Copy
git clone https://github.com/tu-usuario/ProyectoMarcaje.git
cd ProyectoMarcaje
Configura el archivo appsettings.json:

Asegúrate de que las URLs y los RUTs estén correctamente configurados.

Compila y ejecuta el proyecto:

bash
Copy
dotnet run
Sistema de Logs
El proyecto incluye un sistema de logs básico que registra eventos importantes en un archivo de texto (log.txt). Los logs tienen el siguiente formato:

Copy
[Fecha y hora] [Nivel] Mensaje
Niveles de Log
INFO: Mensajes informativos (ejemplo: inicio de la aplicación, marcaje exitoso).

WARNING: Advertencias (ejemplo: marcaje en el mismo sentido).

ERROR: Errores (ejemplo: fallas en solicitudes HTTP).

Ejemplo de Logs
Copy
[2023-10-10 12:00:00] [INFO] Iniciando la aplicación...
[2023-10-10 12:00:01] [INFO] Intentando marcar Entrada para el RUT: 18863583-0
[2023-10-10 12:00:02] [INFO] Entrada marcada exitosamente para el RUT: 18863583-0
[2023-10-10 12:00:03] [INFO] Información actualizada para el RUT: 18863583-0
[2023-10-10 12:00:04] [INFO] Aplicación finalizada.
Personalización
Cambiar la ruta del archivo de logs
Puedes modificar la ruta del archivo de logs en la función Log:

csharp
Copy
static void Log(string message, string level)
{
    string logFilePath = "logs/log.txt"; // Cambia la ruta según tus necesidades
    // ...
}
Rotación de Logs
Si deseas implementar rotación de logs (por ejemplo, crear un nuevo archivo cada día), puedes modificar la función Log:

csharp
Copy
static void Log(string message, string level)
{
    string logFilePath = $"logs/log_{DateTime.Now:yyyy-MM-dd}.txt"; // Un archivo por día
    // ...
}
Contribución
Si deseas contribuir a este proyecto, sigue estos pasos:

Haz un fork del repositorio.

Crea una rama para tu contribución:

bash
Copy
git checkout -b mi-contribucion
Realiza tus cambios y haz commit:

bash
Copy
git commit -m "Agregar nueva funcionalidad"
Envía un pull request.

Licencia
Este proyecto está bajo la licencia MIT. Consulta el archivo LICENSE para más detalles.
