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
https://youtu.be/tP8b1zUzyGM


## Actividad 4: Construcción, validación y defensa final

### Qué se implementó
El flujo **asignar repartidor a una entrega**, diseñado en la Actividad 3, como un vertical
slice ejecutable: entrada HTTP, caso de uso, dominio y persistencia en SQLite, con pruebas
unitarias, de integración y de arquitectura.

### Arquitectura
```
src/
├── Domain/           → Delivery, objetos de valor e invariantes
├── Application/      → AssignCourierUseCase, puertos y errores de aplicación
├── Infrastructure/   → adaptadores SQLite y scripts de migración
└── Api/              → entrada HTTP (solo traduce errores a códigos HTTP)
tests/
├── Domain.Tests/           → pruebas unitarias de las invariantes
└── Api.IntegrationTests/   → pruebas de integración y de arquitectura
```
Las dependencias apuntan hacia adentro: `Domain` no depende de nadie, `Application` solo
depende de `Domain`, e `Infrastructure` implementa los puertos de `Application`.

### Requisitos
- .NET SDK 10 (comprobar con `dotnet --version`)
- No hace falta instalar ninguna base de datos: SQLite es un archivo.

### Cómo ejecutarlo
1. Clonar el repositorio y entrar a la carpeta:
```
git clone https://github.com/LoreGon202/logistica-ultima-milla.git
cd logistica-ultima-milla
```
2. Arrancar la API (crea `logistica.db` y aplica las migraciones al iniciar):
```
dotnet run --project src/Api --urls http://localhost:5080
```
3. En otra terminal de PowerShell, probar el flujo con los datos de demostración:
```
curl.exe -i http://localhost:5080/health
curl.exe -i http://localhost:5080/deliveries/aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa
'{"courierId":"11111111-1111-1111-1111-111111111111"}' | Out-File -Encoding ascii body-ok.json
curl.exe -i -X POST http://localhost:5080/deliveries/aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa/assign-courier -H "Content-Type: application/json" --data "@body-ok.json"
```

### Datos de demostración (migración V002)
| Tipo | Identificador | Estado |
|---|---|---|
| Entrega | `aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa` | Pendiente |
| Entrega | `bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb` | Pendiente |
| Repartidor | `11111111-1111-1111-1111-111111111111` | Disponible |
| Repartidor | `22222222-2222-2222-2222-222222222222` | Ruta saturada |

### Respuestas de la API
| Situación | HTTP | Código |
|---|---|---|
| Asignación exitosa | 200 | — |
| Repartidor vacío | 400 | `INVALID_COURIER` |
| Entrega inexistente | 404 | `DELIVERY_NOT_FOUND` |
| Entrega ya asignada | 409 | `DELIVERY_ALREADY_ASSIGNED` |
| Repartidor no disponible | 409 | `COURIER_NOT_AVAILABLE` |
| Error inesperado | 500 | mensaje genérico |

### Cómo ejecutar las pruebas
```
dotnet test
```
Resultado esperado: **13 pruebas correctas** (4 unitarias del dominio, 5 de integración del
flujo y 4 de arquitectura).

### Documentación de la Actividad 4
- [Riesgos, observabilidad y resultados](./docs/actividad-4/01-riesgos-observabilidad-resultados.md)
- [ADR-003: Decisiones de implementación](./docs/actividad-4/02-adr-003-decisiones-implementacion.md)

### Video de demostración y defensa final
[Enlace al video](PEGA-AQUÍ-TU-ENLACE-CUANDO-LO-TENGAS)

