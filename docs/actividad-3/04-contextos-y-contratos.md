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
