# ConferenSpace - Gestor de Reservas para Salón de Conferencias

## Descripción

**ConferenSpace** es una aplicación diseñada para resolver el caos logístico que surge al gestionar múltiples espacios de reunión en entornos corporativos, educativos o de coworking. Su objetivo es centralizar el proceso de apartar salones de conferencias, eliminando los conflictos de doble reserva y optimizando el uso de cada espacio disponible.

## Arquitectura

El proyecto está estructurado en 5 capas siguiendo los principios de Clean Architecture:

```
ConferenSpace.sln
├── ConferenSpace.Domain           (Entidades y Lógica de Negocio)
├── ConferenSpace.Application      (Casos de Uso y Servicios)
├── ConferenSpace.Infrastructure   (Acceso a Datos)
├── ConferenSpace.API              (Endpoints REST)
└── ConferenSpace.UI               (Interfaz de Usuario - Blazor)
```

### Capas del Proyecto

#### 1. **Domain** - Entidades
Contiene las clases de dominio que representan los conceptos centrales:
- `Salon.cs` - Espacios físicos con capacidad, ubicación y servicios
- `Solicitante.cs` - Personas que realizan las reservas
- `Recurso.cs` - Elementos complementarios (equipos, mobiliario, catering)
- `Reserva.cs` - Núcleo del sistema, une solicitante, salón, fecha y recursos
- `ReservaRecurso.cs` - Relación muchos-a-muchos entre reserva y recursos

#### 2. **Application** - Servicios y DTOs
- **Contracts**: Interfaces de servicios (`ISalonService`, `IReservaService`, etc.)
- **Services**: Implementación de la lógica de negocio
- **DTOs**: Data Transfer Objects para comunicación con clientes
- **MappingProfiles**: Configuración de AutoMapper

#### 3. **Infrastructure** - Acceso a Datos
- **Contracts**: Interfaces de repositorios
- **Repositories**: Implementación de acceso a datos
- **Data**: `ConferenSpaceDbContext` (DbContext de Entity Framework)

#### 4. **API** - REST Endpoints
Controladores REST para cada entidad:
- `SalonesController` - Gestión de salones
- `SolicitantesController` - Gestión de solicitantes
- `RecursosController` - Gestión de recursos
- `ReservasController` - Gestión de reservas

#### 5. **UI** - Interfaz de Usuario
Interfaz Blazor para interacción del usuario (a desarrollar).

## Características Principales

### 1. **Gestión de Solicitantes**
- Crear, editar y eliminar solicitantes
- Buscar por nombre, departamento o correo
- Desactivar solicitantes sin eliminar historial

### 2. **Administración de Salones y Recursos**
- Registrar salones con capacidad y servicios integrados
- Poner salones fuera de servicio temporalmente
- Gestionar recursos adicionales con inventario
- Controlar cantidades disponibles en tiempo real

### 3. **Gestión de Reservas**
- Crear reservas con validación automática de disponibilidad
- Agregar múltiples recursos a una reserva
- Editar reservas existentes
- Cancelar reservas, liberando recursos
- Ver calendario de ocupación por día

### 4. **Validación de Disponibilidad**
- Evitar doble booking automáticamente
- Validar capacidad máxima del salón
- Verificar disponibilidad de recursos solicitados
- Control de inventario en tiempo real

## Requisitos Técnicos

- **.NET 10**
- **SQL Server** (LocalDB o Superior)
- **Entity Framework Core** 10.0.5
- **AutoMapper** 13.0.1
- **OpenAPI**

## Instalación y Configuración

### 1. Clonar o descargar el proyecto

```bash
git clone <url-del-repositorio>
cd ConferenSpace
```

### 2. Restaurar dependencias

```bash
dotnet restore
```

### 3. Configurar la cadena de conexión

En `ConferenSpace.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ConferenSpaceDb;Trusted_Connection=true;"
  }
}
```

### 4. Crear y ejecutar migraciones

```bash
cd ConferenSpace.API
dotnet ef migrations add InitialCreate -p ../ConferenSpace.Infrastructure -s .
dotnet ef database update
```

