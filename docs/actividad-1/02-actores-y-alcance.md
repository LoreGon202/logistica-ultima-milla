# Actores y alcance

## Actores

### Personas
- **Cliente**: realiza pedidos y necesita saber el estado de su entrega.
- **Operador de comercio o tienda**: gestiona los pedidos que llegan por su canal.
- **Administrador de almacén**: valida y gestiona el inventario disponible.
- **Repartidor**: ejecuta las entregas y actualiza el estado en ruta.
- **Soporte al cliente**: atiende reclamos e incidencias relacionadas con pedidos.

### Sistemas externos
- **Sistema de pagos**: procesa los cobros asociados a los pedidos.
- **Servicio de geolocalización**: permite rastrear rutas y ubicación de entregas.
- **Sistema de notificaciones**: informa a clientes y repartidores sobre cambios de estado.
- **ERP o CRM interno**: contiene información de inventario y clientes ya existente en la empresa.

## Alcance

### Qué SÍ incluye esta plataforma
- Registro centralizado de pedidos provenientes de todos los canales de venta.
- Validación de inventario disponible antes de confirmar un pedido.
- Asignación de rutas y repartidores.
- Seguimiento del estado de cada pedido en tiempo real.
- Gestión de incidencias (pedidos fallidos, retrasos, reclamos).

### Qué NO incluye esta plataforma (por ahora)
- El procesamiento de pagos en sí (se apoya en el sistema de pagos existente).
- La gestión de fabricación o reposición de inventario (se apoya en el ERP).
- La app o interfaz de venta al cliente final (se asume que ya existe por canal).

Definir estos límites evita que la plataforma intente resolver problemas que
ya son responsabilidad de otros sistemas de la empresa.
