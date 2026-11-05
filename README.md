# MediSafe

Sistema web para la gestión de pacientes y consultas médicas, desarrollado bajo principios de desarrollo de software seguro.

---

## Descripción del Proyecto

MediSafe es una aplicación web orientada a la administración de información de pacientes y expedientes clínicos. El sistema utiliza autenticación mediante ASP.NET Core Identity y autorización basada en roles (RBAC) para separar las responsabilidades y los permisos de sus tres tipos de usuarios: Administrador, Médico y Recepcionista.

Su objetivo es facilitar el registro y seguimiento de pacientes, así como la administración de consultas médicas, manteniendo restricciones de acceso de acuerdo con el rol y la responsabilidad de cada usuario.

El sistema permite que los médicos consulten y administren únicamente los pacientes asociados a su cuenta, mientras que el personal de recepción puede registrar pacientes, asignarles un médico responsable y consultar información personal y administrativa. El administrador se encarga de gestionar las cuentas de médicos y recepcionistas, sin necesidad de acceder a la información clínica.

## Objetivos del Sistema

- **Separación de responsabilidades:** Diferenciar las funciones disponibles para administradores, médicos y recepcionistas.
- **Protección de información clínica:** Restringir el acceso a historiales médicos y consultas al médico responsable del paciente.
- **Control de acceso basado en roles:** Aplicar autorización para limitar las funcionalidades disponibles según el tipo de usuario.
- **Gestión de pacientes:** Permitir el registro y consulta de pacientes, así como la asignación de un médico responsable.
- **Integridad de datos:** Validar las entradas de usuario y mantener relaciones consistentes entre pacientes, médicos y consultas.

---

## Funcionalidades Principales

### Módulo de Administración

Permite gestionar las cuentas de los usuarios que forman parte del personal del sistema.

- Registro de cuentas de médicos.
- Consulta del directorio de médicos registrados.
- Edición de datos de los médicos.
- Eliminación de cuentas de médicos.
- Registro de cuentas de recepcionistas.
- Consulta del directorio de recepcionistas registrados.
- Edición de datos de los recepcionistas.
- Eliminación de cuentas de recepcionistas.
- Confirmación antes de realizar operaciones de eliminación.

El administrador se encarga de la gestión de usuarios y no tiene acceso a los historiales clínicos de los pacientes.

### Módulo Médico

Permite al personal médico administrar los pacientes que tiene asociados.

- Registro de pacientes.
- Asociación automática del paciente con el médico que inició sesión.
- Consulta del listado de pacientes propios.
- Edición de información personal de sus pacientes.
- Búsqueda de pacientes por nombre, teléfono o correo electrónico.
- Registro de consultas médicas.
- Consulta cronológica del historial clínico.
- Visualización de motivos, diagnósticos, tratamientos y observaciones.
- Restricción de acceso a pacientes asociados a otros médicos.

### Módulo de Recepción

Permite al personal de recepción realizar actividades administrativas relacionadas con el registro y seguimiento de pacientes.

- Registro de pacientes.
- Selección del médico responsable durante el registro.
- Consulta del listado general de pacientes.
- Visualización de datos personales de los pacientes.
- Consulta del médico responsable de cada paciente.
- Búsqueda y localización de pacientes.

El rol de Recepcionista no tiene permisos para consultar o modificar diagnósticos, tratamientos, observaciones ni historiales clínicos. Tampoco puede administrar cuentas de médicos o de otros recepcionistas.

---

## Mecanismos de Seguridad

MediSafe incorpora mecanismos de seguridad para controlar la autenticación, autorización y acceso a la información.

