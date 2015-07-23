using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace WebApp.Helpers
{
    public static class JobifyMongoCollectionExtensions
    {
        public static async Task<TDocument> FindById<TDocument>(this IMongoCollection<TDocument> collection, Expression<Func<TDocument, bool>> filter) where TDocument: class
        {
            var id = GetValueFromLambda(filter);
            var isValid = CheckIfIdIsValid(id);
            if (isValid)
            {
                return await collection.Find(filter).SingleOrDefaultAsync();
            }

            return await Task.FromResult<TDocument>((TDocument)null);
        }

        private static string GetValueFromLambda<TDocument>(Expression<Func<TDocument, bool>> expression)
        {
            var body = expression.Body as BinaryExpression;
            if (body != null)
            {
                var right = body.Right as MemberExpression;
                if (right != null)
                {
                    var value = GetValue(right);
                    return value;
                }
            }
            return String.Empty;
        }

        private static string GetValue(MemberExpression member)
        {
            var objectMember = Expression.Convert(member, typeof(string));
            var getterLambda = Expression.Lambda<Func<string>>(objectMember);
            return getterLambda.Compile()();
        }

        private static bool CheckIfIdIsValid(string id)
        {
            MongoDB.Bson.ObjectId oid = new MongoDB.Bson.ObjectId();
            return MongoDB.Bson.ObjectId.TryParse(id, out oid);
        }
    }
}