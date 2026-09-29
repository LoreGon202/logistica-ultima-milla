# Riesgos

A continuación se listan los principales riesgos identificados para el sistema,
es decir, los puntos donde puede fallar o generar pérdidas para el negocio.

| Riesgo | Descripción | Impacto |
|---|---|---|
| Duplicación de pedidos | Dos canales de venta ofrecen el mismo producto sin sincronización, generando pedidos que no se pueden cumplir. | Insatisfacción del cliente, costos operativos. |
| Inconsistencias de inventario | La cantidad registrada en el sistema no coincide con la disponible en almacén. | Pedidos aceptados sin stock real. |
| Entregas fallidas o retrasadas | Mala asignación de rutas o repartidores, o fallas en la coordinación con geolocalización. | Pérdida de confianza del cliente. |
| Pérdida de trazabilidad | No se puede determinar con certeza en qué etapa está un pedido. | Soporte no puede resolver reclamos, mala experiencia de usuario. |
| Mala coordinación entre almacén, rutas y soporte | Falta de comunicación entre áreas que dependen de información actualizada del sistema. | Retrasos y errores operativos en cadena. |
| Crecimiento operativo sin control | El sistema no está preparado para escalar a más canales, almacenes o volumen de pedidos. | Degradación del servicio a medida que crece la operación. |

## Por qué importan estos riesgos ahora
Aunque todavía no se ha diseñado la solución técnica, identificar estos riesgos desde
esta etapa permite que las decisiones de arquitectura posteriores (en las siguientes
actividades) se tomen pensando en mitigarlos, y no se descubran ya avanzado el desarrollo.