- **Autenticación:** Integración con ASP.NET Core Identity para administrar cuentas de usuario e inicio de sesión.
- **Autorización basada en roles (RBAC):** Separación de permisos entre Administrador, Médico y Recepcionista.
- **Protección de componentes:** Restricción del acceso a las páginas según el rol del usuario autenticado.
- **Filtrado de información:** Los médicos consultan pacientes mediante el identificador de su cuenta, evitando que accedan a pacientes asociados a otros profesionales.
- **Asignación de médico responsable:** Los pacientes registrados por médicos quedan asociados a la cuenta autenticada; los registrados por recepción se asocian al médico seleccionado.
- **Validación de formularios:** Uso de modelos y Data Annotations para validar los datos ingresados.
- **Separación de información clínica:** El módulo de recepción se limita a información personal y administrativa, sin acceso a historiales clínicos.

> **Nota:** La seguridad del sistema debe verificarse mediante pruebas funcionales y de seguridad. La separación visual de opciones no sustituye las validaciones de autorización y acceso a datos en el servidor.

---

## Arquitectura del Software

El sistema utiliza Blazor Web App y Entity Framework Core para organizar la presentación, los servicios de autenticación y autorización, la lógica de la aplicación y el acceso a la base de datos.

```text
             [ Cliente / Navegador ]
                       |
                       v
       [ Capa de Presentación ]
          Blazor Web App
              Bootstrap
                       |
                       v
       [ Autenticación y Autorización ]
          ASP.NET Core Identity
              Roles de usuario
                       |
                       v
       [ Lógica de la Aplicación ]
       Reglas de acceso y validación
                       |
                       v
       [ Acceso a Datos ]
          Entity Framework Core
                       |
                       v
       [ Base de Datos Relacional ]
                 MySQL
```

La aplicación contempla tres perfiles de usuario, cada uno con funcionalidades y restricciones específicas.

---

## Stack Tecnológico

| Componente | Tecnología | Propósito |
|---|---|---|
| Lenguaje | C# | Desarrollo de la aplicación y componentes Blazor |
| Plataforma | .NET 10 | Framework de ejecución |
| Frontend | Blazor Web App | Construcción de las interfaces web |
| Estilos | Bootstrap | Diseño y presentación de componentes |
| Seguridad | ASP.NET Core Identity | Autenticación, gestión de usuarios y roles |
| ORM | Entity Framework Core | Acceso a datos y administración de migraciones |
| Base de datos | MySQL Server | Almacenamiento de información |
| Herramienta de BD | MySQL Workbench | Administración de la base de datos |
| IDE | Visual Studio 2026 | Entorno de desarrollo |

---

## Matriz de Roles y Permisos

| Funcionalidad | Administrador | Médico | Recepcionista |
|---|:---:|:---:|:---:|
| Iniciar sesión | Sí | Sí | Sí |
| Registrar médicos | Sí | No | No |
| Consultar médicos | Sí | No | No |
| Editar médicos | Sí | No | No |
| Eliminar médicos | Sí | No | No |
| Registrar recepcionistas | Sí | No | No |
| Consultar recepcionistas | Sí | No | No |
| Editar recepcionistas | Sí | No | No |
| Eliminar recepcionistas | Sí | No | No |
| Registrar pacientes | No | Sí | Sí |
| Seleccionar médico responsable | No | No | Sí |
| Consultar pacientes propios | No | Sí | No |
| Consultar listado general de pacientes | No | No | Sí |
| Editar datos personales de pacientes | No | Sí | No |
| Registrar consultas médicas | No | Sí | No |
| Consultar historiales clínicos | No | Sí | No |
| Consultar diagnósticos y tratamientos | No | Sí | No |

**Consideraciones:**

- El médico solamente puede acceder a los pacientes asociados a su cuenta.
- El médico no necesita seleccionar manualmente su identificador al registrar un paciente, ya que se obtiene del usuario autenticado.
- La recepcionista debe seleccionar un médico registrado al dar de alta un paciente.
- El administrador gestiona las cuentas del personal, pero no administra expedientes clínicos.
- La recepcionista puede consultar datos personales y administrativos, pero no información clínica.

---

## Estructura del Repositorio

