# Escenarios de aceptación

Estos escenarios describen, en formato Dado-Cuando-Entonces, el comportamiento esperado del
flujo **asignar repartidor a una entrega**, y permiten verificar que las invariantes del
modelo de dominio se cumplen.

## Escenario 1: Asignación exitosa

**Dado** que una entrega existe y no tiene repartidor asignado,
**y** el repartidor indicado está disponible (su ruta no está saturada),
**cuando** se solicita asignar ese repartidor a la entrega,
**entonces** la entrega queda con ese repartidor asignado,
**y** su estado cambia de "Pendiente" a "Asignada".

## Escenario 2: Entrega inexistente

**Dado** que no existe ninguna entrega con el identificador indicado,
**cuando** se solicita asignar un repartidor a esa entrega,
**entonces** el sistema rechaza la operación,
**y** no se realiza ningún cambio de estado.

## Escenario 3: Entrega ya asignada

**Dado** que una entrega ya tiene un repartidor asignado,
**cuando** se solicita asignarle un nuevo repartidor,
**entonces** el sistema rechaza la operación,
**y** el repartidor originalmente asignado se mantiene sin cambios.

## Escenario 4: Repartidor no disponible

**Dado** que una entrega existe y no tiene repartidor asignado,
**y** el repartidor indicado tiene su ruta saturada,
**cuando** se solicita asignar ese repartidor a la entrega,
**entonces** el sistema rechaza la operación,
**y** la entrega permanece en estado "Pendiente".

## Escenario 5: Confirmación del resultado

**Dado** que una entrega fue asignada correctamente a un repartidor,
**cuando** se consulta el resultado de la operación,
**entonces** el sistema confirma o registra la asignación realizada,
**de forma que** quede trazabilidad de qué repartidor fue asignado y cuándo.

---

Los escenarios 1 a 4 corresponden directamente a las invariantes 1 y 2 del modelo de
dominio. El escenario 5 cubre el registro del resultado, relevante para la trazabilidad
exigida desde la Actividad 1.
