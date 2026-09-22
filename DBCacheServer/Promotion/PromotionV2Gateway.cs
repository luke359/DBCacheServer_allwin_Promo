using System;
using System.Collections.Generic;
using System.Linq;
using Promotion.Core.Data;
using Promotion.Data.MySql;
using AdapterException = Promotion.Core.Data.PromotionDataException;

namespace DBCacheServer
{
    // Kept in the Host project so the Adapter does not reference this executable.
    public sealed class PromotionV2Gateway : IPromotionV2Gateway
    {
        private readonly MysqlAcess mysql;
        public PromotionV2Gateway(MysqlAcess mysql) => this.mysql = mysql ?? throw new ArgumentNullException(nameof(mysql));

        public T Transaction<T>(Func<IPromotionV2Transaction, T> action)
        {
            try { return mysql.ExecuteParameterizedTransaction(context => action(new TransactionBridge(context))); }
            catch (PromotionDataException ex) { throw Map(ex); }
        }

        public IReadOnlyList<IReadOnlyDictionary<string, object>> Select(Query query)
        {
            Validate(query);
            try { return mysql.SelectParameterizedV2(Convert(query)); }
            catch (PromotionDataException ex) { throw Map(ex, query.Table); }
        }

        public long Delete(DeleteQuery query)
        {
            PromotionTableMetadata.Validate(query);
            try { return mysql.DeleteParameterizedV2(Convert(query)); }
            catch (PromotionDataException ex) { throw Map(ex, query.Table); }
        }

        private static AdapterException Map(PromotionDataException ex, string table = null) =>
            new((global::Promotion.Core.Data.PromotionDataErrorKind)(byte)ex.Kind, ex.Message, ex,
                PromotionTableMetadata.KnownConstraint(table, ex.ConstraintName));

        private static void Validate(Query query) => PromotionTableMetadata.Validate(query);
        private static MysqlCondition Convert(Condition value) =>
            new(value.Column, (MysqlComparisonOperator)(byte)value.Operator, value.Value);
        private static MysqlSelectCommand Convert(Query value) =>
            new(value.Table, value.Fields, value.Where.Select(Convert).ToArray(),
                value.Order.Select(x => new MysqlOrder(x.Column, x.Descending ? MysqlSortDirection.Descending : MysqlSortDirection.Ascending)).ToArray(),
                value.Limit, value.Offset, value.ForUpdate ? MysqlLockMode.ForUpdate : MysqlLockMode.None);
        private static MysqlDeleteCommand Convert(DeleteQuery value) =>
            new(value.Table, value.Where.Select(Convert).ToArray(),
                value.Order.Select(x => new MysqlOrder(x.Column, x.Descending ? MysqlSortDirection.Descending : MysqlSortDirection.Ascending)).ToArray(),
                value.Limit);

        private sealed class TransactionBridge : IPromotionV2Transaction
        {
            private readonly MysqlTransactionContext context;
            public TransactionBridge(MysqlTransactionContext context) => this.context = context;
            public IReadOnlyList<IReadOnlyDictionary<string, object>> Select(Query query)
            {
                Validate(query);
                try { return context.Select(Convert(query)); }
                catch (PromotionDataException ex) { throw Map(ex, query.Table); }
            }
            public InsertResult Insert(string table, IReadOnlyDictionary<string, object> values)
            {
                PromotionTableMetadata.Validate(table, values.Keys);
                try
                {
                    var result = context.Insert(table, values);
                    return new InsertResult(result.AffectedRows, result.LastInsertedId);
                }
                catch (PromotionDataException ex) { throw Map(ex, table); }
            }
            public long Update(string table, IReadOnlyDictionary<string, object> values, IReadOnlyList<Condition> where)
            {
                PromotionTableMetadata.Validate(table, values.Keys.Concat(where.Select(x => x.Column)));
                if (where.Count == 0) throw new ArgumentException("Update requires conditions.", nameof(where));
                try { return context.Update(table, values, where.Select(Convert).ToArray()); }
                catch (PromotionDataException ex) { throw Map(ex, table); }
            }
            public long Count(string table, IReadOnlyList<Condition> where)
            {
                PromotionTableMetadata.Validate(table, where.Select(x => x.Column));
                try { return System.Convert.ToInt64(context.ExecuteScalar(new MysqlScalarCommand(table,
                    MysqlScalarOperation.Count, null, where.Select(Convert).ToArray()))); }
                catch (PromotionDataException ex) { throw Map(ex, table); }
            }
            public long Delete(DeleteQuery query)
            {
                PromotionTableMetadata.Validate(query);
                try { return context.Delete(Convert(query)); }
                catch (PromotionDataException ex) { throw Map(ex, query.Table); }
            }
        }
    }
}
