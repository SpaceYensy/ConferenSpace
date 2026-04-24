# Instrucciones para Crear y Ejecutar Migraciones

## Prerequisitos
- .NET 10 SDK instalado
- SQL Server LocalDB o SQL Server (community edition, express, etc.)

## Pasos

### 1. Asegúrate que estás en la carpeta raíz del proyecto

```bash
cd C:\Users\yensy\OneDrive\Escritorio\New folder\App\ConferenSpace\
```

### 2. Crear la primera migración

```bash
dotnet ef migrations add InitialCreate --project ConferenSpace.Infrastructure --startup-project ConferenSpace.API
```

O si estás en la carpeta del proyecto API:

```bash
cd ConferenSpace.API
dotnet ef migrations add InitialCreate --project ../ConferenSpace.Infrastructure
```

### 3. Actualizar la base de datos

```bash
cd ConferenSpace.API
dotnet ef database update --project ../ConferenSpace.Infrastructure
```

### 4. Compilar la solución

```bash
dotnet build
```

### 5. Ejecutar la API

```bash
cd ConferenSpace.API
dotnet run
```

La API estará disponible en:
- **HTTPS**: `https://localhost:7123`
- **HTTP**: `http://localhost:5123`
- **Swagger**: `https://localhost:7123/swagger`

## Verificar la Base de Datos

Para ver la base de datos creada en SQL Server, abra **SQL Server Management Studio** o **Visual Studio**:

1. Abra el **SQL Server Object Explorer** en Visual Studio
2. Conéctese a `(LocalDB)\MSSQLLocalDB`
3. Busque la base de datos `ConferenSpaceDb`
4. Verifique que todas las tablas se hayan creado correctamente

## Resolver Problemas Comunes

### Error: "Microsoft.EntityFrameworkCore could not be found"
Asegúrese de restaurar los paquetes NuGet:
```bash
dotnet restore
```

### Error: "Cannot open database"
Verifique que:
1. SQL Server LocalDB esté corriendo
2. La cadena de conexión en `appsettings.json` sea correcta
3. El nombre de la base de datos sea correcto

### Error: "Migrations history table could not be created"
Esto puede ocurrir si LocalDB no está disponible. Verifique:
```bash
sqllocaldb info
sqllocaldb start MSSQLLocalDB
```

### Limpiar y reiniciar migraciones
Si necesita comenzar de nuevo:

```bash
# Eliminar la migración
dotnet ef migrations remove --project ../ConferenSpace.Infrastructure

# Eliminar la base de datos
dotnet ef database drop --project ../ConferenSpace.Infrastructure

# Crear nueva migración
dotnet ef migrations add InitialCreate --project ../ConferenSpace.Infrastructure

# Actualizar base de datos
dotnet ef database update --project ../ConferenSpace.Infrastructure
```

## Verificar que funciona

Una vez que la API está corriendo, pruebe estos endpoints en Swagger:

1. **GET** `/api/salones` - Debería retornar una lista vacía `[]`
2. **POST** `/api/salones` - Crear un salón de prueba
3. **GET** `/api/salones` - Debería retornar el salón creado

## Datos de Prueba (Opcional)

Puede crear una clase `SeedData` para agregar datos iniciales:

```csharp
// En ConferenSpace.Infrastructure/Data/SeedData.cs
public static class SeedData
{
    public static void Initialize(ConferenSpaceDbContext context)
    {
        if (context.Salones.Any())
            return;

        var salones = new Salon[]
        {
            new Salon { Nombre = "Sala A", Ubicacion = "Piso 1", CapacidadMaxima = 20 },
            new Salon { Nombre = "Sala B", Ubicacion = "Piso 2", CapacidadMaxima = 50 }
        };

        foreach (var salon in salones)
            context.Salones.Add(salon);

        context.SaveChanges();
    }
}
```

Luego llame a `SeedData.Initialize()` en `Program.cs` después de las migraciones.
