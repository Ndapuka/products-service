namespace eCommerce.BusinessLogicLayer.RabbitMQ;

public record ProductDeletionMessage(Guid ProductID, String? ProductName);