La siguiente estructura representa los principales archivos y componentes del proyecto. Puede ajustarse conforme se incorporen nuevas funcionalidades.

```text
MediSafe/
├── Components/
│   └── Pages/
│       ├── Administrador.razor
│       │   # Gestión de cuentas de médicos y recepcionistas
│       ├── Medico.razor
│       │   # Panel principal del médico
│       ├── Recepcion.razor
│       │   # Panel y funciones de recepción
│       ├── Pacientes.razor
│       │   # Registro y listado de pacientes del médico
│       ├── Consultas.razor
│       │   # Consulta y búsqueda de pacientes
│       ├── HistorialPaciente.razor
│       │   # Historial de consultas médicas
│       └── Login.razor
│           # Interfaz de inicio de sesión
│
├── Data/
│   └── ApplicationDbContext.cs
│       # Contexto de base de datos y entidades
│
├── Models/
│   ├── Paciente.cs
│   │   # Entidad de paciente
│   ├── Consulta.cs
│   │   # Entidad de consulta médica
│   ├── CrearPacienteModel.cs
│   │   # Modelo de validación para pacientes
│   ├── CrearConsultaModel.cs
│   │   # Modelo de validación para consultas
│   └── CrearMedicoModel.cs
│       # Modelo para registrar cuentas del personal
│
├── Program.cs
│   # Configuración de servicios, Identity y roles
│
└── README.md
```

---

## Requisitos del Entorno

- SDK de .NET 10.
- MySQL Server 8.0 o superior.
- Visual Studio 2026 con la carga de trabajo de ASP.NET y desarrollo web.
- MySQL Workbench.
- Cliente Git.
- Herramienta de línea de comandos de Entity Framework Core (`dotnet-ef`).

---

## Guía de Instalación y Configuración

### 1. Clonar el repositorio

```bash
git clone https://github.com/usuario/medisafe.git
cd medisafe
```

### 2. Crear la base de datos

Iniciar sesión en MySQL Workbench y ejecutar:

```sql
CREATE DATABASE medisafe_db
CHARACTER SET utf8mb4
COLLATE utf8mb4_unicode_ci;
```

### 3. Configurar los secretos de conexión

Para evitar exponer credenciales dentro del repositorio, se utiliza el Administrador de Secretos de .NET.

Inicializar User Secrets:

```bash
dotnet user-secrets init
```

