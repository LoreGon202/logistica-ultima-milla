# Architecture.md — Plataforma de Gestión Logística de Última Milla

## Resumen
Este documento describe la arquitectura de la plataforma logística, consolidando las
decisiones tomadas en las Actividades 1 a 3. Sirve como punto de referencia para entender
cómo está organizado el sistema antes de continuar con la implementación de integraciones
reales en la Actividad 4.

## Contexto del negocio
La plataforma centraliza pedidos, inventario, rutas y entregas para una empresa de retail
digital con múltiples canales de venta, resolviendo la falta de trazabilidad y coordinación
diagnosticada en la Actividad 1. El detalle completo está en
[`/docs/actividad-1`](./docs/actividad-1).

## Decisión estructural
Se eligió un **monolito modular con límites claros entre dominios** (pedidos, inventario,
entregas, incidencias), en lugar de una arquitectura distribuida por servicios, dado que el
volumen de operación actual no justifica la complejidad operativa adicional. El análisis
completo de requisitos y alternativas está en [`/docs/actividad-2`](./docs/actividad-2) y en
el [ADR-001](./docs/actividad-2/05-adr-decision-arquitectonica.md).

## Bounded contexts
El sistema se organiza en tres contextos principales:
- **Gestión de Pedidos e Inventario**
- **Gestión de Entregas**
- **Gestión de Repartidores y Rutas**

Cada uno con responsabilidades propias y comunicación mediante contratos explícitos (no
acceso directo a modelos internos de otro contexto). El detalle está en
[`04-contextos-y-contratos.md`](./docs/actividad-3/04-contextos-y-contratos.md).

## Organización del código


### Regla de dependencias


`Domain` no depende de ninguna otra capa. `Application` depende de `Domain` y de sus propios
puertos (interfaces), nunca de una implementación concreta. `Infrastructure` implementa esos
puertos, pero el dominio y los casos de uso no conocen los detalles técnicos (base de datos,
APIs externas, frameworks). Esto sigue el principio de inversión de dependencias (SOLID).

## Flujo modelado en detalle: Asignar repartidor a una entrega
Caso de uso `AssignCourierUseCase`, que protege la invariante "una entrega no puede
asignarse a un repartidor si ya tiene uno asignado" directamente en la entidad `Delivery`.

El modelo de dominio completo, las invariantes, los escenarios de aceptación
(Dado-Cuando-Entonces) y los diagramas C4 están documentados en
[`/docs/actividad-3`](./docs/actividad-3).

## Decisiones de diseño relevantes
Las decisiones de diseño del dominio (por qué la regla vive en la entidad, por qué se usa un
puerto para consultar disponibilidad, por qué se modela una entrega por pedido, entre otras)
están registradas en el [ADR-002](./docs/actividad-3/07-adr-decisiones-diseno.md).

## Qué queda pendiente para la Actividad 4
- Reemplazar `InMemoryDeliveryRepository` por una implementación real de persistencia.
- Implementar `CourierServiceAvailabilityChecker` contra el módulo real de Repartidores y Rutas.
- Completar la integración de los demás casos de uso identificados (`CreateDeliveryUseCase`,
  `ReportIncidentUseCase`, `CompleteDeliveryUseCase`).
