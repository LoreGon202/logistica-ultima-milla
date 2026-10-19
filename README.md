# Logística Última Milla

Plataforma de gestión logística de última milla para una empresa de retail digital.

## Resumen ejecutivo

**Producto:** Plataforma centralizada de gestión logística de última milla.

**Problema principal:** La empresa recibe pedidos desde varios canales de venta,
maneja inventario distribuido en distintos puntos y coordina entregas con diferentes
rutas y repartidores, sin contar con una vista centralizada del estado real de cada
pedido. Esto genera duplicación de información, retrasos, errores operativos y poca
trazabilidad.

**Usuarios:** cliente, operador de comercio o tienda, administrador de almacén,
repartidor y soporte al cliente.

**Objetivos:**
- Centralizar el registro y seguimiento de pedidos provenientes de todos los canales.
- Dar visibilidad en tiempo real del estado de cada pedido a todos los involucrados.
- Reducir errores operativos causados por falta de información compartida.
- Mejorar la trazabilidad y la gestión de incidencias en las entregas.

**Alcance:** la plataforma centraliza pedidos, valida inventario, asigna rutas,
rastrea entregas y gestiona incidencias. No reemplaza el sistema de pagos, el ERP
ni las interfaces de venta existentes; se integra con ellos.

## Documentación de la Actividad 1

La documentación completa del diagnóstico y contexto arquitectónico se encuentra en
[`/docs/actividad-1`](./docs/actividad-1):

- [Análisis del problema](./docs/actividad-1/01-analisis-problema.md)
- [Actores y alcance](./docs/actividad-1/02-actores-y-alcance.md)
- [Diagrama de contexto](./docs/actividad-1/03-diagrama-contexto.md)
- [Riesgos](./docs/actividad-1/04-riesgos.md)
- [Supuestos y decisiones iniciales](./docs/actividad-1/05-supuestos-y-decisiones.md)

## Video de sustentación

https://youtu.be/jjgUap6X9fQ


## Actividad 2: Requisitos y decisión estructural

### Resumen del problema
La plataforma debe centralizar pedidos, inventario, rutas y entregas de una empresa de
retail digital que opera con múltiples canales de venta, resolviendo la falta de trazabilidad
y coordinación identificada en la Actividad 1.

### Requisitos prioritarios
- Registrar pedidos desde cualquier canal y validar inventario antes de confirmarlos (RF-01, RF-02).
- Rastrear el estado de cada pedido en tiempo real (RF-04).
- Gestionar incidencias dentro de la misma plataforma (RF-06).
- Cumplir tiempos de respuesta bajos, alta disponibilidad, trazabilidad completa y facilidad
  de evolución sin reescribir el sistema (RNF-01, RNF-02, RNF-06, RNF-07).

El catálogo completo de requisitos y su priorización está en
[`/docs/actividad-2`](./docs/actividad-2).

### Decisión arquitectónica
Se eligió un **monolito modular con límites claros entre dominios** (pedidos, inventario,
entregas, incidencias), en lugar de una arquitectura distribuida por servicios, dado que el
volumen de operación actual no justifica la complejidad operativa adicional, y los módulos
quedan diseñados para una futura separación si el negocio lo exige más adelante.

El detalle completo de la decisión, alternativas consideradas, justificación y trade-offs
está documentado en el
[ADR-001](./docs/actividad-2/05-adr-decision-arquitectonica.md).

### Documentación de la Actividad 2
- [Catálogo de requisitos](./docs/actividad-2/01-catalogo-requisitos.md)
- [Matriz de priorización](./docs/actividad-2/02-matriz-priorizacion.md)
- [Escenarios de calidad](./docs/actividad-2/03-escenarios-calidad.md)
- [Comparación de alternativas](./docs/actividad-2/04-comparacion-alternativas.md)
- [ADR-001: Decisión arquitectónica](./docs/actividad-2/05-adr-decision-arquitectonica.md)

### Video de sustentación
https://youtu.be/oaRi0-4lsQ8


## Actividad 3: Diseño y dominio

### Flujo modelado
Se modeló el flujo **asignar repartidor a una entrega**, que vincula una entrega existente
(sin repartidor) a un repartidor disponible, protegiendo las reglas del negocio.

### Modelo de dominio
- **Entidades:** Cliente, Pedido, Entrega, Repartidor, Incidencia.
- **Objetos de valor:** DeliveryId, DeliveryAddress, EstadoEntrega, Prioridad.
- **Invariante principal del flujo:** una entrega no puede asignarse a un repartidor si ya
  tiene uno asignado.

El modelo completo, el glosario de lenguaje ubicuo, los escenarios de aceptación y los
bounded contexts están en [`/docs/actividad-3`](./docs/actividad-3).

### Arquitectura del código
El dominio se protege mediante una separación en capas (Domain, Application, Infrastructure)
con inversión de dependencias, documentada en [`Architecture.md`](./Architecture.md).


### Decisiones de diseño
Las decisiones clave (por qué la regla vive en la entidad, por qué se usa un puerto para
consultar disponibilidad, entre otras) están en el
[ADR-002](./docs/actividad-3/07-adr-decisiones-diseno.md).

### Documentación de la Actividad 3
- [Flujo y glosario](./docs/actividad-3/01-flujo-y-glosario.md)
- [Modelo de dominio](./docs/actividad-3/02-modelo-dominio.md)
- [Escenarios de aceptación](./docs/actividad-3/03-escenarios-aceptacion.md)
- [Contextos y contratos](./docs/actividad-3/04-contextos-y-contratos.md)
- [Casos de uso, puertos y adaptadores](./docs/actividad-3/05-casos-de-uso-puertos.md)
- [Diagrama C4](./docs/actividad-3/06-diagrama-c4.md)
- [ADR-002: Decisiones de diseño](./docs/actividad-3/07-adr-decisiones-diseno.md)

### Video de sustentación
[Enlace al video](PEGA-AQUÍ-TU-ENLACE-CUANDO-LO-TENGAS)

