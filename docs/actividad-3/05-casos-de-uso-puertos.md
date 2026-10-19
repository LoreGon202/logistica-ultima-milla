# Casos de uso, puertos y adaptadores

## Caso de uso principal: Asignar repartidor a una entrega

**Nombre:** `AssignCourierUseCase`

**Intención:** vincular una entrega existente, sin repartidor asignado, a un repartidor
disponible, protegiendo las invariantes del dominio.

**Pasos:**
1. Cargar la entrega mediante `IDeliveryRepository`.
2. Verificar que la entrega exista y no tenga repartidor activo (lo protege la propia entidad `Delivery`).
3. Consultar disponibilidad del repartidor mediante un puerto (`ICourierAvailabilityChecker`), no mediante una API concreta.
4. Ejecutar `delivery.AssignCourier(courierId)` para proteger la invariante dentro de la entidad.
5. Guardar la entrega actualizada mediante el repositorio.
6. Registrar o publicar el resultado mediante un adaptador de notificación/registro.

**Importante:** el caso de uso coordina la intención, pero no contiene la regla de negocio en
sí misma; la regla ("no se puede asignar si ya tiene repartidor") vive dentro de la entidad
`Delivery`, no en el caso de uso ni en infraestructura.

## Puertos (interfaces que expresan lo que el caso de uso necesita)

| Puerto | Responsabilidad | Quién lo implementa |
|---|---|---|
| `IDeliveryRepository` | Cargar y guardar una entrega por su identificador. | Adaptador de persistencia (Infrastructure). |
| `ICourierAvailabilityChecker` | Responder si un repartidor está disponible para una nueva entrega. | Adaptador que consulta al contexto de Repartidores y Rutas. |
| `IDeliveryNotifier` | Registrar o notificar el resultado de una asignación. | Adaptador de notificaciones/registro (Infrastructure). |

## Adaptadores (implementaciones concretas, en Infrastructure)

| Adaptador | Implementa | Detalle técnico que oculta al dominio |
|---|---|---|
| `InMemoryDeliveryRepository` (o `SqlDeliveryRepository` a futuro) | `IDeliveryRepository` | Cómo y dónde se guarda la entrega (memoria, base de datos). |
| `CourierServiceAvailabilityChecker` | `ICourierAvailabilityChecker` | Cómo se consulta la disponibilidad real (API interna, otro módulo, servicio externo a futuro). |
| `LoggingDeliveryNotifier` | `IDeliveryNotifier` | Cómo se registra el resultado (log, cola de eventos, etc.). |

En esta actividad, los adaptadores se dejan como implementaciones simples (en memoria o de
registro básico), ya que la integración real con infraestructura corresponde a la Actividad 4.

## Dirección de las dependencias



- **Domain** no depende de nada externo: solo contiene `Delivery`, sus objetos de valor y sus invariantes.
- **Application** (el caso de uso) depende de **Domain** y de los **puertos** (interfaces), nunca de una implementación concreta.
- **Infrastructure** depende de **Application** (implementa sus puertos) y, si es necesario, de **Domain**, pero nunca al revés.

Esto cumple el principio de inversión de dependencias (la "D" de SOLID): las capas internas
(Domain, Application) no conocen ni dependen de los detalles técnicos externos.

## Otros casos de uso identificados (fuera del alcance detallado de esta actividad)

- `CreateDeliveryUseCase`: crear una entrega a partir de un pedido confirmado.
- `ReportIncidentUseCase`: registrar una incidencia sobre una entrega en curso.
- `CompleteDeliveryUseCase`: marcar una entrega como completada.

Estos se mencionan para mostrar coherencia con el modelo de dominio completo, pero solo
`AssignCourierUseCase` se modela y documenta en profundidad en esta actividad.

