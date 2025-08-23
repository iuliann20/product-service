# Product Service - Event-Driven Architecture Fix

## Problemele găsite și soluțiile implementate

### 1. **IDomainEvent nu implementa INotification**
**Problema**: `IDomainEvent` nu extindea `INotification` din MediatR, ceea ce împiedica domain events să fie publicate prin MediatR.

**Soluție**: Am modificat interfața să extindă `INotification`:
```csharp
public interface IDomainEvent : INotification { }
```

### 2. **Interceptorul nu era înregistrat în DbContext**
**Problema**: `ConvertDomainEventsToOutboxMessagesInterceptor` era creat dar nu era adăugat în configurația DbContext.

**Soluție**: Am înregistrat interceptorul ca service și l-am adăugat în DbContext:
```csharp
services.AddScoped<ConvertDomainEventsToOutboxMessagesInterceptor>();
services.AddDbContext<ProductServiceDbContext>((sp, optionsBuilder) =>
{
    optionsBuilder.UseSqlServer(connectionString);
    optionsBuilder.AddInterceptors(sp.GetRequiredService<ConvertDomainEventsToOutboxMessagesInterceptor>());
});
```

### 3. **Logică duplicată pentru domain events**
**Problema**: Aveai procesarea domain events atât în `UnitOfWork` cât și în interceptor, ceea ce cauza evenimente duplicate.

**Soluție**: Am eliminat logica din `UnitOfWork` și am păstrat-o doar în interceptor pentru o sursă unică de adevăr.

### 4. **Inconsistență în tipurile pentru Outbox**
**Problema**: În interceptor foloseai `GetType().Name` dar în OutboxDispatcher aveai nevoie de `AssemblyQualifiedName` pentru deserializare.

**Soluție**: Am standardizat să folosesc `AssemblyQualifiedName` în interceptor pentru compatibilitate cu deserializarea.

### 5. **MassTransit Outbox Configuration**
**Problema**: Aveai `UseInMemoryOutbox` în configurația MassTransit ceea ce interferează cu outbox-ul custom.

**Soluție**: Am eliminat `UseInMemoryOutbox` din configurația MassTransit pentru a folosi outbox pattern-ul custom.

### 6. **Domain Event Handlers**
**Soluție**: Am adăugat domain event handlers pentru logare și monitorizare:
- `ProductCreatedDomainEventHandler`
- `ProductUpdatedDomainEventHandler`
- `StockAdjustedDomainEventHandler`
- `ProductDeactivatedDomainEventHandler`

## Fluxul evenimentelor acum

1. **Domain Events**: Când se fac modificări pe entități (Product), se ridică domain events
2. **Interceptor**: Interceptorul convertește domain events în integration events și le salvează în tabela OutboxMessages
3. **OutboxDispatcher**: Background service-ul ia evenimentele din outbox și le publică în RabbitMQ via MassTransit
4. **Consumers**: Alte servicii consumă evenimentele și procesează logic business-ul (ex: actualizare stock la order placed/cancelled)

## Configurația RabbitMQ

Configurația din `appsettings.json` este corectă:
```json
"RabbitMq": {
  "Host": "rabbitmq",
  "Username": "guest", 
  "Password": "guest",
  "VirtualHost": "/"
}
```

## Testare

Pentru a testa funcționarea:

1. **Creează un produs** → Va ridica `ProductCreatedDomainEvent` → Va crea `ProductCreatedIntegrationEvent` în outbox → Va fi trimis în RabbitMQ
2. **Actualizează stock** → Va ridica `StockAdjustedDomainEvent` → Va crea `StockAdjustedIntegrationEvent` în outbox → Va fi trimis în RabbitMQ
3. **Primește order placed/cancelled** → Va consuma evenimentele și va actualiza stock-ul

## Notă importantă

Asigură-te că:
- RabbitMQ server rulează și este accesibil
- Connection string-ul pentru database este corect criptat
- Tabela `OutboxMessages` există în database (prin migrări)
- `OutboxDispatcher` background service rulează
