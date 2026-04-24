# Documentación de API - ConferenSpace

## Base URL
```
https://localhost:7123/api
```

## Autenticación
Actualmente, la API no requiere autenticación. En producción, se recomienda implementar JWT.

## Respuestas Estándar

### Exitosa (2xx)
```json
{
  "data": {...},
  "mensaje": "Operación exitosa",
  "exitoso": true
}
```

### Error (4xx/5xx)
```json
{
  "mensaje": "Descripción del error",
  "tipo": "TipoDeError"
}
```

---

## 📍 Endpoints - SALONES

### Obtener todos los salones
```http
GET /api/salones
```

**Respuesta (200 OK)**
```json
[
  {
    "id": 1,
    "nombre": "Sala A",
    "ubicacion": "Piso 3, Ala Oeste",
    "capacidadMaxima": 50,
    "serviciosIntegrados": "Proyector, Pizarra digital",
    "estaActivo": true,
    "fechaCreacion": "2024-01-15T10:30:00Z"
  }
]
```

---

### Obtener salón por ID
```http
GET /api/salones/{id}
```

**Parámetros**
- `id` (int): ID del salón

**Respuesta (200 OK)**
```json
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

---

### Crear nuevo salón
```http
POST /api/salones
Content-Type: application/json
```

**Body**
```json
{
  "nombre": "Sala A",
  "ubicacion": "Piso 3, Ala Oeste",
  "capacidadMaxima": 50,
  "serviciosIntegrados": "Proyector, Pizarra digital"
}
```

**Respuesta (201 Created)**
```json
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

---

### Actualizar salón
```http
PUT /api/salones/{id}
Content-Type: application/json
```

**Parámetros**
- `id` (int): ID del salón a actualizar

**Body**
```json
{
  "nombre": "Sala A - Actualizada",
  "ubicacion": "Piso 3, Ala Este",
  "capacidadMaxima": 60,
  "serviciosIntegrados": "Proyector, Pizarra digital, Videoconferencia"
}
```

**Respuesta (200 OK)**
```json
{
  "id": 1,
  "nombre": "Sala A - Actualizada",
  "ubicacion": "Piso 3, Ala Este",
  "capacidadMaxima": 60,
  "serviciosIntegrados": "Proyector, Pizarra digital, Videoconferencia",
  "estaActivo": true,
  "fechaCreacion": "2024-01-15T10:30:00Z"
}
```

---

### Poner salón fuera de servicio
```http
PATCH /api/salones/{id}/fuera-de-servicio
```

**Parámetros**
- `id` (int): ID del salón

**Respuesta (200 OK)**
```json
{
  "mensaje": "Salón puesto fuera de servicio correctamente."
}
```

---

### Reactivar salón
```http
PATCH /api/salones/{id}/reactivar
```

**Parámetros**
- `id` (int): ID del salón

**Respuesta (200 OK)**
```json
{
  "mensaje": "Salón reactivado correctamente."
}
```

---

### Obtener salones disponibles
```http
GET /api/salones/disponibles?fecha=2024-02-20&horaInicio=14:00:00&horaFin=16:00:00&capacidadMinima=20
```

**Parámetros Query**
- `fecha` (DateTime): Fecha de la reserva
- `horaInicio` (TimeSpan): Hora de inicio (formato HH:mm:ss)
- `horaFin` (TimeSpan): Hora de fin (formato HH:mm:ss)
- `capacidadMinima` (int): Capacidad mínima requerida

**Respuesta (200 OK)**
```json
[
  {
    "id": 1,
    "nombre": "Sala A",
    "ubicacion": "Piso 3",
    "capacidadMaxima": 50,
    "serviciosIntegrados": "Proyector",
    "estaActivo": true,
    "fechaCreacion": "2024-01-15T10:30:00Z"
  }
]
```

---

### Eliminar salón
```http
DELETE /api/salones/{id}
```

**Parámetros**
- `id` (int): ID del salón

**Respuesta (200 OK)**
```json
{
  "mensaje": "Salón eliminado correctamente."
}
```

**Errores**
- `404` - Salón no encontrado
- `400` - El salón tiene reservas asociadas

---

## 👤 Endpoints - SOLICITANTES

### Obtener todos los solicitantes
```http
GET /api/solicitantes
```

**Respuesta (200 OK)**
```json
[
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
]
```

---

### Crear nuevo solicitante
```http
POST /api/solicitantes
Content-Type: application/json
```

**Body**
```json
{
  "nombreCompleto": "Juan Pérez",
  "telefono": "+34 912 34 56 78",
  "correo": "juan.perez@empresa.com",
  "departamento": "Ventas",
  "numeroIdentificacion": "12345678A"
}
```

