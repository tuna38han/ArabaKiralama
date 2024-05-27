using ArabaKirala.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ArabaKirala.API.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    DbSet<T> Table { get; } 
}