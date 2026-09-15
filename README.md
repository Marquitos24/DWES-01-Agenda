# Práctica de Agenda

Este repositorio contiene el desarrollo de una aplicación de gestión de agenda de contactos desarrollada en C# como primera práctica del curso de segundo de DAW. El proyecto está estructurado por capas utilizando una solución moderna en formato `.slnx`.

---

## 🏗️ Arquitectura y Estructura de la Solución

La solución se divide modularmente para separar las responsabilidades de negocio, el almacenamiento y las pruebas automatizadas, segun como especifica el SOLID:

- **PracticaAgenda.Logica:** Contiene los modelos, la gestión de datos, la lógica de negocio, el sistema de caché y los registros de actividad.
- **PracticaAgenda.Test:** Proyecto dedicado a asegurar el correcto funcionamiento del software mediante pruebas unitarias y de integración.

---

## 📇 Modelo de Datos y Funcionalidades de la Aplicación

La aplicación gestiona una **Agenda de Contactos**, aunque yo en vez de poner contacto he puesto ususario, reconozco que es mi primer fallo, aunque es bastante facil de solucionar. La Aplicación es persistida en una base de datos **SQLite**. Cada contacto almacena la siguiente información:
- **Alias:** Identificador corto o apodo del contacto, unico en cada caso.
- **Nombre:** Nombre completo de la persona.
- **Teléfono:** Número de telefóno.
- **Email:** Correo electrónico asociado.

A nivel funcional, la lógica implementa un conjunto completo de operaciones optimizadas y dotadas de las siguientes capacidades:

* **Crear:** Permite dar de alta nuevos contactos en la base de datos.
* **Buscar por ID:** Permite localizar un registro de forma directa mediante su identificador único.
* **Buscar por Alias:** Facilita la localización rápida de contactos utilizando su alias.
* **Búsqueda paginada:** Permite consultar listados grandes de contactos divididos en páginas para mejorar el rendimiento.
* **Actualizar:** Permite modificar los datos de un contacto existente.
* **Borrar:** Permite eliminar registros de la agenda.
* **Sistema de Caché:** Integrado mediante memoria RAM para almacenar temporalmente los datos consultados y reducir el número de accesos directos a la base de datos.
* **Logging:** Registro detallado de la actividad de la aplicación tanto en consola como en ficheros de texto para la traza de errores y eventos.

---

## 🛠️ Tecnologías Utilizadas y Justificación

La elección de las librerías y herramientas responde al proceso de aprendizaje y consolidación de conceptos avanzados de .NET en el ámbito académico:

| Categoría | Tecnología / Paquete | Motivo de Elección |
| :--- | :--- | :--- |
| **Base de Datos** | `Microsoft.Data.Sqlite` | Librería oficial de .NET para la comunicación con SQLite. Es preferible este frente a otras alternativas, ya que nos permite tener un control mejor del framework utilizado en la conexión con la base de datos ligera. Ademas no es complicado de usar, sabiendo que no soy experto en este tipo de BD ligera|
| **Soporte SQLite** | `SQLitePCLRaw.bundle_e_sqlite3` | Paquete complementario necesario para empaquetar los binarios nativos de SQLite y asegurar la correcta ejecución multiplataforma de las consultas. Simplemente un paquete de complementación para que SQLite funcione correctamente. |
| **Logging** | `Serilog`<br>`Serilog.Sinks.Console`<br>`Serilog.Sinks.File` | Implementación de un sistema de registro estructurado. Se utiliza para trazar el flujo de la aplicación, volcando la información tanto en la consola durante la depuración como en archivos de texto persistentes para auditoría y control de errores. |
| **Caché** | `Microsoft.Extensions.Caching.Memory` | Implementación de caché en memoria RAM. Elegido para aprender a optimizar el rendimiento de la aplicación evitando accesos innecesarios a la BD mientras el programa se encuentra en ejecución. No se si esta elección ha sido la habitual que se ha usado en clase en años anteriores, pero al no haber apuntes de la realización del Caché, lo he buscado por internet. |

---

## 🧪 Estrategia de Testing (Pruebas Unitarias y de Integración)

Dado que nos encontramos en fase de aprendizaje de pruebas de software, hemos hecho varias pruabas de testing. El proyecto incluye tanto **pruebas unitarias** como **pruebas de integración** (aun siguen ampliándose y perfeccionándose):

* **NUnit:** Framework de pruebas elegido por su carácter intuitivo y su sistema claro de atributos y aserciones, ideal para el proceso de aprendizaje en testing.
* **Moq:** Librería de aislamiento para crear objetos simulados (*mocks*). Se emplea para simular el comportamiento del Repositorio y de la Caché, permitiendo aislar la lógica de negocio durante las pruebas unitarias.
* **FluentAssertions:** Utilizado para redactar las aserciones de las pruebas de forma fluida, descriptiva y mucho más legible para el programador.

---

## 🚀 Instalación y Ejecución

Para poner en marcha la aplicación, asegúrate de tener instalado el SDK de .NET y ejecuta los siguientes comandos en la raíz de la solución:

```bash
# Restaurar dependencias
dotnet restore

# Ejecutar los tests (unitarios y de integración)
dotnet test
