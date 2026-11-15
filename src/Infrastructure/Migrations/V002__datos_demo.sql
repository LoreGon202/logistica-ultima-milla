-- V002: datos de demostración con identificadores fijos, para probar el flujo

INSERT OR IGNORE INTO couriers (id, name, route_saturated) VALUES
    ('11111111-1111-1111-1111-111111111111', 'Repartidor Demo Disponible', 0),
    ('22222222-2222-2222-2222-222222222222', 'Repartidor Demo Saturado', 1);

INSERT OR IGNORE INTO deliveries (id, address, courier_id, estado) VALUES
    ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'Calle 10 #20-30, Medellín', NULL, 'Pendiente'),
    ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 'Carrera 43A #5-10, Medellín', NULL, 'Pendiente');