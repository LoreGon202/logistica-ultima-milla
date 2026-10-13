# ADR-001: Estructura arquitectónica inicial de la plataforma logística

## Estado
Aceptado

## Contexto
La plataforma de gestión logística de última milla debe centralizar pedidos, inventario,
rutas y entregas para una empresa de retail digital que opera con múltiples canales de venta
y un volumen de operación moderado, con posibilidad de crecer.

A partir del diagnóstico de la Actividad 1 y del catálogo de requisitos de esta actividad, se
identificó la necesidad de una estructura que permita mantener dominios claros (pedidos,
inventario, entregas, incidencias), cumplir escenarios de calidad concretos (tiempos de
respuesta, disponibilidad, trazabilidad) y evolucionar sin reescribirse por completo, sin
incurrir en una complejidad operativa que el negocio todavía no necesita.

## Opciones consideradas

**Opción A: Monolito modular con límites claros entre dominios** (pedidos, inventario,
entregas, incidencias) dentro de una sola aplicación.

**Opción B: Arquitectura distribuida por dominios y servicios**, con cada dominio
desplegado de forma independiente y comunicación mediante APIs o eventos.

El detalle de ventajas, desventajas y la tabla comparativa de ambas opciones se encuentra en
[`04-comparacion-alternativas.md`](./04-comparacion-alternativas.md).

## Decisión
Se elige la **Opción A: Monolito modular con límites claros entre dominios**.

La plataforma se construirá como una sola aplicación, organizada internamente en módulos
independientes por dominio (pedidos, inventario, entregas, incidencias), con interfaces
claras entre ellos, de forma que cada módulo pueda evolucionar o, eventualmente, separarse
en el futuro sin necesidad de rediseñar todo el sistema.

## Justificación
- El volumen de operación actual (hasta 1.000 transacciones por hora, según ESC-03) no
  justifica la complejidad operativa de una arquitectura distribuida.
- Los requisitos de mayor prioridad (RF-01, RF-02, RF-04, RNF-01, RNF-02, RNF-06, RNF-07)
  se pueden cumplir con un monolito modular bien diseñado, siempre que se respeten los
  límites entre dominios.
- Reduce el costo y el tiempo de implementación inicial, algo relevante dado que es la
  primera versión de la plataforma.
- Mantiene una ruta de evolución abierta: si el negocio crece lo suficiente como para
  justificarlo, los módulos ya definidos facilitan una futura separación en servicios
  independientes (Opción B), sin partir de cero.

## Riesgos y trade-offs
- **Riesgo de acoplamiento:** si no se mantiene disciplina en los límites entre módulos
  (pedidos, inventario, entregas, incidencias), el monolito puede volverse difícil de
  mantener con el tiempo. *Mitigación:* definir contratos claros entre módulos desde el
  diseño, aunque compartan el mismo despliegue.
- **Riesgo de refactorización futura:** si la operación crece de forma mucho más rápida de
  lo esperado, puede ser necesario separar algunos módulos en servicios independientes más
  adelante. *Mitigación:* diseñar los módulos con límites de dominio ya pensados para esa
  eventual separación.
- **Trade-off aceptado:** se sacrifica la flexibilidad de escalar o desplegar cada dominio
  de forma independiente (que sí ofrecería la Opción B), a cambio de menor complejidad,
  menor costo y mayor velocidad de entrega en esta etapa del proyecto.

## Consecuencias
- El equipo debe definir y respetar límites de módulo claros desde el inicio del diseño
  técnico (próximas actividades).
- La base de datos, en esta primera etapa, puede mantenerse única, pero organizada de forma
  que los datos de cada dominio estén claramente diferenciados.
- Esta decisión deberá revisarse si el negocio evidencia necesidades reales de escalar un
  dominio de forma independiente, o si varios equipos empiezan a trabajar en paralelo sobre
  la misma base de código.