Configurar la conexión a MySQL:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=medisafe_db;User=SU_USUARIO;Password=SU_CONTRASENA;"
```

Sustituir `SU_USUARIO` y `SU_CONTRASENA` por las credenciales locales de MySQL.

### 4. Aplicar migraciones

Ejecutar:

```bash
dotnet ef database update
```

Este comando aplica las migraciones disponibles y crea las tablas necesarias para la aplicación y ASP.NET Core Identity.

Si no se encuentra instalada la herramienta de Entity Framework Core, ejecutar:

```bash
dotnet tool install --global dotnet-ef
```

### 5. Restaurar dependencias, compilar y ejecutar

```bash
dotnet restore
dotnet build
dotnet run
```

La consola mostrará la dirección local para acceder a la aplicación, por ejemplo:

```text
https://localhost:7001
```

El puerto puede variar dependiendo de la configuración del proyecto.

---

## Casos de Prueba y Verificación

Las siguientes pruebas permiten verificar el comportamiento de las funcionalidades y los permisos de los tres roles.

| ID | Funcionalidad | Prueba real | Resultado esperado |
|---|---|---|---|
| P-01 | Inicio de sesión | Ingresar las credenciales correctas de un administrador, médico o recepcionista. | El sistema permite el acceso y dirige al usuario al panel correspondiente a su rol. |
| P-02 | Control de acceso administrativo | Iniciar sesión como médico e intentar acceder al panel de administración. | El sistema impide el acceso a usuarios sin rol de Administrador. |
| P-03 | Registro de médicos | Registrar una cuenta desde el panel de administración. | El sistema crea la cuenta y le asigna el rol Médico. |
| P-04 | Registro de recepcionistas | Registrar una cuenta desde el panel de administración. | El sistema crea la cuenta y le asigna el rol Recepcionista. |
| P-05 | Registro de pacientes por médico | Iniciar sesión como médico y registrar un paciente. | El paciente queda asociado automáticamente al médico autenticado. |
| P-06 | Registro de pacientes por recepción | Registrar un paciente desde recepción y seleccionar un médico responsable. | El paciente queda asociado al médico seleccionado. |
| P-07 | Listado general de pacientes | Acceder al listado de pacientes desde recepción. | Se muestran los pacientes registrados y sus datos administrativos, incluyendo el médico responsable. |
| P-08 | Separación de información clínica | Iniciar sesión como recepcionista e intentar acceder al historial clínico. | El sistema impide el acceso a la información clínica. |
| P-09 | Historial clínico | Iniciar sesión como médico y consultar el historial de un paciente propio. | Se muestran las consultas, diagnósticos, tratamientos y observaciones correspondientes. |
| P-10 | Aislamiento de pacientes | Intentar consultar el historial de un paciente asociado a otro médico. | El sistema impide consultar información de pacientes que no pertenecen al médico autenticado. |
| P-11 | Edición de pacientes | Modificar los datos personales de un paciente propio desde el panel médico. | El sistema guarda los cambios correctamente. |
| P-12 | Registro de consultas | Registrar una consulta médica para un paciente propio. | El sistema guarda la consulta asociada al paciente y al médico autenticado. |
| P-13 | Edición de médicos | Modificar los datos de una cuenta médica desde el panel administrativo. | El sistema actualiza la información de la cuenta. |
| P-14 | Eliminación de médicos | Confirmar la eliminación de una cuenta médica. | El sistema ejecuta la operación de eliminación y aplica el comportamiento definido para sus pacientes y consultas asociados. |
| P-15 | Edición de recepcionistas | Modificar los datos de una cuenta de recepción desde el panel administrativo. | El sistema actualiza la información de la cuenta. |
| P-16 | Eliminación de recepcionistas | Confirmar la eliminación de una cuenta de recepción. | El sistema elimina la cuenta sin eliminar los pacientes ni las consultas asociados a otros médicos. |

**Importante:** la columna de resultado esperado describe el comportamiento que se debe comprobar. Para documentar las pruebas ejecutadas, se recomienda agregar una columna de resultado obtenido y registrar lo que realmente sucedió durante cada prueba.

---

## Consideraciones de Seguridad y Estado del Proyecto

- MediSafe se encuentra en desarrollo como producto mínimo viable (MVP) académico.
- El sistema implementa autenticación y separación de permisos mediante ASP.NET Core Identity.
- La información clínica está destinada al acceso de los médicos responsables de los pacientes.
- El módulo de recepción está limitado a actividades administrativas y datos personales.
- El administrador tiene funciones de gestión de cuentas de personal.
- Antes de utilizar el sistema en un entorno productivo con información médica real, será necesario realizar pruebas de seguridad adicionales, revisar el manejo de datos sensibles, evaluar el cifrado en tránsito y en reposo, implementar mecanismos de auditoría y revisar el cumplimiento de la normativa aplicable.
- No se deben almacenar contraseñas, cadenas de conexión ni otros secretos directamente en archivos versionados mediante Git.

---

## Contexto del Proyecto

MediSafe es desarrollado como proyecto práctico para la materia de **Desarrollo de Software Seguro**, con el propósito de aplicar conceptos de autenticación, autorización, validación de datos, separación de responsabilidades y protección de información en una aplicación web construida con tecnologías .NET.

El proyecto busca demostrar cómo una aplicación puede organizar sus funcionalidades mediante roles y establecer restricciones de acceso de acuerdo con las responsabilidades de cada usuario.
