namespace FireAlt.Mono
{
    public interface IDataPersistence<T> where T : DataModel
    {
        void LoadData(T data);

        void SaveData(T data);
    }
}