# Diagrama de contexto

El siguiente diagrama muestra el sistema de gestión logística en el centro,
junto con los actores y sistemas externos que interactúan con él, y el flujo
principal de información entre ellos.

```mermaid
flowchart TB
    Cliente([Cliente])
    Operador([Operador de tienda])
    Almacen([Administrador de almacén])
    Repartidor([Repartidor])
    Soporte([Soporte al cliente])

    Sistema[["Plataforma de gestión logística<br/>(sistema principal)"]]

    Pagos[(Sistema de pagos)]
    Geo[(Servicio de geolocalización)]
    Notif[(Sistema de notificaciones)]
    ERP[(ERP / CRM interno)]

    Cliente -- "Realiza pedido" --> Sistema
    Sistema -- "Estado del pedido" --> Cliente

    Operador -- "Registra pedidos del canal" --> Sistema
    Almacen -- "Confirma inventario" --> Sistema
    Sistema -- "Solicita validación de stock" --> Almacen

    Sistema -- "Asigna ruta y entrega" --> Repartidor
    Repartidor -- "Actualiza estado de entrega" --> Sistema

    Soporte -- "Consulta incidencias" --> Sistema
    Sistema -- "Info del pedido" --> Soporte

    Sistema -- "Solicita cobro" --> Pagos
    Pagos -- "Confirma pago" --> Sistema

    Sistema -- "Solicita ubicación/ruta" --> Geo
    Geo -- "Datos de ubicación" --> Sistema

    Sistema -- "Envía alertas" --> Notif

    Sistema -- "Sincroniza inventario/clientes" --> ERP
    ERP -- "Datos actualizados" --> Sistema
```

## Descripción del diagrama

- **Centro**: la plataforma de gestión logística, que centraliza pedidos, inventario, rutas y entregas.
- **Actores humanos**: cliente, operador de tienda, administrador de almacén, repartidor y soporte, cada uno interactuando con el sistema según su rol.
- **Sistemas externos**: pagos, geolocalización, notificaciones y el ERP/CRM, con los que el sistema se comunica pero que no forman parte de él.
- **Límite del sistema**: todo lo que está dentro del rectángulo central es responsabilidad de esta plataforma; lo demás son servicios externos con los que solo se integra.
