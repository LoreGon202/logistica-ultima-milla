# Flujo de negocio y glosario

## Flujo elegido: Asignar repartidor a una entrega

### Descripción
Una vez que un pedido ha sido confirmado (inventario validado) y preparado, debe asignarse
a un repartidor disponible para iniciar la entrega. Este flujo cubre el momento en que el
sistema decide qué repartidor atenderá una entrega específica.

### Actores
- **Administrador de almacén / operador logístico**: inicia la asignación de la entrega una vez el pedido está listo.
- **Repartidor**: recibe la asignación y queda vinculado a la entrega.
- **Sistema**: valida las reglas del negocio antes de confirmar la asignación.

### Propósito
Garantizar que cada entrega quede vinculada a un único repartidor disponible, evitando
asignaciones duplicadas o a repartidores con rutas saturadas.

### Resultado esperado
La entrega queda con un repartidor asignado, lista para iniciar su recorrido. Si la
asignación no es posible (entrega inexistente, ya asignada, o repartidor no disponible), el
sistema rechaza la operación sin modificar el estado de la entrega.

## Glosario de lenguaje ubicuo

| Término | Significado en este dominio |
|---|---|
| **Entrega** | Unidad de trabajo logístico asociada a un pedido confirmado, que debe ser llevada a una dirección específica. |
| **Repartidor** | Persona responsable de transportar una entrega desde el punto de despacho hasta el cliente. |
| **Asignar** | Acción de vincular una entrega a un repartidor específico, de forma exclusiva. |
| **Ruta** | Conjunto de entregas agrupadas que un repartidor debe cubrir en un recorrido. |
| **Disponibilidad** | Condición de un repartidor para recibir una nueva entrega, según su carga actual de ruta. |
| **Estado de entrega** | Situación actual de una entrega dentro de su ciclo de vida (pendiente, asignada, en ruta, entregada, incidencia). |
| **Incidencia** | Evento que interrumpe el flujo normal de una entrega (dirección incorrecta, cliente ausente, etc.). |

## Términos ambiguos a confirmar
- **"Disponibilidad" del repartidor**: no está definido aún si se mide por número máximo de
  entregas activas, por zona geográfica, por horario de turno, o una combinación de estos.
  Se asume, para esta actividad, que un repartidor "no disponible" es aquel que ya tiene una
  ruta marcada como saturada (ver invariantes en el modelo de dominio).
- **"Entrega" vs. "Pedido"**: se asume que un pedido puede generar una o más entregas (por
  ejemplo, si se despacha en varios paquetes), aunque en el alcance de esta actividad se
  modela la relación de forma simple, una entrega por pedido, para no sobrediseñar un caso
  que el negocio aún no ha confirmado.
