# Análisis del problema

## ¿Qué ocurre hoy?
La empresa recibe pedidos desde varios canales de venta (tienda física, app, marketplace),
maneja inventario distribuido en distintos puntos, y coordina las entregas con diferentes
rutas y repartidores. No existe una vista centralizada que muestre en tiempo real el estado
real de cada pedido.

## ¿Por qué es un problema?
Al no existir información centralizada, cada área (ventas, almacén, soporte, repartidores)
maneja su propia versión de la realidad. Esto genera descoordinación entre lo que se vende,
lo que hay en inventario y lo que realmente se puede entregar.

## ¿Qué consecuencias tiene?
- Pedidos duplicados cuando dos canales venden el mismo producto sin saber que el otro ya lo vendió.
- Inconsistencias de inventario entre lo que el sistema cree que hay y lo que realmente hay en bodega.
- Entregas fallidas o retrasadas por mala asignación de rutas o repartidores.
- Pérdida de trazabilidad: nadie puede responder con certeza "¿dónde está mi pedido?".
- Soporte al cliente sin información confiable para resolver reclamos.

## ¿Qué variables importan?
- Canal de origen del pedido (tienda, app, marketplace).
- Ubicación y disponibilidad real del inventario.
- Ruta y repartidor asignado.
- Estado del pedido en cada etapa (recibido, en preparación, en ruta, entregado, incidencia).
- Tiempo transcurrido en cada etapa, para detectar demoras.
