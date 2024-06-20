namespace Ex3.DataAccess.Repositories;

public class DataSetTypeExeption:Exception
{
    public DataSetTypeExeption(string message, Exception inner)
        : base(message, inner)
    {
        
    }
}


