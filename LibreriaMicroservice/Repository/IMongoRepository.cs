using LibreriaMicroservice.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LibreriaMicroservice.Repository
{
    public interface IMongoRepository<TDocument> where TDocument : IDocument
    {
        Task<IEnumerable<TDocument>> GetAll();
        Task<TDocument> GetById(string Id); 
        Task InsertDocument(TDocument Document); //CREATE
        Task UpdateDocument(TDocument Document); //UPDATE
        Task DeleteById(string Id); //DELETE
        Task<PaginationEntity<TDocument>> PaginationBy(
            Expression<Func<TDocument, bool>> FilterExpression,
            PaginationEntity<TDocument> Pagination
            );
        Task<PaginationEntity<TDocument>> PaginationByFilter(
            PaginationEntity<TDocument> Pagination
            );
    }
}
