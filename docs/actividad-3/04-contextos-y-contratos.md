# Contextos delimitados y contratos

## Bounded contexts identificados

A partir del modelo de dominio general (Actividad 1 y 2), se delimitan los siguientes
contextos relevantes para el flujo de esta actividad:

### Contexto: Gestión de Entregas
Responsable de todo lo relacionado con el ciclo de vida de una entrega: creación, asignación
de repartidor, cambios de estado e incidencias. Es el contexto principal de esta actividad.

**Conceptos propios:** Entrega, EstadoEntrega, Incidencia, DeliveryAddress.

### Contexto: Gestión de Repartidores y Rutas
Responsable de la disponibilidad de los repartidores y la carga de sus rutas. Es consultado
por el contexto de Entregas para saber si un repartidor puede recibir una nueva asignación,
pero no conoce el detalle interno de cada entrega.

**Conceptos propios:** Repartidor, Ruta, Disponibilidad.

### Contexto: Gestión de Pedidos e Inventario
Responsable de validar y confirmar pedidos antes de que generen una entrega. Es anterior al
flujo de esta actividad: una entrega solo existe si un pedido ya fue confirmado en este
contexto.

**Conceptos propios:** Pedido, LíneaPedido, Inventario, Almacén.

## Relación entre contextos (para el flujo de esta actividad)



- El contexto de **Pedidos e Inventario** entrega al de **Entregas** la información mínima
  necesaria para crear una entrega (identificador de pedido, dirección, prioridad). No
  comparte su modelo interno completo (líneas de pedido, inventario).
- El contexto de **Entregas**, al ejecutar la asignación, **consulta** (no modifica
  directamente) al contexto de **Repartidores y Rutas** para saber si un repartidor está
  disponible. Esta consulta se modela como un puerto (`ICourierAvailabilityChecker`), no
  como una llamada directa a las entidades de ese contexto.

## Contrato de intercambio (para el flujo de asignación)

| De | A | Qué intercambia | Mecanismo propuesto |
|---|---|---|---|
| Gestión de Entregas | Gestión de Repartidores y Rutas | Solicita si un repartidor (`courierId`) está disponible. | Puerto `ICourierAvailabilityChecker`, implementado por un adaptador en infraestructura. |
| Gestión de Entregas | (resultado hacia quien invoque el caso de uso) | Confirmación de asignación exitosa o motivo de rechazo. | Resultado del caso de uso `AssignCourierUseCase` (sin exponer detalles internos de la entidad `Delivery`). |

## Por qué se delimitan así

Separar estos tres contextos evita que el flujo de asignación de entregas dependa
directamente del detalle interno de inventario o de la lógica de rutas: solo necesita
**preguntar** si un repartidor está disponible, no **saber cómo** se calcula esa
disponibilidad. Esto es consistente con la decisión de la Actividad 2 (monolito modular con
límites claros): los contextos pueden convivir en la misma aplicación, pero con fronteras de
responsabilidad bien definidas, preparadas para una eventual separación futura.


