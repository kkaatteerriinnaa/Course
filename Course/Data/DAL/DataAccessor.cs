using Course.Services.Kdf;
using Course.Data.DAL;
using Course.Data;
using Course.Services.Kdf;

namespace Course.Data.DAL
{
    public class DataAccessor
    {
        private readonly Object _dbLocker = new Object();
        private readonly DataContext _dataContext;
        private readonly IKdfService _kdfService;
        public UserDao UserDao { get; private set; }
        public ContentDao ContentDao { get; private set; }

        public DataAccessor(DataContext dataContext, IKdfService kdfService)
        {
            _dataContext = dataContext;
            _kdfService = kdfService;
            UserDao = new UserDao(dataContext, kdfService, _dbLocker);
            ContentDao = new(_dataContext, _dbLocker);
        }
    }
}
