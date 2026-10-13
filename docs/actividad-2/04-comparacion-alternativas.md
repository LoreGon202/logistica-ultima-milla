# Comparación de alternativas estructurales

Se evaluaron dos formas de estructurar la plataforma de gestión logística, considerando el
problema, los actores, los riesgos y los escenarios de calidad definidos previamente.

## Opción A: Monolito modular con límites claros

Toda la plataforma se construye como una sola aplicación, pero organizada internamente en
módulos bien definidos y separados por dominio: pedidos, inventario, entregas e incidencias.
Cada módulo tiene su propia responsabilidad, aunque comparten el mismo proceso y el mismo
despliegue.

**Ventajas**
- Más rápido de construir para una operación con crecimiento moderado.
- Permite definir dominios claros (pedido, inventario, entregas, incidencias) sin la
  complejidad de separarlos en servicios independientes.
- Reduce la complejidad operativa inicial: un solo sistema que desplegar y monitorear.
- Facilita la trazabilidad y el mantenimiento, al tener la información en un mismo lugar.

**Desventajas**
- Puede convertirse en una base de código acoplada si los límites entre módulos no se
  respetan con disciplina.
- El crecimiento fuerte de la operación puede exigir refactorización o separación de
  módulos más adelante.

## Opción B: Arquitectura distribuida por dominios y servicios

Cada dominio (inventario, entregas, soporte, etc.) se implementa como un servicio
independiente, con su propio despliegue y, potencialmente, su propia base de datos,
comunicándose entre sí mediante APIs o eventos.

**Ventajas**
- Separa mejor inventario, entregas y soporte como responsabilidades independientes.
- Permite que distintos equipos trabajen y desplieguen cada módulo por separado.
- Facilita la integración con varios sistemas externos de forma más desacoplada.

**Desventajas**
- Aumenta considerablemente la complejidad operativa (más piezas que desplegar, monitorear
  y mantener coordinadas).
- Exige más coordinación técnica, más infraestructura y más experiencia del equipo.
- Puede sobrediseñarse: introducir esta complejidad sin que el negocio todavía la necesite.

## Tabla comparativa

| Criterio | Opción A: Monolito modular | Opción B: Arquitectura distribuida |
|---|---|---|
| Complejidad | Baja-media | Alta |
| Costo inicial | Bajo | Alto |
| Rapidez de implementación | Alta | Baja-media |
| Facilidad de evolución | Media-alta (si se mantienen los límites) | Alta (a costa de más esfuerzo operativo) |
| Riesgo operativo | Bajo | Medio-alto |
| Alineación con el negocio actual | Alta | Media (adelantada a la necesidad real) |

## Relación con los escenarios de calidad

- **ESC-05** (agregar un nuevo canal sin tocar otros módulos) y **ESC-06** (corregir rutas sin
  afectar pedidos) se pueden cumplir en ambas opciones, siempre que los límites de los módulos
  se respeten; la Opción A los cumple con menor esfuerzo de infraestructura.
- **ESC-03** (1.000 transacciones por hora) es alcanzable con un monolito modular bien
  diseñado; no es un volumen que, por sí solo, justifique pasar a servicios distribuidos.
- La Opción B ofrecería ventajas reales si la empresa ya tuviera múltiples equipos trabajando
  en paralelo o necesidades de escalar un dominio de forma independiente de los demás, algo
  que no se evidencia en el diagnóstico actual (Actividad 1).
