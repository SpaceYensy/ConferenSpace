# 🚀 Guía Rápida de Inicio - ConferenSpace

## ⚡ Inicio Rápido (5 minutos)

### 1. Restaurar dependencias
```bash
dotnet restore
```

### 2. Compilar el proyecto
```bash
dotnet build
```

### 3. Crear migraciones
```bash
cd ConferenSpace.API
dotnet ef migrations add InitialCreate --project ../ConferenSpace.Infrastructure
```

### 4. Aplicar migraciones (crear BD)
```bash
dotnet ef database update --project ../ConferenSpace.Infrastructure
```

### 5. Ejecutar la API
```bash
dotnet run
```

### 6. Acceder a Swagger
```
https://localhost:7123/swagger
```

---

## 📋 Verificar instalación

### Comprobar .NET
```bash
dotnet --version
```
Debe ser versión 10.0 o superior.

### Comprobar SQL Server LocalDB
```bash
sqllocaldb info
sqllocaldb start MSSQLLocalDB
```

### Probar conexión
```bash
dotnet ef dbcontext info --project ConferenSpace.API
```

---

## 🧪 Pruebas Manuales en Swagger

### 1. Crear un Solicitante
**POST** `/api/solicitantes`
```json
{
  "nombreCompleto": "Juan Pérez",
  "telefono": "+34 912 34 56 78",
  "correo": "juan.perez@empresa.com",
  "departamento": "Ventas",
  "numeroIdentificacion": "12345678A"
}
```

### 2. Crear un Salón
**POST** `/api/salones`
```json
{
  "nombre": "Sala A",
  "ubicacion": "Piso 3",
  "capacidadMaxima": 50,
  "serviciosIntegrados": "Proyector, Pizarra digital"
}
```

### 3. Crear un Recurso
**POST** `/api/recursos`
```json
{
  "nombre": "Proyector",
  "descripcion": "Proyector para presentaciones",
  "cantidadTotal": 5,
  "cantidadDisponible": 5,
  "costoUnitario": 200
}
```

### 4. Crear una Reserva
**POST** `/api/reservas`
```json
{
  "solicitanteId": 1,
  "salonId": 1,
  "fecha": "2024-02-20",
  "horaInicio": "14:00:00",
  "horaFin": "16:00:00",
  "proposito": "Reunión de junta",
  "cantidadAsistentes": 25,
  "notas": "Confirmar asistencia",
  "recursos": [
    {
      "recursoId": 1,
      "cantidadSolicitada": 1
    }
  ]
}
```

### 5. Verificar Disponibilidad
**GET** `/api/salones/disponibles?fecha=2024-02-21&horaInicio=10:00:00&horaFin=12:00:00&capacidadMinima=20`

---

## 📂 Estructura del Proyecto

```
ConferenSpace/
├── ConferenSpace.Domain/           → Entidades
├── ConferenSpace.Application/      → Servicios y DTOs
├── ConferenSpace.Infrastructure/   → Acceso a datos
├── ConferenSpace.API/              → REST API
└── ConferenSpace.UI/               → Interfaz (Por hacer)
```

---

## 🔐 Configuración de Base de Datos

En `ConferenSpace.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ConferenSpaceDb;Trusted_Connection=true;"
  }
}
```

---

## 🐛 Solución de Problemas

### Error: "Connection refused"
- Verificar que SQL Server LocalDB esté corriendo: `sqllocaldb start MSSQLLocalDB`

### Error: "Migrations history table could not be created"
- Borrar la base de datos: `dotnet ef database drop`
- Recrear: `dotnet ef database update`

### Error: "Cannot restore packages"
- Limpiar cache NuGet: `dotnet nuget locals all --clear`
- Restaurar nuevamente: `dotnet restore`

### La API no inicia
- Asegurar que el puerto 7123 no esté en uso
- Revisar los logs en la consola
- Verificar la configuración de appsettings.json

---

## 📊 Estado de Funcionalidades

| Funcionalidad | Estado |
|--------------|--------|
| Gestión de Salones | ✅ Completo |
| Gestión de Solicitantes | ✅ Completo |
| Gestión de Recursos | ✅ Completo |
| Gestión de Reservas | ✅ Completo |
| Validación de Disponibilidad | ✅ Completo |
| Calendario de Ocupación | ✅ Completo |
| REST API | ✅ Completo |
| Swagger/OpenAPI | ✅ Completo |
| UI (Blazor) | 📋 Pendiente |
| Autenticación JWT | 📋 Pendiente |
| Tests Unitarios | 📋 Pendiente |

---

## 📚 Documentación Disponible

- **README.md** - Descripción general del proyecto
- **API_DOCUMENTATION.md** - Documentación completa de endpoints
- **MIGRACIONES.md** - Guía para crear y ejecutar migraciones
- **PROJECT_STATUS.md** - Estado y resumen del proyecto

---

## 🎯 Próximos Pasos Recomendados

1. ✅ Crear migraciones y base de datos
2. ✅ Ejecutar la API
3. ✅ Probar endpoints en Swagger
4. ⬜ Crear componentes UI en Blazor
5. ⬜ Implementar autenticación JWT
6. ⬜ Agregar tests unitarios
7. ⬜ Configurar deployment

---

## 💡 Tips Útiles

### Ver las URLs de la API
```bash
cd ConferenSpace.API
dotnet run --urls "https://localhost:7123"
```

### Resetear la base de datos
```bash
dotnet ef database drop -f --project ../ConferenSpace.Infrastructure
dotnet ef database update --project ../ConferenSpace.Infrastructure
```

### Limpiar y reconstruir
```bash
dotnet clean
dotnet build
```

### Ver logs en consola
Los logs se mostrarán automáticamente en la consola cuando ejecute `dotnet run`.
Aumente el nivel de detalle en `appsettings.Development.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.EntityFrameworkCore": "Debug"
    }
  }
}
```

---

## 🆘 ¿Necesita ayuda?

1. Revise los archivos de documentación
2. Verifique que todas las dependencias estén instaladas
3. Asegúrese de que SQL Server LocalDB esté disponible
4. Consulte la sección de Solución de Problemas

---

**¡Disfrute usando ConferenSpace! 🎉**

*Para más información, vea los otros archivos .md en la raíz del proyecto.*
