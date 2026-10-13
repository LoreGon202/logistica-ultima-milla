# Escenarios de calidad

Un escenario de calidad convierte un requisito no funcional en algo concreto y medible,
con una condición clara y una métrica verificable. Esto permite comprobar después si la
arquitectura elegida realmente cumple lo que promete.

| ID | Atributo relacionado | Escenario | Condición | Métrica / medida |
|---|---|---|---|---|
| ESC-01 | Rendimiento (RNF-01) | Un cliente consulta el estado de su pedido. | En condiciones normales de carga. | La respuesta debe entregarse en menos de 2 segundos. |
| ESC-02 | Disponibilidad (RNF-02) | La plataforma opera durante el horario comercial. | Durante un mes calendario. | El sistema debe estar disponible al menos el 99% del tiempo. |
| ESC-03 | Escalabilidad (RNF-05) | La empresa recibe un pico de pedidos en temporada alta. | Hasta 1.000 transacciones por hora. | El sistema debe procesarlas sin degradar el tiempo de respuesta. |
| ESC-04 | Seguridad (RNF-03) | Se almacena o transmite información de pago o datos personales del cliente. | En cualquier momento. | La información sensible debe estar cifrada en tránsito y en reposo. |
| ESC-05 | Facilidad de evolución (RNF-06) | El negocio agrega un nuevo canal de venta (por ejemplo, una nueva app). | Sin interrumpir el servicio actual. | La integración no debe requerir modificar los módulos de inventario, rutas o entregas. |
| ESC-06 | Mantenibilidad (RNF-04) | Se detecta un error en la lógica de asignación de rutas. | Durante operación normal. | Debe poder corregirse sin necesidad de modificar el módulo de pedidos o inventario. |
| ESC-07 | Trazabilidad (RNF-07) | Soporte al cliente necesita reconstruir el historial de un pedido ante un reclamo. | En cualquier momento, incluso días después de la entrega. | El sistema debe mostrar todos los cambios de estado del pedido, con fecha y hora. |

Estos escenarios son los que se deben tener en cuenta al comparar las alternativas
estructurales: una arquitectura que no pueda cumplir, por ejemplo, ESC-05 o ESC-06 sin
reescribir módulos completos, no sería apropiada para este negocio.
