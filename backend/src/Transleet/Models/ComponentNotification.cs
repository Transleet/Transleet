using MongoDB.Bson;

namespace Transleet.Models;

public record ComponentNotification(ObjectId Id, NotificationOperation Operation);
