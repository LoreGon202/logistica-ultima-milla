# ADR-002: Decisiones de diseño del dominio y flujo de asignación de repartidor

## Estado
Aceptado

## Contexto
A partir de la decisión arquitectónica de la Actividad 2 (monolito modular con límites claros
entre dominios), se requiere diseñar en detalle el modelo de dominio de la plataforma y un
flujo de negocio concreto: **asignar un repartidor a una entrega**. El diseño debe proteger
las reglas del negocio (invariantes) y mantener el dominio independiente de la infraestructura,
siguiendo los principios de puertos y adaptadores e inversión de dependencias.

## Decisiones tomadas

### 1. La regla de negocio vive en la entidad, no en el caso de uso
Se decidió que la invariante "una entrega no puede asignarse a un repartidor si ya tiene uno
asignado" se proteja dentro del método `AssignCourier` de la entidad `Delivery`, y no como una
validación suelta en `AssignCourierUseCase`.

**Alternativa descartada:** validar esta regla directamente en el caso de uso, antes de
llamar a la entidad.

**Por qué se descartó:** dejar la regla en el caso de uso permitiría que, en el futuro, otro
caso de uso modifique la entidad sin pasar por esa validación, rompiendo la invariante. Al
protegerla dentro de la entidad, la regla se cumple sin importar desde dónde se invoque.

### 2. La disponibilidad del repartidor se consulta mediante un puerto, no una API concreta
Se decidió introducir `ICourierAvailabilityChecker` como puerto, en lugar de que el caso de
uso llame directamente a un servicio HTTP o a las entidades del contexto de Repartidores y
Rutas.

**Alternativa descartada:** que `AssignCourierUseCase` dependiera directamente de las clases
internas del módulo de Repartidores y Rutas.

**Por qué se descartó:** acoplaría el contexto de Entregas al detalle interno de otro
contexto, dificultando que evolucionen por separado (contradice los bounded contexts
definidos en el documento 04). Con el puerto, el contexto de Repartidores y Rutas puede
cambiar su implementación interna sin afectar el flujo de asignación.

### 3. Una entrega por pedido (relación simple), en lugar de modelar múltiples entregas por pedido
Se decidió modelar, para esta actividad, una relación 1 a 1 entre pedido y entrega.

**Alternativa descartada:** modelar desde ya que un pedido puede generar varias entregas
(por ejemplo, por despachos parciales).

**Por qué se descartó:** el negocio no ha confirmado que ese escenario sea necesario
actualmente (ver "términos ambiguos" en el documento 01). Introducir esa complejidad ahora
sería sobrediseñar sin evidencia de que se necesita, lo cual contradice la misma lógica usada
en la Actividad 2 para descartar microservicios prematuros.

### 4. Los adaptadores de esta actividad son simples (en memoria / registro básico)
Se decidió implementar `InMemoryDeliveryRepository` y `LoggingDeliveryNotifier` como
versiones simples, sin conexión real a base de datos ni a servicios externos.

**Por qué:** esta actividad evalúa el diseño del dominio y sus límites, no la integración
real con infraestructura. Esa integración corresponde a la Actividad 4, y los puertos ya
definidos permiten reemplazar estos adaptadores simples por implementaciones reales sin
tocar el dominio ni el caso de uso.

## Trade-offs aceptados
- Se gana
