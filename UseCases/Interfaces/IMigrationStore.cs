namespace UseCases.Interfaces
{
    public interface IMigrationStore
    {
        MigrationState State { get; set; }
    }
}