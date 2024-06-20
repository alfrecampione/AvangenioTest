using Microsoft.EntityFrameworkCore;

namespace Ex3.DataAccess.Repositories;

public class DataRepository : IDataRepository
{
    private readonly ApplicationDbContext context;

    public DataRepository(ApplicationDbContext context) => this.context = context;

    public IDataSet<T> Set<T>() where T : class
    {
        return new DataSet<T>(context);
    }

    public async Task Save(CancellationToken ct)
    {

        //  await context.SaveChangesAsync(ct);
        try
        {
            await context.SaveChangesAsync(ct);
        }
        catch (Exception e)
        {

            throw new DataSetTypeExeption($"Unable to save changes", e);
        }
    }
}