**Respuesta (201 Created)**
```json
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

---

### Buscar solicitantes por nombre
```http
GET /api/solicitantes/buscar/nombre/Juan
```

**Parámetros**
- `nombre` (string): Nombre a buscar (búsqueda parcial)

**Respuesta (200 OK)**
```json
[
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
]
```

---

### Buscar solicitantes por departamento
```http
GET /api/solicitantes/buscar/departamento/Ventas
```

**Parámetros**
- `departamento` (string): Departamento a buscar

**Respuesta (200 OK)**
```json
[...]
```

---

### Buscar solicitante por correo
```http
GET /api/solicitantes/buscar/correo?correo=juan.perez@empresa.com
```

**Parámetros Query**
- `correo` (string): Correo electrónico exacto

**Respuesta (200 OK)**
```json
{
  "id": 1,
  "nombreCompleto": "Juan Pérez",
  ...
}
```

---

## 📦 Endpoints - RECURSOS

### Obtener todos los recursos
```http
GET /api/recursos
```

**Respuesta (200 OK)**
```json
[
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
]
```

---

### Crear nuevo recurso
```http
POST /api/recursos
Content-Type: application/json
```

**Body**
```json
{
  "nombre": "Proyector portátil",
  "descripcion": "Proyector para presentaciones",
  "cantidadTotal": 5,
  "cantidadDisponible": 5,
  "costoUnitario": 200.00
}
```

**Respuesta (201 Created)**
```json
{
  "id": 1,
  "nombre": "Proyector portátil",
  "descripcion": "Proyector para presentaciones",
  "cantidadTotal": 5,
  "cantidadDisponible": 5,
  "costoUnitario": 200.00,
  "estaActivo": true,
  "fechaCreacion": "2024-01-05T09:15:00Z"
}
```

---

### Obtener cantidad disponible de recurso
```http
GET /api/recursos/{id}/disponibilidad
```

**Parámetros**
- `id` (int): ID del recurso

**Respuesta (200 OK)**
```json
{
  "cantidad": 3
}
```

---

### Obtener recursos disponibles
```http
GET /api/recursos/disponibles?fecha=2024-02-20&horaInicio=14:00:00&horaFin=16:00:00&cantidadMinima=2
```

**Parámetros Query**
- `fecha` (DateTime): Fecha
- `horaInicio` (TimeSpan): Hora de inicio
- `horaFin` (TimeSpan): Hora de fin
- `cantidadMinima` (int): Cantidad mínima requerida

**Respuesta (200 OK)**
```json
[...]
```

---

## 📅 Endpoints - RESERVAS

### Crear nueva reserva
```http
POST /api/reservas
Content-Type: application/json
```

**Body**
```json
{
  "solicitanteId": 1,
  "salonId": 1,
  "fecha": "2024-02-20",
  "horaInicio": "14:00:00",
  "horaFin": "16:00:00",
  "proposito": "Reunión de junta directiva",
  "cantidadAsistentes": 25,
  "notas": "Confirmar catering con 24 horas",
  "recursos": [
    {
      "recursoId": 1,
      "cantidadSolicitada": 2
    }
  ]
}
```

**Respuesta (201 Created)**
```json
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
  "notas": "Confirmar catering con 24 horas",
  "recursos": [
    {
      "recursoId": 1,
      "cantidadSolicitada": 2,
      "recurso": {...}
    }
  ],
  "fechaCreacion": "2024-02-15T10:00:00Z"
}
```

**Errores**
- `400` - El salón no está disponible o recursos insuficientes
- `400` - La cantidad de asistentes excede la capacidad

---

### Obtener todas las reservas
```http
GET /api/reservas
```

**Respuesta (200 OK)**
```json
[...]
```

---

### Obtener reserva por ID
```http
GET /api/reservas/{id}
```

**Parámetros**
- `id` (int): ID de la reserva

**Respuesta (200 OK)**
```json
{...}
```

---

### Actualizar reserva
```http
PUT /api/reservas/{id}
Content-Type: application/json
```

**Body**: Mismo que crear reserva

**Respuesta (200 OK)**
```json
{...}
```

---

### Cancelar reserva
```http
PATCH /api/reservas/{id}/cancelar
```

**Parámetros**
- `id` (int): ID de la reserva

**Respuesta (200 OK)**
```json
{
  "mensaje": "Reserva cancelada correctamente."
}
```

---

### Obtener reservas por solicitante
```http
GET /api/reservas/solicitante/{solicitanteId}
```

**Parámetros**
- `solicitanteId` (int): ID del solicitante

**Respuesta (200 OK)**
```json
[...]
```

---

### Obtener reservas de un salón en una fecha
```http
GET /api/reservas/salon/{salonId}?fecha=2024-02-20
```

**Parámetros**
- `salonId` (int): ID del salón
- `fecha` (DateTime): Fecha a consultar

**Respuesta (200 OK)**
```json
[...]
```

---

### Obtener disponibilidad de salón (calendario)
```http
GET /api/reservas/salon/{salonId}/disponibilidad?fechaInicio=2024-02-01&fechaFin=2024-02-29
```

**Parámetros**
- `salonId` (int): ID del salón
- `fechaInicio` (DateTime): Fecha inicio
- `fechaFin` (DateTime): Fecha fin

**Respuesta (200 OK)**
```json
[
  {
    "salonId": 1,
    "fecha": "2024-02-20",
    "horaInicio": "00:00:00",
    "horaFin": "14:00:00",
    "estaDisponible": true,
    "razon": null
  },
  {
    "salonId": 1,
    "fecha": "2024-02-20",
    "horaInicio": "14:00:00",
    "horaFin": "16:00:00",
    "estaDisponible": false,
    "razon": "Reserva existente"
  },
  {
    "salonId": 1,
    "fecha": "2024-02-20",
    "horaInicio": "16:00:00",
    "horaFin": "24:00:00",
    "estaDisponible": true,
    "razon": null
  }
]
```

---

### Validar disponibilidad
```http
POST /api/reservas/validar-disponibilidad?salonId=1&fecha=2024-02-20&horaInicio=14:00:00&horaFin=16:00:00
```

**Parámetros Query**
- `salonId` (int): ID del salón
- `fecha` (DateTime): Fecha
- `horaInicio` (TimeSpan): Hora inicio
- `horaFin` (TimeSpan): Hora fin

**Respuesta (200 OK)**
```json
{
  "disponible": true,
  "mensaje": "El salón está disponible"
}
```

o

```json
{
  "disponible": false,
  "mensaje": "El salón no está disponible"
}
```

---

## 🔢 Códigos de Estado HTTP

| Código | Significado |
|--------|------------|
| 200 | OK - Operación exitosa |
| 201 | Created - Recurso creado |
| 204 | No Content - Sin contenido |
| 400 | Bad Request - Solicitud inválida |
| 404 | Not Found - Recurso no encontrado |
| 409 | Conflict - Conflicto (ej: doble booking) |
| 500 | Internal Server Error - Error del servidor |

---

## ⚠️ Estados de Reserva

| Valor | Significado |
|-------|------------|
| 0 | Pendiente |
| 1 | Confirmada |
| 2 | Cancelada |

---

## 🧪 Ejemplo de Flujo Completo

### 1. Crear un solicitante
```bash
curl -X POST https://localhost:7123/api/solicitantes \
  -H "Content-Type: application/json" \
  -d '{
    "nombreCompleto": "Juan Pérez",
    "telefono": "+34 912 34 56 78",
    "correo": "juan.perez@empresa.com",
    "departamento": "Ventas",
    "numeroIdentificacion": "12345678A"
  }'
