# Supuestos y decisiones iniciales

## Supuestos
Cosas que damos por ciertas por ahora, sin haberlas confirmado formalmente con la empresa:

- Los canales de venta (tienda física, app, marketplace) ya existen y seguirán operando de forma independiente; la plataforma no los reemplaza, solo centraliza su información.
- El ERP/CRM interno cuenta con algún mecanismo (API o exportación de datos) que permite integrarse con la nueva plataforma.
- Cada almacén puede reportar su inventario de forma digital, aunque sea básica.
- Los repartidores cuentan con un dispositivo móvil con conexión a internet para actualizar el estado de las entregas.
- El sistema de pagos y el de geolocalización ya existen como servicios externos independientes y no se van a construir desde cero.

## Decisiones iniciales
Decisiones que ya se toman en esta etapa, antes de entrar en detalles técnicos:

- Se opta por una **plataforma centralizada** en lugar de conectar cada canal de venta por separado con cada almacén, para eliminar duplicidad de información.
- El sistema será el punto único de verdad sobre el estado de cada pedido, aunque los datos de origen (inventario, pagos, ubicación) vivan en sistemas externos.
- La gestión de incidencias (pedidos fallidos, retrasos, reclamos) se maneja dentro de la plataforma, y no como procesos externos o manuales.
- No se construirán módulos que dupliquen funciones ya cubiertas por sistemas externos (pagos, geolocalización, ERP), sino que la plataforma se integrará con ellos.

## Qué aún no se sabe / qué habrá que confirmar luego
- Con qué tecnología o protocolo exacto se integrará cada sistema externo (eso corresponde a actividades posteriores de diseño técnico).
- Volumen real de pedidos y repartidores, necesario para dimensionar la solución.
- Si todos los almacenes tienen el mismo nivel de digitalización de su inventario.
- Nivel de servicio (SLA) esperado para tiempos de entrega y disponibilidad del sistema.
