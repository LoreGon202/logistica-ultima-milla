# Catálogo de requisitos

## Requisitos funcionales

| ID | Requisito |
|---|---|
| RF-01 | Registrar un pedido desde cualquier canal de venta (tienda, app, marketplace). |
| RF-02 | Validar disponibilidad de inventario antes de confirmar un pedido. |
| RF-03 | Asignar una ruta y un repartidor a cada pedido confirmado. |
| RF-04 | Actualizar y consultar el estado de una entrega en tiempo real. |
| RF-05 | Notificar al cliente sobre cambios relevantes en su pedido (confirmado, en ruta, entregado, incidencia). |
| RF-06 | Registrar y gestionar incidencias (pedido fallido, retraso, reclamo). |
| RF-07 | Sincronizar información de inventario y clientes con el ERP/CRM interno. |
| RF-08 | Procesar el cobro de un pedido a través del sistema de pagos externo. |
| RF-09 | Permitir a soporte al cliente consultar el historial completo de un pedido. |

## Requisitos no funcionales (atributos de calidad)

| ID | Atributo | Descripción |
|---|---|---|
| RNF-01 | Rendimiento | Las consultas de estado de pedido deben responder en tiempos bajos, incluso con muchos pedidos simultáneos. |
| RNF-02 | Disponibilidad | El sistema debe mantenerse operativo durante horario comercial extendido, con mínima interrupción. |
| RNF-03 | Seguridad | La información de clientes y pagos debe protegerse adecuadamente (datos sensibles cifrados). |
| RNF-04 | Mantenibilidad | Debe ser posible corregir errores o ajustar reglas de negocio sin afectar todo el sistema. |
| RNF-05 | Escalabilidad | El sistema debe poder crecer en volumen de pedidos y canales sin rediseñarse por completo. |
| RNF-06 | Facilidad de evolución | Agregar un nuevo canal de venta o un nuevo sistema externo no debe requerir cambios estructurales grandes. |
| RNF-07 | Trazabilidad | Debe ser posible reconstruir el historial completo de cualquier pedido en cualquier momento. |

Este catálogo parte directamente del problema, los actores y los riesgos identificados en la
Actividad 1, y es la base para priorizar, definir escenarios de calidad y comparar alternativas
estructurales en los siguientes documentos.