```

### 2. Crear un salón
```bash
curl -X POST https://localhost:7123/api/salones \
  -H "Content-Type: application/json" \
  -d '{
    "nombre": "Sala A",
    "ubicacion": "Piso 3",
    "capacidadMaxima": 50,
    "serviciosIntegrados": "Proyector, Pizarra"
  }'
```

### 3. Crear un recurso
```bash
curl -X POST https://localhost:7123/api/recursos \
  -H "Content-Type: application/json" \
  -d '{
    "nombre": "Proyector",
    "descripcion": "Proyector para presentaciones",
    "cantidadTotal": 5,
    "cantidadDisponible": 5,
    "costoUnitario": 200
  }'
```

### 4. Validar disponibilidad
```bash
curl https://localhost:7123/api/salones/disponibles?fecha=2024-02-20&horaInicio=14:00:00&horaFin=16:00:00&capacidadMinima=20
```

### 5. Crear una reserva
```bash
curl -X POST https://localhost:7123/api/reservas \
  -H "Content-Type: application/json" \
  -d '{
    "solicitanteId": 1,
    "salonId": 1,
    "fecha": "2024-02-20",
    "horaInicio": "14:00:00",
    "horaFin": "16:00:00",
    "proposito": "Reunión",
    "cantidadAsistentes": 20,
    "recursos": [{"recursoId": 1, "cantidadSolicitada": 1}]
  }'
```

---

**Documentación de API - ConferenSpace**  
*Última actualización: 2024*
