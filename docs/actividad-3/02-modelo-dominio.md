# Modelo de dominio

Este documento describe el modelo de dominio completo de la plataforma (de forma general,
basado en el diagnóstico de las Actividades 1 y 2) y profundiza en el detalle necesario para
el flujo modelado en esta actividad: **asignar repartidor a una entrega**.

## Entidades (tienen identidad propia)

| Entidad | Descripción |
|---|---|
| **Cliente** | Persona que realiza un pedido. Se identifica de forma única aunque dos clientes compartan nombre o dirección. |
| **Pedido** | Solicitud de compra confirmada, con una o más líneas de producto. |
| **Entrega** | Unidad de trabajo logístico derivada de un pedido, con identidad propia e independiente del pedido que la originó. |
| **Repartidor** | Persona que ejecuta entregas. Tiene identidad propia, aunque dos repartidores compartan nombre. |
| **Incidencia** | Evento registrado sobre una entrega, con identidad propia y trazabilidad. |

## Objetos de valor (sin identidad propia, se definen por sus atributos)

| Objeto de valor | Descripción |
|---|---|
| **DeliveryId** | Identificador de una entrega. Dos instancias con el mismo valor representan la misma entrega. |
| **DeliveryAddress** | Dirección de entrega. Se compara por su contenido, no por una identidad propia. |
| **EstadoEntrega** | Estado actual de una entrega (pendiente, asignada, en ruta, entregada, con incidencia). |
| **Prioridad** | Nivel de urgencia de una entrega (normal, alta), usado para ordenar asignaciones. |

## Estados y transiciones de la entidad Entrega
