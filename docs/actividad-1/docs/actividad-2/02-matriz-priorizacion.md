# Matriz de priorización de requisitos

La prioridad se define combinando dos factores: qué tan importante es el requisito para el
negocio, y qué tanto impacta en las decisiones de arquitectura. Un requisito con alto impacto
en ambos frentes debe resolverse bien desde el diseño inicial, no se puede "arreglar después".

## Requisitos funcionales

| ID | Requisito | Prioridad | Impacto en negocio | Impacto en arquitectura | Complejidad |
|---|---|---|---|---|---|
| RF-01 | Registrar pedido multicanal | Alta | Alto | Alto | Media |
| RF-02 | Validar inventario | Alta | Alto | Alto | Media |
| RF-03 | Asignar ruta/repartidor | Alta | Alto | Medio | Media |
| RF-04 | Rastrear estado en tiempo real | Alta | Alto | Alto | Alta |
| RF-05 | Notificar al cliente | Media | Medio | Bajo | Baja |
| RF-06 | Gestionar incidencias | Alta | Alto | Medio | Media |
| RF-07 | Sincronizar con ERP/CRM | Media | Medio | Medio | Media |
| RF-08 | Procesar cobro (integración pagos) | Media | Alto | Bajo | Baja |
| RF-09 | Consultar historial (soporte) | Media | Medio | Bajo | Baja |

## Requisitos no funcionales

| ID | Atributo | Prioridad | Impacto en negocio | Impacto en arquitectura | Complejidad |
|---|---|---|---|---|---|
| RNF-01 | Rendimiento | Alta | Alto | Alto | Alta |
| RNF-02 | Disponibilidad | Alta | Alto | Alto | Media |
| RNF-03 | Seguridad | Alta | Alto | Medio | Media |
| RNF-04 | Mantenibilidad | Alta | Medio | Alto | Media |
| RNF-05 | Escalabilidad | Media | Medio | Alto | Alta |
| RNF-06 | Facilidad de evolución | Alta | Alto | Alto | Media |
| RNF-07 | Trazabilidad | Alta | Alto | Medio | Media |

## Lectura de la matriz

Los requisitos de **alta prioridad con alto impacto en arquitectura** (RF-01, RF-02, RF-04,
RNF-01, RNF-02, RNF-04, RNF-06, RNF-07) son los que más deben pesar al momento de elegir la
estructura del sistema: todos apuntan a la necesidad de centralizar información, mantener
trazabilidad y permitir que el sistema evolucione sin reescribirse por completo. Esto es
justamente lo que se evalúa al comparar alternativas estructurales en el siguiente documento.
