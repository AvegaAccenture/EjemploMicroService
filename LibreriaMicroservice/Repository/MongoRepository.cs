using LibreriaMicroservice.Core;
using LibreriaMicroservice.Core.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LibreriaMicroservice.Repository
{
    public class MongoRepository<TDocument> : IMongoRepository<TDocument> where TDocument : IDocument
    {
        private readonly IMongoCollection<TDocument> _collection;
        public MongoRepository(IOptions<MongoSettings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            var db = client.GetDatabase(options.Value.Database);
            _collection = db.GetCollection<TDocument>(GetCollectionName(typeof (TDocument)));
        }

        private protected string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute) documentType.GetCustomAttributes(typeof (BsonCollectionAttribute), true).FirstOrDefault()).CollectionName;
        }

        public async Task<IEnumerable<TDocument>> GetAll()
        {
            return await _collection.Find(p => true).ToListAsync();
        }

        public async Task<TDocument> GetById(string Id)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, Id);
            return await _collection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task InsertDocument(TDocument Document)
        {
            await _collection.InsertOneAsync(Document);
        }

        public async Task UpdateDocument(TDocument Document)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, Document.Id);
            await _collection.FindOneAndReplaceAsync(filter, Document);
        }

        public async Task DeleteById(string Id)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, Id);
            await _collection.FindOneAndDeleteAsync(filter);
        }

        public async Task<PaginationEntity<TDocument>> PaginationBy(Expression<Func<TDocument, bool>> FilterExpression, PaginationEntity<TDocument> Pagination)
        {
            var Sort = Builders<TDocument>.Sort.Ascending(Pagination.Sort); //Defino por default el filtro como Ascendiente.

            if (Pagination.SortDirection == "desc") //En caso de que el usuario busque por Descendiente, lo valido y lo asigno.
            {
                Sort = Builders<TDocument>.Sort.Descending(Pagination.Sort);
            }

            //Defino la data filtrada, como Paginación, cuántos elementos traerá por paginación y desde donde contar
            if (string.IsNullOrEmpty(Pagination.Filter)) 
            {
                Pagination.Data = await _collection.Find(p => true)
                                  .Sort(Sort)
                                  .Skip((Pagination.Page - 1) * Pagination.PageSize)
                                  .Limit(Pagination.PageSize)
                                  .ToListAsync();
            }
            else
            {
                Pagination.Data = await _collection.Find(FilterExpression)
                                  .Sort(Sort)
                                  .Skip((Pagination.Page - 1) * Pagination.PageSize)
                                  .Limit(Pagination.PageSize)
                                  .ToListAsync();
            }

            //Defino la cantidad de páginas total según la cantidad de elementos por páginas.
            long TotalDocuments = await _collection.CountDocumentsAsync(FilterDefinition<TDocument>.Empty);
            var TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(TotalDocuments / Pagination.PageSize)));

            Pagination.PageQuantity = TotalPages;

            return Pagination;
        }

        public async Task<PaginationEntity<TDocument>> PaginationByFilter(PaginationEntity<TDocument> Pagination)
        {
            var Sort = Builders<TDocument>.Sort.Ascending(Pagination.Sort); //Defino por default el filtro como Ascendiente.
            int TotalDocuments = 0;

            if (Pagination.SortDirection == "desc") //En caso de que el usuario busque por Descendiente, lo valido y lo asigno.
            {
                Sort = Builders<TDocument>.Sort.Descending(Pagination.Sort);
            }

            //Defino la data filtrada, como Paginación, cuántos elementos traerá por paginación y desde donde contar
            if (Pagination.FilterValue == null)
            {
                Pagination.Data = await _collection.Find(p => true)
                                  .Sort(Sort)
                                  .Skip((Pagination.Page - 1) * Pagination.PageSize)
                                  .Limit(Pagination.PageSize)
                                  .ToListAsync();

                TotalDocuments = (await _collection.Find(p => true).ToListAsync()).Count();
            }
            else
            {
                var ValueFilter = ".*" + Pagination.FilterValue.Valor + ".*";
                var Filter = Builders<TDocument>.Filter.Regex(Pagination.FilterValue.Propiedad, new MongoDB.Bson.BsonRegularExpression(ValueFilter, "i"));

                Pagination.Data = await _collection.Find(Filter)
                                  .Sort(Sort)
                                  .Skip((Pagination.Page - 1) * Pagination.PageSize)
                                  .Limit(Pagination.PageSize)
                                  .ToListAsync();

                TotalDocuments = (await _collection.Find(Filter).ToListAsync()).Count();
            }

            //Defino la cantidad de páginas total según la cantidad de elementos por páginas.
            //long TotalDocuments = await _collection.CountDocumentsAsync(FilterDefinition<TDocument>.Empty);
            var TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(TotalDocuments / Pagination.PageSize)));

            Pagination.PageQuantity = TotalPages;
            Pagination.TotalRows = Convert.ToInt32(TotalDocuments);

            return Pagination;
        }
    }
}
