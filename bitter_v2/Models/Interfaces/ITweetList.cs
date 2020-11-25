using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace bitter_v2.Models.Interfaces
{
    public interface ITweetList
    {

        public ObservableCollection<Tweet> GetCollection();
        public Task<BaseListModel> ResetList(string userID, string password);

        public void LoadNextChunkOnLastIndex(string userID, string password, int index);

        public Task<BaseListModel> LoadNextChunkAsync(string userID, string password);

    }
}
