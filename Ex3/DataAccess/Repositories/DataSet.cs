
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Collections;

namespace Ex3.DataAccess.Repositories;


public class DataSet<T> : IDataSet<T> where T : class
{

    private readonly DbContext context;

    public DataSet(DbContext context) => this.context = context;

    public IQueryable<T> Data => context.Set<T>();

    public Type ElementType => Data.ElementType;

    public Expression Expression => Data.Expression;

    public IQueryProvider Provider => Data.Provider;

    public async Task Create(T item, CancellationToken ct = default)
    {
       // await context.Set<T>().AddAsync(item, ct);
       try
       {
           
           await context.Set<T>().AddAsync(item, ct);
       }
       catch (Exception e)
       {
           throw new DataSetTypeExeption($"Unable to add the element:{item.GetType()}", e);
       }
    }

    public void Update(T item)
    {
       // context.Set<T>().Update(item);
       try
       {
           context.Set<T>().Update(item);
       }
       catch (Exception e)
       {
           
           throw new Exception($"Unable to update the element:{item.GetType()}", e);
       }
    }

    public void Remove(T item)
    {
       // context.Set<T>().Remove(item);
       try
       {
           context.Set<T>().Remove(item);
       }
       catch (Exception e)
       {
           
           throw new Exception($"Unable to remove the element:{item.GetType()}", e);
       }
    }

    public IEnumerator<T> GetEnumerator()
    {
        return Data.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)Data).GetEnumerator();
    }
}