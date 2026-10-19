# Diagrama C4: Contenedores y Componentes

## Nivel 2: Diagrama de contenedores

Muestra las grandes piezas de la plataforma y cómo se relacionan, ampliando el diagrama de
contexto de la Actividad 1 con un nivel más de detalle.

```mermaid
flowchart TB
    Operador([Operador logístico])
    Repartidor([Repartidor])

    subgraph Plataforma["Plataforma de Gestión Logística (monolito modular)"]
        Pedidos["Módulo Pedidos e Inventario"]
        Entregas["Módulo Gestión de Entregas"]
        Rutas["Módulo Repartidores y Rutas"]
    end

    DB[(Base de datos)]
    ERP[(ERP / CRM interno)]
    Geo[(Servicio de geolocalización)]

    Operador -- "Confirma pedido" --> Pedidos
    Pedidos -- "Pedido confirmado" --> Entregas
    Entregas -- "Consulta disponibilidad" --> Rutas
    Repartidor -- "Actualiza estado de entrega" --> Entregas
    Entregas -- "Persiste datos" --> DB
    Rutas -- "Persiste datos" --> DB
    Pedidos -- "Sincroniza" --> ERP
    Rutas -- "Consulta ubicación" --> Geo
```

## Nivel 3: Diagrama de componentes (flujo "Asignar repartidor a una entrega")

Muestra el detalle interno del módulo de Entregas para el flujo modelado en esta actividad,
siguiendo la separación Domain / Application / Infrastructure.

```mermaid
flowchart TB
    subgraph Application["Application"]
        UseCase["AssignCourierUseCase"]
    end

    subgraph Domain["Domain"]
        Delivery["Delivery (entidad)"]
        DeliveryId["DeliveryId (objeto de valor)"]
        DeliveryAddress["DeliveryAddress (objeto de valor)"]
    end

    subgraph Infrastructure["Infrastructure"]
        Repo["InMemoryDeliveryRepository"]
        Checker["CourierServiceAvailabilityChecker"]
        Notifier["LoggingDeliveryNotifier"]
    end

    IRepo[["IDeliveryRepository (puerto)"]]
    IChecker[["ICourierAvailabilityChecker (puerto)"]]
    INotifier[["IDeliveryNotifier (puerto)"]]

    UseCase -- "usa" --> Delivery
    UseCase -- "depende de" --> IRepo
    UseCase -- "depende de" --> IChecker
    UseCase -- "depende de" --> INotifier

    Repo -- "implementa" --> IRepo
    Checker -- "implementa" --> IChecker
    Notifier -- "implementa" --> INotifier

    Delivery -- "usa" --> DeliveryId
    Delivery -- "usa" --> DeliveryAddress
```

## Lectura de los diagramas

- En el diagrama de contenedores, el **monolito modular** (decisión de la Actividad 2) se
  representa como una sola plataforma con tres módulos internos, cada uno con su
  responsabilidad, consistente con los bounded contexts definidos en este documento.
- En el diagrama de componentes, se evidencia la **dirección de las dependencias**: tanto
  `Infrastructure` como `Application` apuntan hacia los puertos y hacia `Domain`, pero
  `Domain` no depende de ninguno de los dos. Esto protege la lógica de negocio de los
  detalles técnicos, tal como exige la actividad.
