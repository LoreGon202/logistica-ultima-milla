-- V001: tablas base del flujo "asignar repartidor a una entrega"

CREATE TABLE IF NOT EXISTS couriers (
    id              TEXT    NOT NULL PRIMARY KEY,
    name            TEXT    NOT NULL,
    route_saturated INTEGER NOT NULL DEFAULT 0 CHECK (route_saturated IN (0, 1))
);

CREATE TABLE IF NOT EXISTS deliveries (
    id         TEXT NOT NULL PRIMARY KEY,
    address    TEXT NOT NULL,
    courier_id TEXT NULL REFERENCES couriers (id),
    estado     TEXT NOT NULL
               CHECK (estado IN ('Pendiente', 'Asignada', 'EnRuta', 'Entregada', 'ConIncidencia'))
);