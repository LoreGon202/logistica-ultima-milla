# ADR-003: Decisiones de implementación del flujo "asignar repartidor"

## Estado
Aceptado

## Contexto
La Actividad 3 dejó el diseño del flujo (dominio, casos de uso, puertos y adaptadores) con
adaptadores simples en memoria. La Actividad 4 exige implementarlo como un vertical slice
ejecutable: entrada, caso de uso, dominio y persistencia real, con pruebas y sin romper los
límites entre capas.

Este ADR registra las decisiones de implementación y señala cuáles **ajustan** lo que se
había diseñado en la Actividad 3.

## Decisiones

### 1. Persistencia con SQLite y ADO.NET (Microsoft.Data.Sqlite)
**Ajuste respecto a la Actividad 3:** el diseño planteaba adaptadores en memoria y mencionaba
una posible implementación con Entity Framework. Se implementó `SqliteDeliveryRepository`
con ADO.NET directo.

- **Alternativas descartadas:** PostgreSQL (requiere instalar y operar un servidor, sin
  beneficio para el volumen actual) y Entity Framework (agrega una herramienta y conceptos
  adicionales para solo dos tablas).
- **Por qué SQLite:** es un archivo, no requiere servidor y mantiene el proyecto reproducible
  en cualquier computador. Es coherente con el ADR-001: no sobrediseñar antes de que el
  volumen lo justifique.
- **Aislamiento:** el dominio y la aplicación no saben que existe SQLite. Solo
  `Infrastructure` lo conoce, y una prueba de arquitectura lo verifica.

### 2. Migraciones como scripts SQL numerados, aplicados al arrancar
Los scripts `V001__crear_tablas_base.sql` y `V002__datos_demo.sql` viajan dentro del
ensamblado y los ejecuta `SqliteMigrator` en orden al iniciar la API.

- **Alternativas descartadas:** migraciones de Entity Framework (depende de la decisión
  anterior) y una herramienta externa como Flyway (agrega un componente que instalar).
- **Por qué:** el esquema es reproducible: cualquier persona que clone el repositorio obtiene
  la misma base de datos. Los scripts usan `IF NOT EXISTS` e `INSERT OR IGNORE`, así que
  pueden ejecutarse varias veces sin romper nada.
- **Costo asumido:** no hay registro de qué migraciones ya se aplicaron. Se acepta mientras
  existan pocos scripts idempotentes (ver riesgos residuales).

### 3. El dominio expone `Restore` y excepciones propias
**Ajuste respecto a la Actividad 3:** se agregó `Delivery.Restore(...)` y
`DeliveryAlreadyAssignedException`.

- `Restore` permite reconstruir una entrega leída de la base de datos sin abrir los
  setters, de modo que el estado solo se cambia a través de métodos que protegen las
  invariantes.
- La excepción propia permite que la entrada distinga "ya asignada" de cualquier otro error.

### 4. Errores de aplicación con nombre propio
**Ajuste respecto a la Actividad 3:** `AssignCourierUseCase` dejó de lanzar
`InvalidOperationException` genérica y ahora lanza `DeliveryNotFoundException` y
`CourierNotAvailableException`.

- **Por qué:** con errores genéricos la entrada no podría responder con el código correcto.
- Las reglas de negocio no se movieron: la invariante sigue viviendo en la entidad.

### 5. La entrada solo traduce errores a HTTP
El endpoint `POST /deliveries/{id}/assign-courier` no contiene reglas de negocio. Traduce
cada excepción a un código HTTP y a un código de error estable:

| Situación | HTTP | Código |
|---|---|---|
| Asignación exitosa | 200 | — |
| Repartidor vacío | 400 | `INVALID_COURIER` |
| Entrega inexistente | 404 | `DELIVERY_NOT_FOUND` |
| Entrega ya asignada | 409 | `DELIVERY_ALREADY_ASSIGNED` |
| Repartidor no disponible | 409 | `COURIER_NOT_AVAILABLE` |
| Error inesperado | 500 | mensaje genérico |

### 6. No se implementa mensajería asíncrona ni colas
El PDF pide justificar qué patrones aplican y cuáles no. Se descartan: colas con
productor-consumidor, Dead Letter Queue, process manager y Event Sourcing.

- **Por qué:** la asignación es una operación corta y síncrona, el volumen de referencia es
  de hasta 1.000 transacciones por hora (ESC-03) y la única integración, el notificador, es
  un registro local que no puede fallar de forma que justifique reintentos.
- **Cuándo se reconsideraría:** si la notificación pasara a un servicio externo real
  (correo, SMS, push) o si el volumen creciera de forma sostenida.

### 7. Comprobación de arquitectura con reflexión, sin librería externa
Las 4 pruebas de `ArchitectureTests.cs` revisan por reflexión qué ensamblados referencia
cada capa.

- **Alternativa descartada:** una librería especializada en reglas de arquitectura.
- **Por qué:** evita una dependencia adicional y cubre las reglas que importan: el dominio no
  depende de nadie, la aplicación no depende de la infraestructura y la infraestructura
  implementa los puertos.
- **Limitación:** solo comprueba referencias entre proyectos, no el uso de tipos concretos
  dentro de un mismo proyecto.

### 8. Pruebas de integración con la API real y una base temporal
Cada prueba arranca la API completa con su propia base de datos SQLite en una carpeta
temporal, creada por las mismas migraciones. Así se prueba el recorrido entrada, caso de
uso, dominio y persistencia sin tocar la base de desarrollo.

## Trade-offs aceptados
- Se gana simplicidad y reproducibilidad con SQLite y scripts, a costa de no tener control de
  concurrencia fino ni un historial de migraciones aplicadas.
- Se evita sobrediseñar sin mensajería, a costa de que, si la notificación se vuelve
  externa, habrá que revisar esa decisión.
- Se evita una dependencia con la comprobación de arquitectura por reflexión, a costa de una
  cobertura más limitada que la de una librería dedicada.

## Consecuencias
- Los adaptadores `InMemoryDeliveryRepository` y `CourierServiceAvailabilityChecker` de la
  Actividad 3 se conservan en el código pero la API ya no los usa.
- Cambiar de base de datos solo exige escribir nuevos adaptadores para los puertos
  `IDeliveryRepository` y `ICourierAvailabilityChecker`. El dominio y el caso de uso no
  cambian.
- Esta decisión se revisa si cambia el volumen, si aparece más de una instancia de la API
  que comparta datos, o si el notificador deja de ser local.
