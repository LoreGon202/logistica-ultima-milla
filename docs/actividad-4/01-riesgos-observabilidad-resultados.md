# Riesgos, observabilidad y resultados

## 1. Riesgos de seguridad y operación

Cada riesgo se relaciona con una mitigación y con la evidencia que permite verificarla.

| # | Riesgo | Mitigación implementada | Evidencia de verificación |
|---|---|---|---|
| R1 | Inyección SQL al consultar o guardar entregas | Todas las consultas usan parámetros (`$id`, `$address`, ...) y nunca se arma el SQL concatenando texto. | Revisión del código en `SqliteDeliveryRepository.cs` y `SqliteCourierAvailabilityChecker.cs`. |
| R2 | Fuga de datos personales en los registros (la dirección de entrega es dato personal) | Los registros escriben solo identificadores (`DeliveryId`, `CourierId`) y nunca la dirección ni nombres. | Registros mostrados en la ejecución de la API y de las pruebas de integración. |
| R3 | Que el cliente reciba detalles internos de un fallo | En la asignación, los errores inesperados se registran con detalle pero al cliente solo se le responde "Ocurrió un error inesperado" (HTTP 500). | Respuesta 500 observada cuando la base de datos aún no tenía tablas. |
| R4 | Que las capas se mezclen con el tiempo y el dominio dependa de la infraestructura | Comprobación automática de dependencias entre capas. | 4 pruebas de arquitectura en `ArchitectureTests.cs`, todas en verde. |
| R5 | Que la migración no cree las tablas y nadie se entere | Las pruebas de integración crean una base de datos nueva con las mismas migraciones; si faltara una tabla, las pruebas fallan. | Las 5 pruebas de integración pasan. Este riesgo ocurrió realmente durante el desarrollo, cuando un script quedó como carpeta y la base quedó vacía. |
| R6 | Que se acepten peticiones inválidas | La entrada valida que el identificador del repartidor no esté vacío (HTTP 400). | Prueba `Asignar_con_repartidor_vacio_responde_400`. |
| R7 | Entrega asignada dos veces por error | La regla vive dentro de la entidad `Delivery`, y el sistema responde 409 al intentar repetirla. | Pruebas unitarias del dominio y prueba de integración `Asignar_dos_veces_la_misma_entrega_responde_409`. |
| R8 | Asignar a un repartidor con ruta saturada | El caso de uso consulta la disponibilidad antes de asignar y responde 409 sin modificar la entrega. | Prueba `Asignar_repartidor_saturado_responde_409_y_no_cambia_la_entrega`. |

## 2. Señales de observabilidad

Qué permite diagnosticar el flujo sin exponer datos sensibles:

- **Registros con identificadores:** cada resultado del flujo deja una línea con `DeliveryId` y, cuando aplica, `CourierId`.
- **Niveles de registro con significado:**
  - `Information`: asignación exitosa.
  - `Warning`: entrega no encontrada, repartidor no disponible, entrega ya asignada.
  - `Error`: fallo inesperado, con la excepción completa solo en el registro interno.
- **Código de error estable en las respuestas:** `DELIVERY_NOT_FOUND`, `DELIVERY_ALREADY_ASSIGNED`, `COURIER_NOT_AVAILABLE`, `INVALID_COURIER`. Permiten contar y clasificar fallos sin leer mensajes.
- **Endpoint de salud:** `GET /health` responde `{"status":"ok"}` para saber si la API está viva.

**Datos que se excluyen a propósito de los registros:** dirección de entrega, nombres de repartidores y cualquier dato del cliente.

## 3. Resultados

| Conjunto de pruebas | Cantidad | Resultado |
|---|---|---|
| Pruebas unitarias del dominio | 4 | Todas correctas |
| Pruebas de integración del flujo | 5 | Todas correctas |
| Pruebas de arquitectura | 4 | Todas correctas |
| **Total** | **13** | **13 correctas, 0 con errores** |

El flujo cubre el camino correcto y cuatro casos de error o límite: entrega inexistente (404), entrega ya asignada (409), repartidor saturado (409) y entrada inválida (400).

## 4. Riesgos residuales

Riesgos que siguen presentes y que no se resolvieron en esta actividad:

- **Concurrencia:** si dos peticiones asignan la misma entrega exactamente al mismo tiempo, ambas podrían leerla sin repartidor y la última en guardar prevalecería. La regla del dominio protege las peticiones secuenciales, pero no hay control de concurrencia en la base de datos.
- **Detalles de error al consultar una entrega:** el endpoint de consulta (`GET /deliveries/{id}`) no tiene manejo de errores propio. En el entorno de desarrollo, un fallo de base de datos mostró el detalle técnico de la excepción en la respuesta.
- **Sin autenticación:** los endpoints no exigen identificarse.
- **Sin reintentos:** si la base de datos no está disponible, la operación falla de forma controlada pero no se reintenta.
- **Datos de demostración mezclados con las migraciones:** el script `V002` carga entregas y repartidores de ejemplo, que no deberían existir en un entorno real.
- **El migrador no informa qué ejecutó**, lo que dificulta detectar un fallo silencioso.

## 5. Próximos pasos

1. Agregar control de concurrencia (por ejemplo, una actualización condicionada en la base de datos).
2. Agregar manejo de errores a la consulta de entregas y ocultar el detalle técnico fuera de desarrollo.
3. Agregar autenticación y autorización a los endpoints.
4. Separar los datos de demostración en una migración que solo se ejecute en desarrollo.
5. Hacer que el migrador registre qué scripts ejecutó.
6. Incorporar métricas y trazas (por ejemplo, con OpenTelemetry): conteo de errores 409 por código y tiempo de respuesta de la asignación.
7. Reemplazar SQLite por una base de datos de servidor cuando el volumen lo justifique.
