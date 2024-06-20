using Microsoft.EntityFrameworkCore;

namespace Ex3.DataAccess.Repositories;

public interface IDataRepository
{
    public IDataSet<T> Set<T>() where T : class;
    public Task Save(CancellationToken ct);
}


