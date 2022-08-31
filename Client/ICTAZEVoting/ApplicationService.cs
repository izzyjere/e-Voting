using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICTAZEVoting;

internal static class ApplicationService
{
    public static event EventHandler OnCloseClicked;
    public static void NotifyOncloseClicked(object sender, EventArgs args)
    {
        OnCloseClicked?.Invoke(sender, args);
    }
}
