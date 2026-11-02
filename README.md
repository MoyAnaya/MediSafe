# MediSafe

Sistema web para la gestión segura de expedientes de pacientes y consultas médicas, desarrollado bajo principios de diseño de software seguro.

---

## Descripción del Proyecto

MediSafe es una solución web orientada a la administración de información clínica que implementa controles rigurosos de autenticación, autorización basada en roles (RBAC) y aislamiento de datos. Su diseño mitiga accesos no autorizados a registros médicos garantizando que cada profesional de la salud acceda estrictamente a los expedientes clínicos bajo su responsabilidad directa, complementado por un módulo administrativo independiente.

## Objetivos del Sistema

* **Aislamiento de información clínica:** Restringir la visualización y manipulación de historiales médicos al profesional de la salud asignado.
* **Control de acceso granular:** Aplicar autorización estricta del lado del servidor para diferenciar privilegios operativos entre perfiles administrativos y médicos.
* **Integridad de datos:** Validar entradas de usuario y parametrizar consultas para prevenir vulnerabilidades comunes en capas de persistencia y presentación.

---

## Funcionalidades Principales

### Módulo de Administración

* Gestión de ciclo de vida de cuentas médicas (creación, lectura, actualización y eliminación).
* Confirmación explícita para operaciones destructivas (baja de usuarios).
* Supervisión del directorio de médicos registrados.

### Módulo Médico

* Alta, consulta y edición de información de pacientes propios.
* Registro y actualización de consultas clínicas.
* Consulta cronológica del historial médico por paciente.
* Restricción automática de acceso a expedientes pertenecientes a otros profesionales.

### Mecanismos de Seguridad Implementados

* **Autenticación robusta:** Integración nativa con ASP.NET Core Identity.
* **Autorización en servidor:** Protección de endpoints y componentes mediante políticas y validación de claims.
* **Filtrado contextual de datos:** Consultas acotadas por identificador de usuario para evitar referencias directas inseguras a objetos (IDOR).
* **Validación de esquemas:** Comprobación estricta de modelos de entrada en frontend y backend.

---

## Arquitectura del Software

El sistema sigue una arquitectura por capas desacopladas orientada a la separación de responsabilidades y la seguridad en profundidad:

```text
[ Cliente / Navegador ]
        │
        ▼
[ Capa de Presentación (Blazor Web App / Bootstrap) ]
        │
        ▼
[ Capa de Seguridad (Autenticación y Autorización con ASP.NET Core Identity) ]
        │
        ▼
[ Capa de Lógica de Negocio y Reglas de Dominio ]
        │
        ▼
[ Capa de Acceso a Datos (Entity Framework Core) ]
        │
        ▼
[ Base de Datos Relacional (MySQL Server) ]

```

---

## Stack Tecnológico

| Componente | Tecnología | Propósito |
| --- | --- | --- |
| **Lenguaje** | C# | Lenguaje de desarrollo backend y componentes Blazor |
| **Plataforma** | .NET 10 | Framework base de ejecución y servicios |
| **Frontend** | Blazor Web App | Interfaz interactiva del lado del cliente/servidor |
| **Estilos** | Bootstrap | Diseño responsivo de vistas y componentes |
| **Seguridad** | ASP.NET Core Identity | Manejo de identidades, hashing de contraseñas y roles |
| **ORM** | Entity Framework Core | Mapeo objeto-relacional y ejecución de migraciones |
| **Base de Datos** | MySQL Server | Motor relacional de almacenamiento |
| **Herramienta BD** | MySQL Workbench | Modelado y administración de esquemas |
| **IDE** | Visual Studio 2026 | Entorno de desarrollo integrado |

---

## Matriz de Roles y Permisos

| Rol | Alcance de Permisos | Nivel de Aplicación |
| --- | --- | --- |
| **Administrador** | Creación, lectura, actualización y desactivación de cuentas médicas. Sin acceso a historiales clínicos. | Servidor (Directivas de rol e Identity) |
| **Médico** | Gestión completa de sus propios pacientes asignados e historiales de consulta. | Servidor (Filtros por `UserId` del médico) |