### 5. Ejecutar la API

```bash
dotnet run
```

## Endpoints de la API

### Salones
```
GET    /api/salones                          - Obtener todos
GET    /api/salones/{id}                     - Obtener por ID
POST   /api/salones                          - Crear nuevo
PUT    /api/salones/{id}                     - Actualizar
DELETE /api/salones/{id}                     - Eliminar
PATCH  /api/salones/{id}/fuera-de-servicio   - Poner fuera de servicio
PATCH  /api/salones/{id}/reactivar           - Reactivar
GET    /api/salones/disponibles              - Obtener disponibles
```

### Solicitantes
```
GET    /api/solicitantes                     - Obtener todos
GET    /api/solicitantes/{id}                - Obtener por ID
POST   /api/solicitantes                     - Crear nuevo
PUT    /api/solicitantes/{id}                - Actualizar
DELETE /api/solicitantes/{id}                - Eliminar
PATCH  /api/solicitantes/{id}/desactivar     - Desactivar
GET    /api/solicitantes/buscar/nombre/{nombre}
GET    /api/solicitantes/buscar/departamento/{departamento}
GET    /api/solicitantes/buscar/correo
```

### Recursos
```
GET    /api/recursos                         - Obtener todos
GET    /api/recursos/{id}                    - Obtener por ID
POST   /api/recursos                         - Crear nuevo
PUT    /api/recursos/{id}                    - Actualizar
DELETE /api/recursos/{id}                    - Eliminar
GET    /api/recursos/{id}/disponibilidad     - Cantidad disponible
GET    /api/recursos/disponibles             - Obtener disponibles
PATCH  /api/recursos/{id}/desactivar         - Desactivar
PATCH  /api/recursos/{id}/reactivar          - Reactivar
```

### Reservas
```
GET    /api/reservas                         - Obtener todas
GET    /api/reservas/{id}                    - Obtener por ID
POST   /api/reservas                         - Crear nueva
PUT    /api/reservas/{id}                    - Actualizar
PATCH  /api/reservas/{id}/cancelar           - Cancelar
GET    /api/reservas/solicitante/{solicitanteId}
GET    /api/reservas/salon/{salonId}         - Por salón y fecha
GET    /api/reservas/salon/{salonId}/disponibilidad
POST   /api/reservas/validar-disponibilidad
```

## 📊 Estructura de Datos

### Salon
```csharp
{
  "id": 1,
  "nombre": "Sala A",
  "ubicacion": "Piso 3, Ala Oeste",
  "capacidadMaxima": 50,
  "serviciosIntegrados": "Proyector, Pizarra digital",
  "estaActivo": true,
  "fechaCreacion": "2024-01-15T10:30:00Z"
}
```

### Solicitante
```csharp
{
  "id": 1,
  "nombreCompleto": "Juan Pérez",
  "telefono": "+34 912 34 56 78",
  "correo": "juan.perez@empresa.com",
  "departamento": "Ventas",
  "numeroIdentificacion": "12345678A",
  "estaActivo": true,
  "fechaCreacion": "2024-01-10T08:00:00Z"
}
```

### Recurso
```csharp
{
  "id": 1,
  "nombre": "Proyector portátil",
  "descripcion": "Proyector para presentaciones",
  "cantidadTotal": 5,
  "cantidadDisponible": 3,
  "costoUnitario": 200.00,
  "estaActivo": true,
  "fechaCreacion": "2024-01-05T09:15:00Z"
}
```

### Reserva
```csharp
{
  "id": 1,
  "solicitanteId": 1,
  "salonId": 1,
  "fecha": "2024-02-20",
  "horaInicio": "14:00:00",
  "horaFin": "16:00:00",
  "proposito": "Reunión de junta directiva",
  "cantidadAsistentes": 25,
  "estado": 1,
  "notas": "Confirmar catering con 24 horas de anticipación",
  "recursos": [
    {
      "recursoId": 1,
      "cantidadSolicitada": 2
    }
  ],
  "fechaCreacion": "2024-01-15T10:30:00Z"
}
```
