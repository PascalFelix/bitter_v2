using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace bitter_v2.Models
{
    public abstract class BaseListModel : BaseModel
    {
        protected int offset = 0;
        protected int offsetIncrement = 10;
        public const int OffsetInt = 10;

        protected bool _currentlyRefreshing = false;

        protected bool _firstLoadDone = false;

        public void ResetOffset()
        {
            offsetIncrement = OffsetInt;
            offset = OffsetInt * -1;
        }

        public List<string> Ids = new List<string>();
        public ObservableCollection<BaseModel> Collection = new ObservableCollection<BaseModel>();

        public BaseListModel()
        {
            ResetList();
        }

        public virtual void ResetList()
        {
            this.Ids.Clear();
            this.Collection.Clear();
            ResetOffset();
        }

        public async Task<BaseListModel> LoadNextChunkAsync(string userID, string password)
        {
            offset += offsetIncrement;
            return await LoadAsync(userID, password, offset.ToString());
        }

        public abstract Task<BaseListModel> LoadAsync(string userID, string password, string offset);

        public virtual  ObservableCollection<BaseModel> GetCollection()
        {
            return Collection;
        }
        public virtual async Task<BaseListModel> ResetList(string userID, string password)
        {
            ResetList();
            _currentlyRefreshing = true;
            var tmp = await LoadNextChunkAsync(userID, password);
            _currentlyRefreshing = false;
            return tmp;
        }

        public virtual async void LoadNextChunkOnLastIndex(string userID, string password, int index)
        {
            if (index + 1 >= Collection.Count && _firstLoadDone && !_currentlyRefreshing)
            {
                _currentlyRefreshing = true;
                await LoadNextChunkAsync(userID, password);
                _currentlyRefreshing = false;
            }
        }

    }
}