> **Nota:** Todas las restricciones de acceso se validan en el servidor y en la capa de persistencia; las restricciones visuales en la interfaz son únicamente auxiliares a la experiencia de usuario.

---

## Estructura del Repositorio

```text
MediSafe/
├── Components/
│   └── Pages/
│       ├── Administrador.razor       # Gestión de cuentas médicas
│       ├── Medico.razor              # Dashboard operativo del médico
│       ├── Pacientes.razor           # Catálogo y registro de pacientes
│       ├── Consultas.razor           # Formulario y control de consultas
│       ├── HistorialPaciente.razor   # Visualización cronológica de expedientes
│       └── Login.razor               # Formulario de autenticación
├── Data/
│   └── ApplicationDbContext.cs       # Contexto de base de datos y mapeos EF Core
├── Models/
│   ├── Paciente.cs                   # Entidad paciente
│   ├── Consulta.cs                   # Entidad consulta médica
│   ├── CrearPacienteModel.cs         # DTO de validación para alta de pacientes
│   └── CrearConsultaModel.cs         # DTO de validación para consultas
├── Program.cs                        # Configuración de servicios, pipelines e Identity
└── README.md

```

---

## Requisitos del Entorno

* SDK de .NET 10.
* Servidor MySQL Server 8.0 o superior en ejecución.
* Visual Studio 2026 (con carga de trabajo *ASP.NET y desarrollo web*) o Visual Studio Code con extensiones de C#.
* Cliente Git.

---

## Guía de Instalación y Configuración

### 1. Clonar el repositorio

```bash
git clone https://github.com/usuario/medisafe.git
cd medisafe

```

### 2. Configurar el gestor de base de datos

Inicie sesión en MySQL y genere la base de datos vacía:

```sql
CREATE DATABASE medisafe_db CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

```

### 3. Configurar variables de entorno y secretos

Para evitar exponer credenciales en archivos rastreados por Git, use el Administrador de Secretos de .NET:

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=medisafe_db;User=SU_USUARIO;Password=SU_CONTRASENA;"

```

### 4. Aplicar migraciones

Cree las tablas del esquema base y las tablas del subsistema de Identity:

```bash
dotnet ef database update

```

*(En caso de no tener la herramienta CLI de EF Core instalada globalmente, ejecute: `dotnet tool install --global dotnet-ef`)*

### 5. Compilar y ejecutar

```bash
dotnet restore
dotnet build
dotnet run

```

La consola indicará el puerto local asignado (por ejemplo, `https://localhost:7001`).

---

## Casos de Prueba y Verificación de Seguridad

* **Control de sesiones:** Comprobar caducidad de tokens y redirección de rutas protegidas tras el cierre de sesión.
* **Aislamiento horizontal:** Verificar que una petición autenticada como *Médico A* devuelva un código HTTP `403 Forbidden` o `404 Not Found` al intentar acceder directamente al ID de un paciente registrado por el *Médico B*.
* **Prevención de escalamiento vertical:** Intentar acceder a rutas administrativas desde una sesión con rol de *Médico*.
* **Inyección de datos e integridad:** Enviar payloads que contengan caracteres especiales, campos en blanco y tipos no válidos para comprobar el rechazo por validación de modelo (`Data Annotations`).
* **Manejo de excepciones:** Validar que los errores de ejecución 500 no expongan stack traces, nombres de tablas ni cadenas de conexión al usuario final.

---

## Consideraciones de Seguridad y Estado

* El proyecto se encuentra en fase de desarrollo activo como producto mínimo viable (MVP).
* No debe desplegarse en entornos productivos con datos reales de pacientes sin antes completar auditorías de cifrado en reposo, firmas criptográficas y cumplimiento de normativas de salud digital.
* Queda estrictamente prohibido commitear contraseñas, secretos de API o certificados dentro del control de versiones.

---

## Contexto del Proyecto

Desarrollado como proyecto práctico para la materia de **Desarrollo de Software Seguro**, con el propósito de integrar técnicas de mitigación de vulnerabilidades, arquitectura de código seguro y modelos de autorización en plataformas .NET.
