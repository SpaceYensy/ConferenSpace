# ConferenSpace - Gestor de Reservas para Salón de Conferencias

## 📋 Descripción

**ConferenSpace** es una aplicación diseñada para resolver el caos logístico que surge al gestionar múltiples espacios de reunión en entornos corporativos, educativos o de coworking. Su objetivo es centralizar el proceso de apartar salones de conferencias, eliminando los conflictos de doble reserva y optimizando el uso de cada espacio disponible.

## 🏗️ Arquitectura

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

## 🎯 Características Principales

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

## 🔧 Requisitos Técnicos

- **.NET 10**
- **SQL Server** (LocalDB o Superior)
- **Entity Framework Core** 10.0.5
- **AutoMapper** 13.0.1
- **Swagger/OpenAPI**

## 📦 Instalación y Configuración

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

La API estará disponible en: `https://localhost:7123`

## 🚀 Endpoints de la API

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

## 🔒 Validaciones Implementadas

1. **Validación de Disponibilidad del Salón**
   - No permite reservar si hay conflicto de horarios
   - Valida la capacidad máxima vs asistentes

2. **Validación de Recursos**
   - Verifica disponibilidad de cantidad requerida
   - Considera reservas en el mismo horario

3. **Validaciones de Entidades**
   - No permite eliminar solicitantes con reservas
   - No permite eliminar salones con reservas
   - No permite desactivar solicitantes con reservas activas

4. **Integridad Referencial**
   - Las relaciones se mantienen automáticamente
   - Cascada de eliminación en ReservaRecurso

## 🗄️ Base de Datos

### Tablas principales:
- `Salones` - Espacios de conferencias
- `Solicitantes` - Personas que reservan
- `Recursos` - Elementos adicionales disponibles
- `Reservas` - Reservaciones realizadas
- `ReservaRecursos` - Relación muchos-a-muchos

### Índices para optimización:
- Índice en `(SalonId, Fecha)` de Reservas
- Índice en `SolicitanteId` de Reservas
- Índice en `Estado` de Reservas
- Índice único en `(ReservaId, RecursoId)` de ReservaRecursos

## 📝 Ejemplos de Uso

### Crear una Reserva
```http
POST /api/reservas
Content-Type: application/json

{
  "solicitanteId": 1,
  "salonId": 1,
  "fecha": "2024-02-20",
  "horaInicio": "14:00:00",
  "horaFin": "16:00:00",
  "proposito": "Reunión de junta",
  "cantidadAsistentes": 25,
  "notas": "Confirmar catering",
  "recursos": [
    {
      "recursoId": 1,
      "cantidadSolicitada": 2
    }
  ]
}
```

### Obtener Disponibilidad
```http
GET /api/salones/disponibles?fecha=2024-02-20&horaInicio=14:00:00&horaFin=16:00:00&capacidadMinima=20
```

### Obtener Calendario de Salón
```http
GET /api/reservas/salon/1/disponibilidad?fechaInicio=2024-02-01&fechaFin=2024-02-29
```

## 🧪 Testing

Para ejecutar pruebas unitarias (cuando se implementen):

```bash
dotnet test
```

## 📚 Documentación

La API incluye Swagger/OpenAPI. Acceda a:

```
https://localhost:7123/swagger
```

## 🔐 Seguridad

Recomendaciones para producción:
- Implementar autenticación JWT
- Agregar autorización basada en roles
- Validar HTTPS
- Implementar rate limiting
- Agregar CORS si es necesario

## 🐛 Resolución de Problemas

### Error: "Database update failed"
Verifique que SQL Server esté corriendo y la conexión sea correcta.

### Error: "The LINQ expression could not be translated"
Asegúrese de que las consultas LINQ sean traducibles a SQL.

### Error de migraciones
```bash
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## 📝 Notas de Desarrollo

- El proyecto usa Entity Framework Core para acceso a datos
- AutoMapper se usa para mapeo entre DTOs y entidades
- Los servicios implementan la lógica de negocio
- Los repositorios manejan el acceso a datos
- Los controladores exponen los endpoints REST

## 🤝 Contribución

Para contribuir al proyecto:
1. Fork el repositorio
2. Crea una rama de feature (`git checkout -b feature/AmazingFeature`)
3. Commit cambios (`git commit -m 'Add AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Open Pull Request

## 📄 Licencia

Este proyecto está bajo licencia MIT.

## ✉️ Contacto

Para preguntas o sugerencias, contactar al equipo de desarrollo.

---

**Versión**: 1.0.0  
**Fecha de creación**: 2024  
**Última actualización**: 2024
