# 🎓 Tarea: Migración de Monolito a Microservicios

## 🎯 Objetivo
Refactorizar una aplicación monolítica existente hacia una arquitectura de microservicios. Deberán extraer la lógica de negocio y las capas de infraestructura para implementarlas en los esqueletos de microservicios proporcionados.

## 🔗 Repositorios
- **Monolito (Origen):** [github.com/jcmordan/demo-monolitic](https://github.com/jcmordan/demo-monolitic)
- **Microservicios (Destino):** [github.com/jcmordan/demo-micro-service](https://github.com/jcmordan/demo-micro-service)
- **Frontend:** [github.com/jcmordan/demo-web](https://github.com/jcmordan/demo-web)

## 📋 Tareas por Estudiante
1. **Analizar** la funcionalidad asignada en el repositorio monolítico.
2. **Implementar Capa Core (Dominio):** Entidades, DTOs e Interfaces de servicio.
3. **Implementar Capa de Infraestructura:** DbContext propio y Repositorios.
4. **Configurar la API:** Registro de dependencias en `Program.cs` y controladores funcionales con seguridad JWT.

## 👥 Equipos y Servicios Asignados

| Equipo | Servicio Asignado | Carpeta del Proyecto |
| :--- | :--- | :--- |
| **Eduardo / Darlin** | 📅 Booking (Reservas) | `/BookingService` |
| **Adrian & Enrique** | 🏨 Room (Habitaciones) | `/RoomService` |
| **Jonas & Joaquin** | 💳 Payment (Pagos) | `/PaymentService` |
| **Luis & Idaris** | 🔔 Notification (Notificaciones) | `/NotificationService` |

## 🏁 Meta Final
Demostrar que la aplicación **demo-web** sigue funcionando correctamente al conectarse a su nuevo backend de microservicios, manteniendo la misma funcionalidad que tenía con el monolito.

## ⚠️ Notas Importantes
- **Mantén la simplicidad**: Evita sobrecomplicar la implementación. No se requieren validaciones complejas para este ejercicio.
- **Uso de IA**: Evita el uso excesivo de IA para la generación de código. El objetivo de esta tarea es que comprendas los fundamentos de la arquitectura de microservicios. La mejor forma de aprender es implementando la solución por ti mismo.

---
**¡Mucho éxito en el desarrollo! 🚀**
