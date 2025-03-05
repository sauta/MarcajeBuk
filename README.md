# **Proyecto de Marcaje Automático con C#**

Este proyecto es una aplicación de consola en C# que automatiza el proceso de marcaje de entrada y salida para una lista de RUTs, utilizando URLs específicas. Además, incluye un sistema de logs básico para registrar eventos importantes.

---

## **Tabla de Contenidos**
1. [Requisitos](#requisitos)
2. [Configuración](#configuración)
3. [Estructura del Proyecto](#estructura-del-proyecto)
4. [Ejecución](#ejecución)
5. [Sistema de Logs](#sistema-de-logs)
6. [Personalización](#personalización)
7. [Contribución](#contribución)

---

## **Requisitos**

- [.NET 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- Un archivo `appsettings.json` con las URLs y la lista de RUTs.
- Conexión a Internet para realizar las solicitudes HTTP.

---

## **Configuración**

### **Archivo `appsettings.json`**

El archivo `appsettings.json` contiene las URLs para el marcaje de entrada/salida y la lista de RUTs. Asegúrate de que tenga la siguiente estructura:

```json
{
  "Urls": {
    "UrlEntradaSalida": "https://app.ctrlit.cl/ctrl/dial/registrarweb/RQgM9fO9cA?i={i}&lat=&lng=&r={rut}",
    "UrlInformación": "https://app.ctrlit.cl/ctrl/dial/infotrab/RQgM9fO9cA?i={i}&r={rut}"
  },
  "Ruts": [
    "18863681-0"
  ]
}
