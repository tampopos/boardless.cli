using UseCases.Interfaces;

namespace UseCases
{
    public class MigrationStore : IMigrationStore
    {
        private volatile MigrationState state = MigrationState.Never;
        private volatile object lockObject = new object();
        public MigrationState State
        {
            get
            {
                lock(this.lockObject)
                {
                    return this.state;
                }
            }
            set
            {
                lock(this.lockObject)
                {
                    this.state = value;
                }
            }
        }
    }
}