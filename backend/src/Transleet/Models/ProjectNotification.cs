
using MongoDB.Bson;

namespace Transleet.Models;

public record ProjectNotification(ObjectId Id, NotificationOperation Operation);
