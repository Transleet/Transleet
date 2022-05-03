
using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;

namespace Transleet.Models;

public class Role : MongoIdentityRole<ObjectId>
{

}
