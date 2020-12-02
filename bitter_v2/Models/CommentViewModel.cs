using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace bitter_v2.Models
{
    public class CommentViewModel : Comment
    {
        public Command ToggleLikeCommand = null;

        public delegate void ProfileClickedDelegate(User user);
        public event ProfileClickedDelegate OnProfileClicked;

        public CommentViewModel():base()
        {
            ToggleLikeCommand = new Command(async x =>
            {
                await ToggleLike(App.User);
            });
        }

        public Command ProfileClicked
        {
            get
            {
                return new Command((x) =>
                {
                    OnProfileClicked?.Invoke(this.User);
                });
            }
        }

    }
}
